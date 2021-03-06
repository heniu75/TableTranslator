﻿using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using TableTranslator.Model;

namespace TableTranslator.Exceptions
{
    /// <summary>
    /// Exception pertaining to the configuration of the table translator
    /// </summary>
    [Serializable]
    public class TableTranslatorConfigurationException : Exception
    {
        /// <summary>
        /// Profile involved in the exception
        /// </summary>
        public TranslationProfile TranslationProfile { get; internal set; }

        internal TableTranslatorConfigurationException()
        {
            
        }

        internal TableTranslatorConfigurationException(string message) 
            : base(message)
        {
            
        }

        internal TableTranslatorConfigurationException(string message, Exception innerException) 
            : base(message, innerException)
        {
            
        }

        internal TableTranslatorConfigurationException(string message, TranslationProfile profile)
            : base(message)
        {
            this.TranslationProfile = profile;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("TranslationProfile", this.TranslationProfile);
        }
    }
}
