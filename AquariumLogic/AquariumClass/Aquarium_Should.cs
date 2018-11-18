using System;
using System.Collections.Generic;
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

        [SetUp]
        public void SetUp()
        {
            aquarium = new Aquarium();
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
            aquarium.AddFish(mockFish.Object);
            
            Assert.AreEqual(1, aquarium.Fishes.Count());
        }

        [Test]
        public void FishCountDoesNotChange_AfterAddingSameFish()
        {
            aquarium.AddFish(mockFish.Object);
            aquarium.AddFish(mockFish.Object);

            Assert.AreEqual(1, aquarium.Fishes.Count());
        }
    }
}
