// Copyright (c) Rixian. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENSE file in the project root for full license information.

namespace Rixian.Extensions.AspNetCore
{
    using Rixian.Extensions.Errors;

    /// <summary>
    /// An exception that wraps the Rixian.Extension.Errors.ErrorBase object.
    /// </summary>
    [System.Serializable]
    public class ErrorException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorException"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public ErrorException(ErrorBase error)
        {
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorException"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ErrorException(ErrorBase error, string message)
            : base(message)
        {
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorException"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ErrorException(ErrorBase error, string message, System.Exception inner)
            : base(message, inner)
        {
            this.Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorException"/> class.
        /// </summary>
        public ErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ErrorException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorException"/> class.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected ErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        public ErrorBase? Error { get; set; }
    }
}
