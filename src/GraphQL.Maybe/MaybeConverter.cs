//
// Copyright (c) cklam2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace GraphQL.Maybe
{
    /// <summary>
    /// Converts objects of type <see cref="Maybe{T}"/> to and from JSON.
    /// </summary>
    public class MaybeConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the maybe object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The maybe value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (Maybe.HasValue(value))
            {
                serializer.Serialize(writer, Maybe.GetValue(value));
            }
        }

        /// <summary>
        /// Reads the JSON representation of the maybe object.
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the maybe object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return Activator.CreateInstance(objectType);
            }
            else
            {
                Type underlyingType = Maybe.GetUnderlyingType(objectType);
                var ctor = objectType.GetConstructor(new [] { underlyingType });

                object value = JToken.Load(reader).ToObject(underlyingType, serializer);
                return ctor.Invoke(new object[] { value });
            }  
        }


        /// <summary>
        /// Gets a value indicating whether this <see cref="MaybeConverter"/> can read JSON.
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType) => Maybe.IsMaybeType(objectType);
    }
}
