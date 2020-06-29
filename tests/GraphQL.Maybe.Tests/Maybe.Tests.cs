using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQL.Maybe.Maybe_Tests
{
    [TestClass]
    public class Maybe_Test
    {
        [TestClass]
        public class Struct
        {
            [TestMethod]
            public void TestNone()
            {
                Maybe<int> mi = Maybe<int>.None;
                Assert.IsFalse(mi.HasValue);
                Assert.ThrowsException<InvalidOperationException>(() => mi.Value);

                Maybe<string> ms = new Maybe<string>();
                Assert.IsFalse(ms.HasValue);
                Assert.ThrowsException<InvalidOperationException>(() => ms.Value);
            }

            [TestMethod]
            public void TestImplicitAssignment()
            {
                Maybe<int> mi = 3;
                Assert.IsTrue(mi.HasValue);
                Assert.AreEqual(3, mi.Value);

                Maybe<bool> mb = new Maybe<bool>(true);
                Assert.IsTrue(mb.HasValue);
                Assert.AreEqual(true, mb.Value);

                Maybe<string> ms = "Test";
                Assert.IsTrue(ms.HasValue);
                Assert.AreEqual("Test", ms.Value);
            }

            [TestMethod]
            public void TestExplicitAssignment()
            {
                Maybe<int> mi = 3;
                int i = (int)mi;
                Assert.AreEqual(3, i);

                Maybe<bool> mb = new Maybe<bool>(true);
                bool b = (bool)mb;
                Assert.AreEqual(true, b);

                Maybe<string> ms = "Test";
                string s = (string)ms;
                Assert.AreEqual("Test", s);

                Maybe<int> mi2 = Maybe<int>.None;
                Assert.ThrowsException<InvalidOperationException>(() => (int)mi2);
            }

            [TestMethod]
            public void TestNullAssignment()
            {
                Maybe<bool?> mb = null;
                Assert.IsTrue(mb.HasValue);
                Assert.AreEqual(null, mb.Value);

                Maybe<string> ms = null;
                Assert.IsTrue(ms.HasValue);
                Assert.AreEqual(null, ms.Value);
            }

            [TestMethod]
            public void TestComplexClass()
            {
                Maybe<Version> mver = new Version(1, 2, 3, 4);
                Version ver = (Version)mver;
                Assert.IsTrue(mver.HasValue);
                Assert.IsTrue(mver.Equals(ver));
                Assert.AreEqual("1.2.3.4", mver.Value.ToString());

                mver = null;
                Assert.IsTrue(mver.HasValue);
                Assert.AreEqual(null, mver.Value);

                mver = Maybe<Version>.None;
                Assert.IsFalse(mver.HasValue);
                Assert.ThrowsException<InvalidOperationException>(() => mver.Value);
            }

            [TestMethod]
            public void TestComplexArray()
            {
                Maybe<List<Maybe<HashSet<Maybe<int>>>>> mlhi = Maybe<List<Maybe<HashSet<Maybe<int>>>>>.None;
                Assert.IsFalse(mlhi.HasValue);

                mlhi = new List<Maybe<HashSet<Maybe<int>>>>();

                Assert.IsTrue(mlhi.HasValue);
                Assert.AreEqual(0, mlhi.Value.Count);

                mlhi.Value.Add(new HashSet<Maybe<int>>());

                Assert.AreEqual(1, mlhi.Value.Count);
                Assert.IsTrue(mlhi.Value[0].HasValue);
                Assert.AreEqual(0, mlhi.Value[0].Value.Count);

                mlhi.Value[0].Value.Add(new Maybe<int>(5));
                mlhi.Value[0].Value.Add(5); // HashSet should ignore this duplicate

                Assert.AreEqual(1, mlhi.Value[0].Value.Count);
                Assert.IsTrue(mlhi.Value[0].Value.Contains(5));

                mlhi.Value[0].Value.Add(Maybe<int>.None);
                Assert.AreEqual(2, mlhi.Value[0].Value.Count);

                mlhi.Value[0].Value.Remove(5);
                Assert.IsFalse(mlhi.Value[0].Value.First().HasValue);
            }

            [TestMethod]
            public void TestEquals()
            {
                Maybe<int> miNone1 = Maybe<int>.None;
                Maybe<int> miNone2 = Maybe<int>.None;

                Maybe<string> miNull1 = null;
                Maybe<string> miNull2 = new Maybe<string>(null);

                Maybe<int> mi1 = 3;
                Maybe<int> mi2 = new Maybe<int>(3);

                Assert.IsTrue(mi2.Equals(mi1));
                Assert.IsTrue(mi2.Equals(3));
                Assert.IsFalse(mi2.Equals(999));

                Assert.IsFalse(miNone1.Equals(mi1));
                Assert.IsTrue(miNone1.Equals(miNone2));

                Assert.IsTrue(miNull1.Equals(null));
                Assert.IsTrue(miNull1.Equals(miNull2));
                Assert.IsFalse(miNull1.Equals(miNone1));
            }

            [TestMethod]
            public void TestHashCode()
            {
                string s = "Test ABC";
                Maybe<string> ms = s;

                Assert.AreEqual(s.GetHashCode(), ms.GetHashCode());
            }

            [TestMethod]
            public void TestToString()
            {
                string s = "Test ABC";
                Maybe<string> ms = s;
                Assert.AreEqual(s.ToString(), ms.ToString());

                int i = 23434;
                Maybe<int> mi = i;
                Assert.AreEqual(i.ToString(), mi.ToString());

                Maybe<Guid> mg;
                mg = Guid.NewGuid();
                Assert.AreEqual(Guid.NewGuid().ToString().Length, mg.ToString().Length);

                Maybe<string> mnull;
                mnull = Maybe<string>.None;
                Assert.AreEqual("", mnull.ToString());
            }
        }

        [TestClass]
        public class Class
        {
            [TestMethod]
            public void TestFromValue()
            {
                Assert.AreEqual(8, Maybe.FromValue(8).Value);
                Assert.AreEqual(false, Maybe.FromValue(false).Value);
                Assert.AreEqual(3.14, Maybe.FromValue(3.14).Value);
            }

            [TestMethod]
            public void TestFromNullableValue()
            {
                Assert.AreEqual(324, Maybe.FromValue(new int?(324)).Value);
                Assert.IsFalse(Maybe.FromValue(new bool?()).HasValue);
                Assert.IsFalse(Maybe.FromValue<int>(null).HasValue);
            }

            [TestMethod]
            public void TestUnderlyingTypeNull()
            {
                Assert.ThrowsException<ArgumentNullException>(() => Maybe.GetUnderlyingType(null));
            }

            [TestMethod]
            public void TestUnderlyingTypeUnknown()
            {
                Assert.IsNull(Maybe.GetUnderlyingType(typeof(Version)));
            }

            [TestMethod]
            public void TestUnderlyingValueType()
            {
                Maybe<int> mi = Maybe<int>.None;
                Assert.AreEqual(typeof(int), Maybe.GetUnderlyingType(mi.GetType()));

                mi = 5;
                Assert.AreEqual(typeof(int), Maybe.GetUnderlyingType(mi.GetType()));
            }

            [TestMethod]
            public void TestUnderlyingRefType()
            {
                Maybe<string> ms = Maybe<string>.None;
                Assert.AreEqual(typeof(string), Maybe.GetUnderlyingType(ms.GetType()));

                Maybe<List<string>> mls = Maybe<List<string>>.None;
                Assert.AreEqual(typeof(List<string>), Maybe.GetUnderlyingType(mls.GetType()));
            }
        }
    }
}