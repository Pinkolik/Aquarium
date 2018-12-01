using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AquariumLogic.PointExtensionClass
{
    [TestFixture]
    public class PointExtension_Should
    {
        private Point point;

        [SetUp]
        public void SetUp()
        {
            point = new Point(0, 0);
        }

        [Test]
        public void CorrectPoint_WhenAddPositiveVector()
        {
            var vector = new Vector2(1,3);

            var actual = point.AddVector(vector);

            Assert.AreEqual(new Point((int) (point.X + vector.X), (int) (point.Y + vector.Y)), actual);
        }

        [Test]
        public void CorrectPoint_WhenAddNegativeVector()
        {
            var vector = new Vector2(-3,-2);

            var actual = point.AddVector(vector);

            Assert.AreEqual(new Point((int) (point.X + vector.X), (int) (point.Y + vector.Y)), actual);
        }

        [Test]
        public void CorrectVector_WhenGetVectorToPointCalled()
        {
            var newPoint = new Point(3, 4);

            var actual = point.GetVectorToPoint(newPoint);

            var expected = new Vector2(newPoint.X-point.X, newPoint.Y-point.Y); 
            Assert.AreEqual(expected, actual);
        }
    }
}
