// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;

namespace FuseBox
{
    /// <summary>
    /// Extensions for <see cref="SerializationInfo"/>.
    /// </summary>
    public static class SerializationInfoExtensions
    {
        /// <summary>
        ///     Retrieves a value from the System.Runtime.Serialization.SerializationInfo store.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> store where the object will be retrieved from.
        /// </param>
        /// <param name="name">
        ///     The name associated with the value to retrieve.
        /// </param>
        /// <typeparam name="T">
        ///     The System.Type of the value to retrieve. If the stored value cannot be converted
        ///     to this type, the system will throw a System.InvalidCastException.
        /// </typeparam>
        /// <returns>
        ///     The object of the specified System.Type associated with name.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     name or type is null.
        /// </exception>
        /// <exception cref="System.InvalidCastException">
        ///     The value associated with name cannot be converted to type.
        /// </exception>
        /// <exception cref="SerializationException">
        ///     An element with the specified name is not found in the current instance.
        /// </exception>
        public static T GetValue<T>(this SerializationInfo info, string name)
        {
            return (T)info.GetValue(name, typeof(T));
        }
    }
}
