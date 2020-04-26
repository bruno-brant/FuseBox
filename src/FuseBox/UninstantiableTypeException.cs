// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace FuseBox
{
    /// <summary>
    /// Thrown when the user tries to register a mapping where the resolving type
    /// isn't instantiable (for instance, an interface or an abstract class).
    /// </summary>
    [Serializable]
    public class UninstantiableTypeException : MappingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UninstantiableTypeException"/> class.
        /// </summary>
        /// <param name="requested">The mapping's requested param.</param>
        /// <param name="resolves">The mapping's resolves param.</param>
        public UninstantiableTypeException(Type requested, Type resolves)
            : base(requested, resolves, $"Type {resolves.FullName} isn't instantiable")
        {
        }
    }
}
