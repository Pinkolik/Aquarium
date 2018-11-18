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
        [Test]
        public void FishHasMaxHealth_AfterCreation()
        {
            var maxHealth = 100;
            var fish = new Fish(maxHealth, 100);

            Assert.AreEqual(maxHealth, fish.Health);
        }

        [Test]
        public void FishHasZeroSpeed_BeforeStartsLiving()
        {
            var maxHealth = 100;
            var fish = new Fish(maxHealth, 100);

            Assert.AreEqual(new Vector2(0, 0), fish.Velocity);
        }

        [Test]
        public void FishDies_AfterTimeToLivePasses()
        {
            var maxHealth = 100;
            var timeToLiveInSeconds = 1;
            var fish = new Fish(maxHealth, timeToLiveInSeconds);
            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds*1000);

            Assert.AreEqual(0, fish.Health);
        }
    }
}
