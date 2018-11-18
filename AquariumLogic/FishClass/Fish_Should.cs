using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AquariumLogic.FishClass
{
    [TestFixture]
    public class Fish_Should
    {
        private int maxHealth;
        private int timeToLiveInSeconds;
        private Fish fish;

        [SetUp]
        public void SetUp()
        {
            maxHealth = 100;
            timeToLiveInSeconds = 1;
            fish = new Fish(maxHealth, timeToLiveInSeconds);
        }


        [Test]
        public void FishHasMaxHealth_AfterCreation()
        {
            Assert.AreEqual(maxHealth, fish.Health);
        }

        [Test]
        public void FishHasZeroSpeed_BeforeStartsLiving()
        {
            Assert.AreEqual(new Vector2(0, 0), fish.Velocity);
        }

        [Test]
        public void FishDies_AfterTimeToLivePasses()
        {
            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds*1000);

            Assert.AreEqual(0, fish.Health);
        }
    }
}
