// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace FuseBox
{
    /// <summary>
    /// Thrown when the user tries to unregister mappings for a type that has no
    /// mappings.
    /// </summary>
    [Serializable]
    public class TypeNotRegisteredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNotRegisteredException"/> class.
        /// </summary>
        /// <param name="requested">
        /// The type being unregistered.
        /// </param>
        public TypeNotRegisteredException(Type requested)
            : base($"Type '{requested.FullName}' has no mappings")
        {
            Requested = requested;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNotRegisteredException"/> class with serialized data.
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
        protected TypeNotRegisteredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            // TODO: Review, test
            Requested = info.GetValue<Type>(nameof(Requested));
        }

        /// <summary>
        /// Gets the type being unregistered.
        /// </summary>
        public Type Requested { get; }
    }
}
