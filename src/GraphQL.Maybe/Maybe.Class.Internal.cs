using System;

namespace GraphQL.Maybe
{
    public static partial class Maybe
    {
        /// <summary>
        /// Checks if object is of type <see cref="Maybe{T}"/>
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>true if object is of type <see cref="Maybe{T}"/>, otherwise false</returns>
        internal static bool IsMaybeType(object obj)
        {
            return obj != null && IsMaybeType(obj.GetType());
        }

        /// <summary>
        /// Checks if type is of type <see cref="Maybe{T}"/>
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if type is of type <see cref="Maybe{T}"/>, otherwise false</returns>
        internal static bool IsMaybeType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Maybe<>);
        }

        /// <summary>
        /// Gets a value indicating whether the object has a valid value of its underlying type.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>true if type is of type <see cref="Maybe{T}"/> and has a value, otherwise false</returns>
        internal static bool HasValue(object obj)
        {
            return IsMaybeType(obj) ?
                (bool)obj.GetType().GetProperty(nameof(Maybe<int>.HasValue)).GetValue(obj, null) :
                false;
        }

        /// <summary>
        /// Gets the value if object is of the Maybe<T> type and has been assigned a valid underlying value.
        /// </summary>
        /// <returns>Underlying value if type is of type <see cref="Maybe{T}"/> and has a value, otherwise null</returns>
        internal static object GetValue(object obj)
        {
            return HasValue(obj) ?
                obj.GetType().GetProperty(nameof(Maybe<int>.Value)).GetValue(obj, null) :
                null;
        }
    }
}