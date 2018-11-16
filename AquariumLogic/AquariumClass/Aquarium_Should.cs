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

        [SetUp]
        public void SetUp()
        {
            aquarium = new Aquarium();
        }

        [Test]
        public void NewAquarium_ShouldBeEmpty()
        {
            Assert.AreEqual(0, aquarium.Fishes.Count());
        }

        [Test]
        public void Aquarium_AddFishWorks()
        {
            var mockFish = new Mock<IFish>();
            
            aquarium.AddFish(mockFish.Object);
            
            Assert.AreEqual(1, aquarium.Fishes.Count());
        }
    }
}
