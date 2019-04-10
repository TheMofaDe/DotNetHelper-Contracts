using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

//CREDIT :: https://stackoverflow.com/questions/94488/what-is-the-correct-way-to-make-a-custom-net-exception-serializable
namespace DotNetHelper_Contracts.CustomException
{
    [Serializable]
    public sealed class UserSerializableException : SerializableException
    {
        public string Username { get; }

        public UserSerializableException()
        {
        }

        public UserSerializableException(string message, string username)
            : base(message)
        {
            this.Username = username;
        }

        public UserSerializableException(string message, string username, Exception innerException)
            : base(message, innerException)
        {
            this.Username = username;
        }

        public UserSerializableException(string message, string username, string resourceName, IList<string> validationErrors)
            : base(message, resourceName, validationErrors)
        {
            this.Username = username;
        }

        public UserSerializableException(string message, string username, string resourceName, IList<string> validationErrors, Exception innerException)
            : base(message, resourceName, validationErrors, innerException)
        {
            this.Username = username;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        // Serialization constructor is private, as this class is sealed
        private UserSerializableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Username = info.GetString("Username");
        }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("Username", this.Username);
            base.GetObjectData(info, context);
        }
    }
}
