// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace FuseBox
{
    /// <summary>
    /// Exception thrown when the user tried to register a mapping between types
    /// that can't be assigned to each other.
    /// </summary>
    public class ImproperMappingException : MappingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImproperMappingException"/> class.
        /// </summary>
        /// <param name="requested">The mapping's requested param.</param>
        /// <param name="resolves">The mapping's resolves param.</param>
        public ImproperMappingException(Type requested, Type resolves)
            : base(requested, resolves, $"Type {requested.FullName} isn't assignable to {resolves.FullName}")
        {
        }
    }
}
