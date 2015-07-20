using System;
using System.Reflection;
using TableTranslator.Exceptions;
using TableTranslator.Helpers;

namespace TableTranslator.Model.ColumnConfigurations
{
    public sealed class MemberColumnConfiguration : ColumnConfigurationBase
    {
        private string _relativePropertyPath;
        private string _columnName;
        public MemberInfo MemberInfo { get; private set; }

        public MemberColumnConfiguration(MemberInfo memberInfo, int ordinal, string columnName, object nullReplacement,
            string relativePropertyPath) : base(ordinal, nullReplacement)
        {
            Initialize(memberInfo, columnName, relativePropertyPath);
        }

        // used for the all properties method, where column names are not specified
        internal MemberColumnConfiguration(MemberInfo memberInfo, int ordinal, object nullReplacement) : base(ordinal, nullReplacement)
        {
            Initialize(memberInfo, null, null);
        }

        private void Initialize(MemberInfo memberInfo, string columnName, string relativePropertyPath)
        {
            this.MemberInfo = memberInfo;
            this._relativePropertyPath = relativePropertyPath;
            this._columnName = columnName;

            ValidateInput();
        }

        public string RelativePropertyPath
        {
            get { return !string.IsNullOrEmpty(this._relativePropertyPath) ? this._relativePropertyPath : this.MemberInfo.Name; }
        }

        public override string ColumnName
        {
            get { return !string.IsNullOrEmpty(this._columnName) ? this._columnName : this.MemberInfo.Name; }
        }

        public override Type OutputType { get { return this.MemberInfo.GetMemberType(); }}

        public override object GetValueFromObject(object obj)
        {
            return ReflectionHelper.GetMemberValue(this.RelativePropertyPath, obj);
        }

        internal override void ValidateInput()
        {
            if (this.MemberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            if (this.NullReplacement != null && base.NullReplacement != DBNull.Value && this.MemberInfo.GetMemberType() != base.NullReplacement.GetType())
            {
                throw new TableTranslatorConfigurationException();
            }

            if (string.IsNullOrEmpty(this.RelativePropertyPath))
            {
                throw new ArgumentNullException("relativePropertyPath");
            }

            if (!this.RelativePropertyPath.Contains(this.MemberInfo.Name))
            {
                throw new TableTranslatorConfigurationException("The full property path does not contain the name of the member info to be retrieved.");
            }
        }
    }
}