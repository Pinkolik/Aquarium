using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AquariumLogic.IDrawableInterface;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AquariumLogic.SizeExtensionClass
{
    [TestFixture]
    public class SizeExtension_Should
    {
        private Size size;
        private Mock<IDrawable> drawable;

        [SetUp]
        public void SetUp()
        {
            size = new Size(10, 10);
            drawable = new Mock<IDrawable>();
        }

        [Test]
        public void ReturnTrue_WhenPointOutOfBounds()
        {
            var point = new Point(11, 9);

            var actual = size.IsOutOfBounds(point);

            Assert.True(actual);
        }

        [Test]
        public void ReturnFalse_WhenPointInsideOfBounds()
        {
            var point = new Point(3, 9);

            var actual = size.IsOutOfBounds(point);

            Assert.False(actual);
        }

        [Test]
        public void ReturnTrue_WhenDrawableOutOfBounds()
        {
            var drawableSize = new Size(4, 3);
            var drawablePos = new Point(10, 10);
            drawable.Setup(d => d.Size).Returns(drawableSize);
            drawable.Setup(d => d.Position).Returns(drawablePos);

            var actual = size.IsOutOfBounds(drawable.Object);

            Assert.True(actual);
        }

        [Test]
        public void ReturnTrue_WhenDrawableLargerThanContainer()
        {
            var drawableSize = new Size(20, 20);
            var drawablePos = new Point(0, 0);
            drawable.Setup(d => d.Size).Returns(drawableSize);
            drawable.Setup(d => d.Position).Returns(drawablePos);

            var actual = size.IsOutOfBounds(drawable.Object);

            Assert.True(actual);
        }

        [Test]
        public void ReturnFalse_WhenDrawableInsideOfBounds()
        {
            var drawableSize = new Size(4, 3);
            var drawablePos = new Point(0, 0);
            drawable.Setup(d => d.Size).Returns(drawableSize);
            drawable.Setup(d => d.Position).Returns(drawablePos);

            var actual = size.IsOutOfBounds(drawable.Object);

            Assert.False(actual);
        }
    }
}
