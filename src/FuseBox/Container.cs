// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FuseBox
{
    /// <summary>
    /// Fuse is a very simple IoC container that should be used to wire-up
    /// applicationsin a loosely coupled way.
    /// </summary>
    public class Container // TODO: Rename it FuseBox once I settle on a package name. :)
    {
        /// <summary>
        /// Tells if a certain type is currently being resolved.
        /// </summary>
        private readonly Dictionary<Type, bool> _isResolving = new Dictionary<Type, bool>(); // TODO: This isn't thread-safe

        /// <summary>
        /// Maps abstract types to concrete implementations.
        /// </summary>
        private readonly Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

        /// <summary>
        /// Registers a mapping between two types.
        /// </summary>
        /// <param name="requested">The type that is requested of the container.</param>
        /// <param name="resolves">The type that the container should resolve.</param>
        public void Register(Type requested, Type resolves)
        {
            if (requested is null)
            {
                throw new ArgumentNullException(nameof(requested));
            }

            if (resolves is null)
            {
                throw new ArgumentNullException(nameof(resolves));
            }

            if (!requested.IsAssignableFrom(resolves))
            {
                throw new ImproperMappingException(requested, resolves);
            }

            if (_mappings.ContainsKey(requested))
            {
                throw new DuplicatedMappingException(requested, resolves, _mappings[requested]);
            }

            if (resolves.IsInterface || resolves.IsAbstract)
            {
                throw new UninstantiableTypeException(requested, resolves);
            }

            _mappings[requested] = resolves;
        }

        /// <summary>
        /// Tries to registers a mapping between two types.
        /// </summary>
        /// <param name="requested">The type that is requested of the container.</param>
        /// <param name="resolves">The type that the container should resolve.</param>
        /// <returns>
        /// True if there were no previous mappings registered and therefore this
        /// registration succeeded. False otherwise.
        /// </returns>
        public bool TryRegister(Type requested, Type resolves)
        {
            if (IsRegistered(requested))
            {
                return false;
            }

            Register(requested, resolves);

            return true;
        }

        /// <summary>
        /// Removes mappings.
        /// </summary>
        /// <param name="requested">The type for which all mappings will be removed.</param>
        /// <exception cref="TypeNotRegisteredException">
        /// When the requested type has no mappings.
        /// </exception>
        public void Unregister(Type requested)
        {
            if (!_mappings.ContainsKey(requested))
            {
                throw new TypeNotRegisteredException(requested);
            }

            _mappings.Remove(requested);
        }

        /// <summary>
        /// Informs whether a given type has mappings registered within the container.
        /// </summary>
        /// <param name="type">
        ///     The type to check for mappings.
        /// </param>
        /// <returns>
        ///     True if the type has mappings, false otherwise.
        /// </returns>
        public bool IsRegistered(Type type)
        {
            return _mappings.ContainsKey(type);
        }

        /// <summary>
        /// Resolves an instance for the given type.
        /// </summary>
        /// <param name="request">The type to be resolved.</param>
        /// <returns>
        ///     A instance that is assignable to <paramref name="request"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     When <paramref name="request"/> is null.
        /// </exception>
        /// <exception cref="CyclicDependencyException">
        ///     When <paramref name="request"/> constructors have cyclic dependencies,
        ///     that is, the object graph they form has at least on cycle.
        /// </exception>
        /// <exception cref="UnresolvableTypeException">
        ///     When <paramref name="request"/> can't be resolved to a instatiable
        ///     constructor.
        /// </exception>
        /// <exception cref="UnmappedTypeException">
        ///     When <paramref name="request"/> isn't instantiable and there are
        ///     no mappings available.
        /// </exception>
        public object Resolve(Type request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (_mappings.ContainsKey(request))
            {
                return Resolve(_mappings[request]);
            }

            if (request.IsAbstract || request.IsInterface)
            {
                throw new UnmappedTypeException(request);
            }

            if (_isResolving.ContainsKey(request) && _isResolving[request] == true)
            {
                throw new CyclicDependencyException(request);
            }

            return ResolveImpl(request);
        }

        private object ResolveImpl(Type request)
        {
            _isResolving[request] = true;

            var constructors = request.GetConstructors();

            var reasons = new Dictionary<string, Exception>();

            foreach (var constructor in constructors.OrderByDescending(_ => _.GetParameters().Length))
            {
                try
                {
                    var instance = Resolve(constructor);

                    _isResolving[request] = false;

                    return instance;
                }
                catch (Exception e)
                {
                    // TODO: Ignoring this exception works, but has poor performance
                    reasons.Add(constructor.ToString(), e);
                }
            }

            _isResolving[request] = false;

            if (constructors.Length == 1)
            {
                throw reasons.Single().Value;
            }
            else
            {
                throw new NoSuitableConstructorException(request, reasons);
            }
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
