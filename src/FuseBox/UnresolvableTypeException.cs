// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace FuseBox
{
    /// <summary>
    /// Exception throw when a certain type couldn't be resolved by <see cref="Container"/>.
    /// </summary>
    public class UnresolvableTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvableTypeException"/> class.
        /// </summary>
        /// <param name="type">The type that couldn't be resolved.</param>
        public UnresolvableTypeException(Type type)
            : this(type, "Couldn't resolve type.", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvableTypeException"/> class.
        /// </summary>
        /// <param name="type">The type that couldn't be resolved.</param>
        /// <param name="reason">The reason why the type couldn't be resolved.</param>
        public UnresolvableTypeException(Type type, string reason)
            : this(type, reason, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvableTypeException"/> class.
        /// </summary>
        /// <param name="type">The type that couldn't be resolved.</param>
        /// <param name="reason">The reason why the type couldn't be resolved.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public UnresolvableTypeException(Type type, string reason, Exception innerException)
            : base(reason, innerException)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvableTypeException"/> class with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The System.Runtime.Serialization.SerializationInfo that holds the serialized
        ///     object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The System.Runtime.Serialization.StreamingContext that contains contextual information
        ///     about the source or destination.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The info parameter is null.
        /// </exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        ///     The class name is null or System.Exception.HResult is zero (0).
        /// </exception>
        protected UnresolvableTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the type that couldn't be resolved.
        /// </summary>
        public Type Type { get; }
    }
}
