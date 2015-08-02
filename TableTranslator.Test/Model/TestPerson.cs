using System;

namespace TableTranslator.Test.Model
{
    public class TestPerson
    {
        public string PublicProperty { get; set; }
        public int PublicField;
        
        internal string InternalProperty { get; set; }
        internal int InternalField;

        protected string ProtectedProperty { get; set; }
        protected int ProtectedField;

        protected internal string ProtectedInternalProperty { get; set; }
        protected internal int ProtectedInternalField;

        public static string StaticProperty { get; set; }
        public static int StaticField;

        public DateTime? NullableDate { get; set; }

        public const int ConstantField = 999;

        public string Method(int value)
        {
            return "I am a method";
        }
    }
}