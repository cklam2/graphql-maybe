using GraphQL.Maybe.Serialization;
using GraphQL.Maybe.SerializerSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GraphQL.Maybe.MaybeSerializerSettings_Tests
{
    [TestClass]
    public class MaybeSerializerSettings_Test
    {
        [TestMethod]
        public void TestMaybeSerializerSettings()
        {
            MaybeSerializerSettings mss = new MaybeSerializerSettings();

            Assert.IsTrue(mss.Converters.OfType<MaybeConverter>().Any());
            Assert.AreEqual(MaybeContractResolver.Instance, mss.ContractResolver);
        }
    }
}
