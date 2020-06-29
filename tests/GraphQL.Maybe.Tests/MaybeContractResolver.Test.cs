using Newtonsoft.Json.Serialization;
using GraphQL.Maybe.Tests.Helpers;
using GraphQL.Maybe.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphQL.Maybe.MaybeContractResolver_Test
{
    [TestClass]
    public class MaybeContractResolver_Test
    {
        [TestMethod]
        public void TestInstance()
        {
            Assert.IsTrue(MaybeContractResolver.Instance is MaybeContractResolver);
        }

        [TestMethod]
        public void TestUpdateShouldSerializeNull()
        {
            MaybeContractResolverWrapper testWrapper = new MaybeContractResolverWrapper();
            JsonProperty property = new JsonProperty
            {
                PropertyType = typeof(int),
                PropertyName = "Id"
            };
            
            Assert.IsNotNull(testWrapper.TestUpdateShouldSerialize(property));
            Assert.IsNull(property.ShouldSerialize, "Should not set Serialize when type is not Maybe");
        }

        [TestMethod]
        public void TestUpdateShouldSerializeMaybe()
        {
            MaybeContractResolverWrapper testWrapper = new MaybeContractResolverWrapper();
            JsonProperty property = new JsonProperty
            {
                PropertyType = typeof(Maybe<int>),
                PropertyName = "Id"
            };

            Assert.IsNotNull(testWrapper.TestUpdateShouldSerialize(property));
            Assert.IsTrue(property.ShouldSerialize(new { Id = new Maybe<int>(5) }));
            Assert.IsFalse(property.ShouldSerialize(new { Id = Maybe<int>.None }));
        }
    }
}
