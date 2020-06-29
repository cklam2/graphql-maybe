using Newtonsoft.Json.Serialization;
using GraphQL.Maybe.Serialization;

namespace GraphQL.Maybe.Tests.Helpers
{
    class MaybeContractResolverWrapper : MaybeContractResolver
    {
        public JsonProperty TestUpdateShouldSerialize(JsonProperty property) => 
            base.UpdateShouldSerialize(property);
    }
}
