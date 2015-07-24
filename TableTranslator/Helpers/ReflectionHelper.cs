using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using TableTranslator.Model.Settings;

namespace TableTranslator.Helpers
{
    internal static class ReflectionHelper
    {
        internal const BindingFlags DEFAULT_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        private static readonly IEnumerable<MemberTypes> DEFAULT_MEMBER_TYPES = new List<MemberTypes> { MemberTypes.Property, MemberTypes.Field };

        /// <summary>
        /// Gets the member from a LambdaExpression
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        internal static MemberInfo GetMemberInfoFromLambda(LambdaExpression lambda)
        {
            var body = lambda.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("The body of the lambda expression must be a MemberExpression.", "lambda");
            }

            return body.Member;
        }

        /// <summary>
        /// Gets the type of the member info
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        internal static Type GetMemberType(this MemberInfo memberInfo)
        {
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.PropertyType;

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
                return fieldInfo.FieldType;

            return null;
        }

        internal static Type GetPureType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        internal static bool IsNullAssignable(this Type type)
        {
            // string can be null even though it doesn't implement Nullable<> and is a value type
            return Nullable.GetUnderlyingType(type) != null || !type.IsValueType || type == typeof(string);
        }

        internal static IEnumerable<Type> GetTraversedGenericTypes(this Type type)
        {
            return type.GetGenericArguments().Traverse(x => x.GetGenericArguments());
        }

        private static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
        {
            var queue = new Queue<T>(items);
            while (queue.Any())
            {
                var next = queue.Dequeue();
                yield return next;
                foreach (var child in childSelector(next))
                    queue.Enqueue(child);
            }
        }

        public static string GetFormattedName(this Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            var sb = new StringBuilder();
            sb.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
            sb.Append('<');
            var appendComma = false;
            foreach (var arg in t.GetGenericArguments())
            {
                if (appendComma) sb.Append(',');
                sb.Append(GetFormattedName(arg));
                appendComma = true;
            }
            sb.Append('>');
            return sb.ToString();
        }

        /// <summary>
        /// Gets the default value for the given type (reference or value types)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Gets the value of the member from an object using the members relative path
        /// </summary>
        /// <param name="relativePropertyPath">Relavtive path to the object from the object passed in</param>
        /// <param name="obj">Object to get the member value from</param>
        /// <returns></returns>
        internal static object GetMemberValue(string relativePropertyPath, object obj)
        {
            // property path segments are separated by a '.'
            foreach (var pathPart in relativePropertyPath.Split('.'))
            {
                if (obj == null) { return null; }
                var type = obj.GetType();
                obj = type.GetMemberValue(pathPart, obj);
            }
            return obj;
        }

        /// <summary>
        /// Gets the value of the member from an object using the member name
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName">Name of the member</param>
        /// <param name="obj">Object to get the member value from</param>
        /// <returns></returns>
        private static object GetMemberValue(this IReflect type, string memberName, object obj)
        {
            var memberInfo = type.GetMember(memberName, DEFAULT_BINDING_FLAGS).FirstOrDefault();

            if (memberInfo is PropertyInfo)
            {
                return type.GetProperty(memberName, DEFAULT_BINDING_FLAGS).GetValue(obj);
            }
            if (memberInfo is FieldInfo)
            {
                return type.GetField(memberName, DEFAULT_BINDING_FLAGS).GetValue(obj);
            }
            return null;
        }

        /// <summary>
        /// Gets the relative path name for a member from a lambda expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="lambda"></param>
        /// <returns></returns>
        internal static string GetMemberRelativePathNameFromLambda<TSource, TResult>(Expression<Func<TSource, TResult>> lambda)
        {
            return lambda.Body.ToString()
                .Replace(string.Format("{0}.", lambda.Parameters.First()), string.Empty)
                .Replace(string.Format("{0}.", typeof(TSource).Name), string.Empty); // allows us to get static members
        }

        /// <summary>
        /// Attempts to get a constant member from a type by using its value
        /// </summary>
        /// <typeparam name="T">Type the constant belongs to</typeparam>
        /// <typeparam name="K">Type of the constant</typeparam>
        /// <param name="valueToCompare">Value of the constant</param>
        /// <param name="memberInfo">MemberInfo of the constant (output parameter)</param>
        /// <returns></returns>
        internal static bool TryGetConstantMemberInfoByValue<T, K>(K valueToCompare, out MemberInfo memberInfo) where T : new()
        {
            var matchingConstants = typeof (T).GetFields(DEFAULT_BINDING_FLAGS)
                .Where(x =>
                {
                    if (!x.IsLiteral || x.GetMemberType() != typeof(K)) return false; // constants of type K only
                    var constValue = (K) typeof (T).GetMemberValue(x.Name, new T());
                    return EqualityComparer<K>.Default.Equals(valueToCompare, constValue); // constant value equals value that was passed in
                }).ToList();

            // it is conceivable that more than one constant of type K could have the same value
            memberInfo = matchingConstants.Count == 1 ? matchingConstants.First() : null; 
            return memberInfo != null;
        }

        /// <summary>
        /// Gets all property and field members of type T that match the provided settings
        /// </summary>
        /// <typeparam name="T">Type to get members of</typeparam>
        /// <param name="getAllMemberSettings">Settings for which members to get</param>
        /// <returns></returns>
        internal static IEnumerable<MemberInfo> GetAllMembers<T>(GetAllMemberSettings getAllMemberSettings) where T : new()
        {
            var members = typeof (T).GetMembers(getAllMemberSettings.BindingFlags)
                .Where(x =>
                    DEFAULT_MEMBER_TYPES.Contains(x.MemberType) // only get properties and fields (e.g. no methods, etc.)
                    && (!x.GetCustomAttributes().Any(y => y is CompilerGeneratedAttribute)) // don't get compiler generated members (e.g. backing fields)
                    && (x.GetMemberType().IsValueType || x.GetMemberType() == typeof(string)) // don't get complex types
                    && (getAllMemberSettings.Predicate == null || getAllMemberSettings.Predicate(x)));

            return getAllMemberSettings.Orderer != null
                ? members.OrderBy(x => x, getAllMemberSettings.Orderer)
                : members;
        }
    }
}