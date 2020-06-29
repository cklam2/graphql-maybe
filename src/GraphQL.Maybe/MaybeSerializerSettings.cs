//
// Copyright (c) cklam2. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using GraphQL.Maybe.Serialization;
using Newtonsoft.Json;

namespace GraphQL.Maybe.SerializerSettings
{
    /// <summary>
    /// Specifies the custom <see cref="JsonSerializer"/> settings for <see cref="Maybe{T}"/>
    /// </summary>
    public class MaybeSerializerSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaybeSerializerSettings"/> class.
        /// </summary>
        public MaybeSerializerSettings()
            : base()
        {
            Converters.Add(new MaybeConverter());
            ContractResolver = MaybeContractResolver.Instance;
        }
    }
}
