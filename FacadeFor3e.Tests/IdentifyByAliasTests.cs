﻿using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class IdentifyByAliasTests
        {
        [Test]
        public void CanConstructStandardAlias()
            {
            var a = new IdentifyByAlias("Number", "L12-345");
            ClassicAssert.IsInstanceOf<StringAttribute>(a.KeyValue);
            ClassicAssert.AreEqual("L12-345", a.KeyValue.Value);
            var renderer = new TestTransactionServiceRenderer(true);
            renderer.RenderKey(a);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Test KeyValue=\"L12-345\" AliasField=\"Number\" />", s);
            }

        [Test]
        public void CanConstructComplexAlias()
            {
            var a = new IdentifyByAlias("MattIndex", new IntAttribute(123456));
            ClassicAssert.IsInstanceOf<IntAttribute>(a.KeyValue);
            ClassicAssert.AreEqual(123456, a.KeyValue.Value);
            var renderer = new TestTransactionServiceRenderer(true);
            renderer.RenderKey(a);
            var s = renderer.Result;
            ClassicAssert.AreEqual("<Test KeyValue=\"123456\" AliasField=\"MattIndex\" />", s);
            }

        [Test]
        public void InvalidAlias()
            {
            Assert.Throws<ArgumentNullException>(() => new IdentifyByAlias(null, "L12-345"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentifyByAlias(" ", "L12-345"));
            }

        [Test]
        public void InvalidKeyValue()
            {
            StringAttribute a = null;
            Assert.Throws<ArgumentNullException>(() => new IdentifyByAlias("Number", a));
            string s = null;
            Assert.Throws<ArgumentNullException>(() => new IdentifyByAlias("Number", s));
            }

        [Test]
        public void ResetAlias()
            {
            var a = new IdentifyByAlias("Code", "Mark");
            Assert.Throws<ArgumentNullException>(() => a.AliasField = null);
            Assert.Throws<ArgumentOutOfRangeException>(() => a.AliasField = "(invalid)");
            a.AliasField = "ItemId";
            }

        [Test]
        public void ResetKeyValue()
            {
            var a = new IdentifyByAlias("Code", "One");
            Assert.Throws<ArgumentNullException>(() => a.KeyValue = null);
            a.KeyValue = new StringAttribute("Two");
            }
        }
    }
