// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace FuseBox
{
    /// <summary>
    /// Exception thrown when <see cref="FuseBox.Container.Resolve(Type)"/> can't resolve
    /// a type because it's dependency graph contains a cycle.
    /// </summary>
    public class CyclicDependencyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CyclicDependencyException"/> class.
        /// </summary>
        /// <param name="type">The type where the cycle was identified.</param>
        public CyclicDependencyException(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type where the cycle was identified.
        /// </summary>
        public Type Type { get; }
    }
}
