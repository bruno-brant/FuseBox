// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace FuseBox
{
    /// <summary>
    /// Exception thrown when the user tried to register a mapping between types
    /// that violates resolution rules.
    /// </summary>
    [Serializable]
    public class MappingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class.
        /// </summary>
        /// <param name="requested">The mapping's requested param.</param>
        /// <param name="resolves">The mapping's resolves param.</param>
        public MappingException(Type requested, Type resolves)
            : this(requested, resolves, $"Type {requested.FullName} can't be mapped to {resolves.FullName}", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class.
        /// </summary>
        /// <param name="requested">The mapping's requested param.</param>
        /// <param name="resolves">The mapping's resolves param.</param>
        /// <param name="message">The message that describes the error.</param>
        public MappingException(Type requested, Type resolves, string message)
            : this(requested, resolves, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class.
        /// </summary>
        /// <param name="requested">The mapping's requested param.</param>
        /// <param name="resolves">The mapping's resolves param.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">
        ///     The exception that is the cause of the current exception, or a null reference
        ///     (Nothing in Visual Basic) if no inner exception is specified.
        /// </param>
        public MappingException(Type requested, Type resolves, string message, Exception inner)
            : base(message, inner)
        {
            Requested = requested;
            Resolves = resolves;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingException"/> class with serialized data.
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
        protected MappingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            // TODO: Review, test
            Requested = info.GetValue<Type>("Requested");
            Resolves = info.GetValue<Type>("Resolves");
        }

        /// <summary>
        /// Gets the mapping's requested param.
        /// </summary>
        public Type Requested { get; }

        /// <summary>
        /// Gets the mapping's resolved param.
        /// </summary>
        public Type Resolves { get; }
    }
}
