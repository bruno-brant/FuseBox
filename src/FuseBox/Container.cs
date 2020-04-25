// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FuseBox
{
    /// <summary>
    /// Fuse is a very simple IoC container that should be used to wire-up applications
    /// in a loosely coupled way.
    /// </summary>
    public class Container // TODO: Rename it FuseBox once I settle on a package name. :)
    {
        /// <summary>
        /// Message when there isn't a suitable constructor.
        /// </summary>
        private const string NoSuitableConstructor = "No suitable constructor found. Inspect inner exception for details.";

        /// <summary>
        /// Tells if a certain type is currently being resolved.
        /// </summary>
        private readonly Dictionary<Type, bool> _isResolving = new Dictionary<Type, bool>(); // TODO: This isn't thread-safe

        /// <summary>
        /// Resolves an instance for the given type.
        /// </summary>
        /// <param name="type">The type to be resolved.</param>
        /// <returns>A instance that is assignable to <paramref name="type"/>.</returns>
        public object Resolve(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (_isResolving.ContainsKey(type) && _isResolving[type] == true)
            {
                throw new CyclicDependencyException(type);
            }

            _isResolving[type] = true;

            var constructors = type.GetConstructors();

            Exception lastException = null;

            foreach (var constructor in constructors.OrderByDescending(_ => _.GetParameters().Length))
            {
                try
                {
                    var instance = Resolve(constructor);

                    _isResolving[type] = false;

                    return instance;
                }
                catch (UnresolvableTypeException e)
                {
                    // TODO: Ignoring this exception works, has poor performance
                    lastException = e;
                }
                catch (CyclicDependencyException e)
                {
                    // TODO: Ignoring this exception works, has poor performance
                    lastException = e;
                }
            }

            _isResolving[type] = false;

            // specific cause, why that constructor isn't suitable
            if (constructors.Length == 1)
            {
                throw lastException;
            }

            // Generic - there are multiple constructors, and none of them are suitable
            throw new UnresolvableTypeException(type, NoSuitableConstructor, lastException);
        }

        private object Resolve(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            if (parameters.Length == 0)
            {
                return constructor.Invoke(null);
            }
            else
            {
                var arguments = constructor
                    .GetParameters()
                    .Select(p => Resolve(p.ParameterType))
                    .ToArray();

                return constructor.Invoke(arguments);
            }
        }
    }
}
