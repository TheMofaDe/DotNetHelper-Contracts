using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace DotNetHelper_Contracts.CustomException
{


    [Serializable]
    // Important: This attribute is NOT inherited from Exception, and MUST be specified 
    // otherwise serialization will fail with a SerializationException stating that
    // "Type X in Assembly Y is not marked as serializable."
    public class SerializableException : Exception
    {
        public SerializableException()
        {
        }

        public SerializableException(string message)
            : base(message)
        {
        }

        public SerializableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SerializableException(string message, string resourceName, IList<string> validationErrors)
            : base(message)
        {
            this.ResourceName = resourceName;
            this.ValidationErrors = validationErrors;
        }

        public SerializableException(string message, string resourceName, IList<string> validationErrors, Exception innerException)
            : base(message, innerException)
        {
            this.ResourceName = resourceName;
            this.ValidationErrors = validationErrors;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        // Constructor should be protected for unsealed classes, private for sealed classes.
        // (The Serializer invokes this constructor through reflection, so it can be private)
        protected SerializableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.ResourceName = info.GetString("ResourceName");
            this.ValidationErrors = (IList<string>)info.GetValue("ValidationErrors", typeof(IList<string>));
        }

        public string ResourceName { get; }

        public IList<string> ValidationErrors { get; }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("ResourceName", this.ResourceName);
            info.AddValue("ValidationErrors", this.ValidationErrors, typeof(IList<string>));
            base.GetObjectData(info, context);
        }
    }




    
}
