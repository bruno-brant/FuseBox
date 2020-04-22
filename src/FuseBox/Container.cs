using System;
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

            var constructors = type.GetConstructors();

            foreach (var constructor in constructors.OrderByDescending(_ => _.GetParameters().Length))
            {
                try
                {
                    return Resolve(constructor);
                }
                catch (UnresolvableTypeException)
                {
                    // TODO: Ignoring this exception works, but also multiple stackunwinds to get the correct ctor.
                }
            }

            throw new UnresolvableTypeException(type, "No suitable constructor found. Inspect inner exception for details.");
        }

        private object Resolve(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            if (parameters.Count() == 0)
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
