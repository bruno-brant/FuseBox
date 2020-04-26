// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace FuseBox
{
    /// <summary>
    /// A type of <see cref="MappingException"/> thrown when the requested type.
    /// already has a mapping.
    /// </summary>
    [Serializable]
    public class DuplicatedMappingException : MappingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicatedMappingException"/> class.
        /// </summary>
        /// <param name="requested">The mapping's requested param.</param>
        /// <param name="newResolution">The mapping's resolves param.</param>
        /// <param name="previousResolution">The already registered resolving type.</param>
        public DuplicatedMappingException(Type requested, Type newResolution, Type previousResolution)
            : base(requested, newResolution, $"{requested.FullName} already is mapped to {previousResolution.FullName}")
        {
            PreviousResolution = previousResolution;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicatedMappingException"/> class with serialized data.
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
        protected DuplicatedMappingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            // TODO: Review, test
            PreviousResolution = info.GetValue<Type>("PreviousResolution");
        }

        /// <summary>
        /// Gets the previous resolving type registered for <see cref="MappingException.Requested"/>.
        /// </summary>
        public Type PreviousResolution { get; }
    }
}
