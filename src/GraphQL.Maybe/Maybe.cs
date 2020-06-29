//
// Copyright (c) cklam2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using System;

namespace GraphQL.Maybe
{
    /// <summary>
    /// Represents an object that may or may not hold a value.
    /// </summary>
    /// <remarks>
    /// Although it may look similar to <see cref="Nullable{T}"/> it is not the same. 
    /// <see cref="Maybe{T}"/> is not limited to value types but can be used for any type.
    /// </remarks>
    /// <typeparam name="T">The underlying value type of the <see cref="Maybe{T}"/> generic type.</typeparam>
    public struct Maybe<T>
    {
        /// <summary>
        /// Represents a Maybe without a value. This field is read-only.
        /// </summary>
        public static Maybe<T> None = new Maybe<T>();

        /// <summary>
        /// Holds the internal value. Value is undetermined when HasValue = false
        /// </summary>
        private readonly T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}"/> structure to the specified value.
        /// </summary>
        /// <param name="value">Value to set</param>
        public Maybe(T value)
        {
            this.value = value;
            HasValue = true;
        }

        /// <summary>
        /// Gets the value of the current Maybe<T> object if it has been assigned a valid underlying value.
        /// </summary>
        /// <remarks>Maybe value can have null value in case of reference or nullable types.</remarks>
        public T Value => HasValue ? value : throw new InvalidOperationException("No Value");

        /// <summary>
        /// Gets a value indicating whether the current <see cref="Maybe{T}"/> object has a valid value of its underlying type.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Maybe{T}"/> instance to its underlying value.
        /// </summary>
        /// <param name="val">A maybe value</param>
        public static explicit operator T(Maybe<T> val) => val.Value;

        /// <summary>
        /// Creates a new <see cref="Maybe{T}"/> object initialized to a specified value.
        /// </summary>
        /// <param name="val">Value to set</param>
        public static implicit operator Maybe<T>(T val) => new Maybe<T>(val);

        /// <summary>
        /// Indicates whether the current <see cref="Maybe{T}"/> object is equal to a specified object.
        /// </summary>
        /// <param name="other">An object</param>
        /// <returns>true if the other parameter is equal to the current <see cref="Maybe{T}"/> object; otherwise, false.</returns>
        public override bool Equals(object other)
        {
            var otherIsMaybe = Maybe.IsMaybeType(other);
            if (otherIsMaybe && (!HasValue || !Maybe.HasValue(other)))
            {
                return !(HasValue || Maybe.HasValue(other));
            }

            if (value == null)
            {
                return (otherIsMaybe ? Maybe.GetValue(other) : other) == null;
            }

            var testVal = otherIsMaybe ? Maybe.GetValue(other) : other;
            return value.Equals(testVal);
        }

        /// <summary>
        /// Retrieves the hash code of the object returned by the Value property.
        /// </summary>
        /// <returns>The hash code of the object returned by the Value property if the HasValue property is true, or zero if the HasValue property is false or Value is null.</returns>
        public override int GetHashCode() => HasValue && value != null ? value.GetHashCode() : 0;

        /// <summary>
        /// Returns the text representation of the value of the current <see cref="Maybe{T}"/> object.
        /// </summary>
        /// <returns>The text representation of the value of the current <see cref="Maybe{T}"/> object if the HasValue property is true, or an empty string ("") if the HasValue property is false or Value is null.</returns>
        public override string ToString() => HasValue && value != null ? value.ToString() : "";
    }
}
