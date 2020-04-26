// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace FuseBox
{
    /// <summary>
    /// Thrown when no suitable constructor is found in the requested type.
    /// </summary>
    public class NoSuitableConstructorException : UnresolvableTypeException
    {
        /// <summary>
        /// Message when there isn't a suitable constructor.
        /// </summary>
        private const string NoSuitableConstructor = "No suitable constructor found. Inspect Reasons property for details.";

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSuitableConstructorException"/> class.
        /// </summary>
        /// <param name="requested">The type that was requested to the container.</param>
        /// <param name="reasons">
        ///     Why each constructor of <paramref name="requested"/> was rejected.
        /// </param>
        public NoSuitableConstructorException(Type requested, IDictionary<string, Exception> reasons)
            : base(requested, NoSuitableConstructor)
        {
            Reasons = reasons;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoSuitableConstructorException"/> class with serialized data.
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
        protected NoSuitableConstructorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // TODO: Review, test
            Reasons = info.GetValue<IDictionary<string, Exception>>(nameof(Reasons));
        }

        /// <summary>
        /// Gets why each constructor of <see cref="UnresolvableTypeException.Requested"/>
        /// was rejected.
        /// </summary>
        /// <remarks>
        /// The key is a description of the constructor and the value is the
        /// <see cref="Exception"/> that was thrown when trying to resolve it.
        /// </remarks>
        public IDictionary<string, Exception> Reasons { get; }
    }
}
