// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace FuseBox
{
    /// <summary>
    /// Thrown when the type requested is abstract and has no mappings.
    /// </summary>
    [Serializable]
    public class UnmappedTypeException : UnresolvableTypeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnmappedTypeException"/> class.
        /// </summary>
        /// <param name="requested">The type that couldn't be resolved.</param>
        public UnmappedTypeException(Type requested)
            : base(requested, $"Type '{requested.FullName}' has no mappings and can't be instantiated.")
        {
        }
    }
}
