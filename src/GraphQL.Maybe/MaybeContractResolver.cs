//
// Copyright (c) cklam2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace GraphQL.Maybe.Serialization
{
    /// <summary>
    /// Custom contract resolver that handles the <see cref="Maybe{T}"/> type during serialization.
    /// </summary>
    public class MaybeContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// MaybeContractResolver Instance
        /// </summary>
        public static readonly MaybeContractResolver Instance = new MaybeContractResolver();

        /// <summary>
        /// Creates a <see cref="JsonProperty"/> for the given <see cref="MemberInfo"/>
        /// </summary>
        /// <param name="member">The member to create a JsonProperty for</param>
        /// <param name="memberSerialization">The member's parent MemberSerialization</param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            return UpdateShouldSerialize(property);
        }

        /// <summary>
        /// Sets the ShouldSerialize when value is of type <see cref="Maybe{T}"/>. If it doesn't have a underlying value
        /// the property won't be serialized.
        /// </summary>
        /// <param name="property">Property being resolved</param>
        /// <returns>Property with ShouldSerialize set when underlying value is of type <see cref="Maybe{T}"/></returns>
        protected virtual JsonProperty UpdateShouldSerialize(JsonProperty property)
        {
            if (Maybe.IsMaybeType(property.PropertyType))
            {
                property.ShouldSerialize = instance =>
                {
                    Type t = instance.GetType();
                    PropertyInfo pi = t.GetProperty(property.PropertyName, BindingFlags.Instance | BindingFlags.Public);
                    if (pi != null)
                    {
                        return Maybe.HasValue(pi.GetValue(instance, null));
                    }

                    FieldInfo fi = t.GetField(property.PropertyName, BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        return Maybe.HasValue(fi.GetValue(instance));
                    }
                    return false;
                };
            }

            return property;
        }
    }
}
