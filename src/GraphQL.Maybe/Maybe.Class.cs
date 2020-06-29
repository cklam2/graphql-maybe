//
// Copyright (c) cklam2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using System;

namespace GraphQL.Maybe
{
    /// <summary>
    /// Supports a maybe type that can be assigned None. This class cannot be inherited.
    /// </summary>
    public static partial class Maybe
    {
        /// <summary>
        /// Returns a <see cref="Maybe{T}"/> that holds the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">An object to be encapsulated</param>
        /// <returns>A <see cref="Maybe{T}"/> with the specified value.</returns>
        public static Maybe<T> FromValue<T>(T value) => 
            new Maybe<T>(value);

        /// <summary>
        /// Returns a <see cref="Maybe{T}"/> that holds the nullable's internal value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">An object to be encapsulated</param>
        /// <returns>A <see cref="Maybe{T}"/> with the specified value or <see cref="Maybe{T}.None"/> if nullable is null.</returns>
        public static Maybe<T> FromValue<T>(T? value) where T: struct => 
            value.HasValue ? new Maybe<T>(value.Value) : Maybe<T>.None;

        /// <summary>
        /// Returns the underlying type argument of the specified maybe type.
        /// </summary>
        /// <param name="maybeType">A Type object that describes a closed generic maybe type.</param>
        /// <returns>The type argument of the maybeType parameter, if the maybeType parameter is a closed generic maybe type; otherwise, null.</returns>
        public static Type GetUnderlyingType(Type maybeType)
        {
            if (IsMaybeType(maybeType))
            {
                // Instantiated generic type only                
                Type genericType = maybeType.GetGenericTypeDefinition();
                if (ReferenceEquals(genericType, typeof(Maybe<>)))
                {
                    return maybeType.GetGenericArguments()[0];
                }
            }
            return null;
        }
    }
}
