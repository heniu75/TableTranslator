using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TableTranslator.Exceptions
{
    [Serializable]
    public class TableTranslatorException : Exception
    {
        internal TableTranslatorException()
        {
            
        }

        internal TableTranslatorException(string message)
            : base(message)
        {
            
        }

        internal TableTranslatorException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}