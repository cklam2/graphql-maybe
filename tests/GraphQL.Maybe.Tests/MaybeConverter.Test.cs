using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GraphQL.Maybe.MaybeConverter_Test
{
    [TestClass]
    public class MaybeConverter_Test
    {
        [TestClass]
        public class Generic
        {
            [TestMethod]
            public void TestCanRead()
            {
                Assert.IsTrue(new MaybeConverter().CanRead);
            }

            [TestMethod]
            public void TestCanConvert()
            {
                MaybeConverter mc = new MaybeConverter();
                Assert.IsFalse(mc.CanConvert(typeof(int)));
                Assert.IsFalse(mc.CanConvert(typeof(bool)));
                Assert.IsFalse(mc.CanConvert(typeof(string)));
                Assert.IsFalse(mc.CanConvert(typeof(int?)));

                Assert.IsTrue(mc.CanConvert(typeof(Maybe<int>)));
                Assert.IsTrue(mc.CanConvert(typeof(Maybe<string>)));
                Assert.IsTrue(mc.CanConvert(typeof(Maybe<int?>)));
            }
        }

        [TestClass]
        public class WriteJson
        {
            [TestMethod]
            public void TestWriteJsonWithValue()
            {
                var maybeConverter = new MaybeConverter();

                Assert.AreEqual("3", JsonConvert.SerializeObject(Maybe.FromValue(3), maybeConverter));
                Assert.AreEqual("true", JsonConvert.SerializeObject(Maybe.FromValue(true), maybeConverter));
                Assert.AreEqual("\"Test\"", JsonConvert.SerializeObject(Maybe.FromValue("Test"), maybeConverter));
            }

            [TestMethod]
            public void TestWriteJsonWithComplexValue()
            {
                var maybeConverter = new MaybeConverter();
                Maybe<List<Maybe<int>>> complex = new List<Maybe<int>>();

                complex.Value.Add(Maybe<int>.None);
                Assert.AreEqual("[]", JsonConvert.SerializeObject(complex, maybeConverter));

                complex.Value.AddRange(new[] { Maybe.FromValue(232), 9 });
                Assert.AreEqual("[232,9]", JsonConvert.SerializeObject(complex, maybeConverter));
            }

            [TestMethod]
            public void TestWriteJsonNoValue()
            {
                var maybeConverter = new MaybeConverter();
                Assert.AreEqual("", JsonConvert.SerializeObject(Maybe<string>.None, maybeConverter));
            }
        }

        [TestClass]
        public class ReadJson
        {
            [TestMethod]
            public void TestReadJsonWithNull()
            {
                var maybeConverter = new MaybeConverter();
                var obj = JsonConvert.DeserializeObject("null", typeof(Maybe<int>), maybeConverter);

                Assert.IsTrue(obj is Maybe<int>);
                Assert.IsFalse(((Maybe<int>)obj).HasValue);
            }

            [TestMethod]
            public void TestReadJsonWithValue()
            {
                var maybeConverter = new MaybeConverter();
                var objStr = JsonConvert.DeserializeObject("\"MaybeHero\"", typeof(Maybe<string>), maybeConverter);

                Assert.IsTrue(objStr is Maybe<string>);
                Assert.IsTrue(((Maybe<string>)objStr).HasValue);
                Assert.AreEqual("MaybeHero", ((Maybe<string>)objStr).Value);

                var objInt = JsonConvert.DeserializeObject("432324328", typeof(Maybe<int>), maybeConverter);

                Assert.IsTrue(objInt is Maybe<int>);
                Assert.IsTrue(((Maybe<int>)objInt).HasValue);
                Assert.AreEqual(432324328, ((Maybe<int>)objInt).Value);
            }

            [TestMethod]
            public void TestReadJsonWithComplexValue()
            {
                var maybeConverter = new MaybeConverter();
                var objBool = JsonConvert.DeserializeObject("[true,false,false,true,null]", typeof(Maybe<List<Maybe<bool>>>), maybeConverter);

                Assert.IsTrue(objBool is Maybe<List<Maybe<bool>>>);

                var mlist = (Maybe<List<Maybe<bool>>>)objBool;
                Assert.IsTrue(mlist.HasValue);
                Assert.AreEqual(5, mlist.Value.Count);
                Assert.IsTrue(mlist.Value.Contains(Maybe<bool>.None));
            }

            [TestMethod]
            public void TestReadJsonWithConversionValue()
            {
                var maybeConverter = new MaybeConverter();
                var objGuid= JsonConvert.DeserializeObject("\"dc95aa3e-c209-4225-becb-3f28eca1ea56\"", typeof(Maybe<Guid>), maybeConverter);

                Assert.IsTrue(objGuid is Maybe<Guid>);

                var mguid = (Maybe<Guid>)objGuid;
                Assert.IsTrue(mguid.HasValue);
            }
        }
    }
}
