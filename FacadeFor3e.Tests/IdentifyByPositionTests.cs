using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;
using NUnit.Framework.Legacy;

// ReSharper disable ObjectCreationAsStatement

namespace FacadeFor3e.Tests
    {
    [TestFixture]
    public class IdentifyByPositionTests
        {
        [Test]
        public void CanConstruct()
            {
            var pk = new IdentifyByPosition(99_000);
            ClassicAssert.AreEqual(99_000, pk.Position);
            var s = CommonLibrary.GetRenderedOutputWithNode(pk.RenderKey);
            ClassicAssert.AreEqual("<Test Position=\"99000\" />", s);
            }

        [Test]
        public void InvalidPositionThrowsError()
            {
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentifyByPosition(-1));
            }
        }
    }
