using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;
using Moq;
using NUnit.Framework;

namespace AquariumLogic.AquariumClass
{
    [TestFixture]
    public class Aquarium_Should
    {
        private Aquarium aquarium;
        private Mock<IFish> mockFish;
        private Size size;

        [SetUp]
        public void SetUp()
        {
            size = new Size(100, 100);
            aquarium = new Aquarium(size);
            mockFish = new Mock<IFish>();
        }

        [Test]
        public void AquariumIsEmpty_AfterCreation()
        {
            Assert.AreEqual(0, aquarium.Fishes.Count());
        }

        [Test]
        public void AquariumContainsFish_AfterAddingFish()
        {
            aquarium.AddFish(mockFish.Object, new Point(0, 0));

            Assert.AreEqual(1, aquarium.Fishes.Count());
        }

        [Test]
        public void FishCountDoesNotChange_AfterAddingSameFish()
        {
            aquarium.AddFish(mockFish.Object, new Point(0, 0));
            aquarium.AddFish(mockFish.Object, new Point(0, 0));

            Assert.AreEqual(1, aquarium.Fishes.Count());
        }

        [Test]
        public void ThrowsException_WhenAddingFishOutOfBounds()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => aquarium.AddFish(mockFish.Object, new Point(-1, -1)));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                aquarium.AddFish(mockFish.Object, new Point(size.Width, size.Height)));
        }
    }
}