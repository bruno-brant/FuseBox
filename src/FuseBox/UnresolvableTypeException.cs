using System;
using System.Runtime.Serialization;

namespace FuseBox
{
    /// <summary>
    /// Exception throw when a certain type couldn't be resolved by <see cref="Container"/>.
    /// </summary>
    public class UnresolvableTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvableTypeException"/> class.
        /// </summary>
        /// <param name="type">The type that couldn't be resolved.</param>
        public UnresolvableTypeException(Type type)
            : this(type, "Couldn't resolve type.", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvableTypeException"/> class.
        /// </summary>
        /// <param name="type">The type that couldn't be resolved.</param>
        /// <param name="reason">The reason why the type couldn't be resolved.</param>
        public UnresolvableTypeException(Type type, string reason)
            : this(type, reason, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnresolvableTypeException"/> class.
        /// </summary>
        /// <param name="type">The type that couldn't be resolved.</param>
        /// <param name="reason">The reason why the type couldn't be resolved.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public UnresolvableTypeException(Type type, string reason, Exception innerException)
            : base(reason, innerException)
        {
            Type = type;
        }

        /// <inheritdoc/>
        protected UnresolvableTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the type that couldn't be resolved.
        /// </summary>
        public Type Type { get; }
    }
}
