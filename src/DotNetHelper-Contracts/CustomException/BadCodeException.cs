﻿using System;
using System.Runtime.Serialization;

namespace DotNetHelper_Contracts.CustomException
{
    /// <inheritdoc />
    /// <summary>
    /// Custom Exception that lets the user know a code change is required to fix this error
    /// </summary>
   [Serializable()]
    public class BadCodeException : System.Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Just create the exception
        /// </summary>
        public BadCodeException() : base()
        {
            
        }

        /// <inheritdoc />
        /// <summary>
        /// Create the exception with description
        /// </summary>
        /// <param name="message">Exception description</param>
        public BadCodeException(string message) : base(message)
        {

        }

        /// <inheritdoc />
        /// <summary>
        /// Create the exception with description and inner cause
        /// </summary>
        /// <param name="message">Exception description</param>
        /// <param name="innerException">Exception inner cause</param>
        public BadCodeException(string message, System.Exception innerException) : base(message, innerException)
        {

        }

        /// <inheritdoc />
        /// <summary>
        /// Create the exception from serialized data.
        /// Usual scenario is when exception is occured somewhere on the remote workstation
        /// and we have to re-create/re-throw the exception on the local machine
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Serialization context</param>
        protected BadCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
