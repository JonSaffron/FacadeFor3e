using System;
using FacadeFor3e.ProcessCommandBuilder;
using NUnit.Framework;

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
            Assert.AreEqual(99_000, pk.Position);
            var s = CommonLibrary.GetRenderedOutputWithNode(pk.RenderKey);
            Assert.AreEqual("<Test Position=\"99000\" />", s);
            }

        [Test]
        public void InvalidPositionThrowsError()
            {
            Assert.Throws<ArgumentOutOfRangeException>(() => new IdentifyByPosition(-1));
            }
        }
    }
