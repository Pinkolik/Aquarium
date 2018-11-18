using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AquariumLogic.FoodClass;
using Moq;
using NUnit.Framework;

namespace AquariumLogic.FishClass
{
    [TestFixture]
    public class Fish_Should
    {
        private int maxHealth;
        private int timeToLiveInSeconds;
        private Fish fish;
        private Mock<IFood> food;
        private int healthValue;

        [SetUp]
        public void SetUp()
        {
            maxHealth = 100;
            timeToLiveInSeconds = 1;
            fish = new Fish(maxHealth, timeToLiveInSeconds, new Size(100, 100), null);

            food = new Mock<IFood>();
            healthValue = 10;
            food.Setup(f => f.HealthValue).Returns(healthValue);
        }


        [Test]
        public void FishHasMaxHealth_AfterCreation()
        {
            Assert.AreEqual(maxHealth, fish.Health);
        }
        
        [Test]
        public void FishDies_AfterTimeToLivePasses()
        {
            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds * 1000);

            Assert.False(fish.IsAlive);
        }

        [Test]
        public void FishIsAlive_AfterCreation()
        {
            Assert.True(fish.IsAlive);
        }

        [Test]
        public void FishHasZeroSpeed_BeforeStartsLiving()
        {
            Assert.AreEqual(new Vector2(0, 0), fish.Velocity);
        }

        [Test]
        public void FishHealthIsLowerThanMaxHealth_AfterConsumingFood()
        {
            fish.ConsumeFood(food.Object);

            Assert.LessOrEqual(fish.Health, fish.MaxHealth);
        }

        [Test]
        public void FishHealthIncreasesCorrectly_AfterConsumingFood()
        {
            fish.StartLiving();
            var healthBefore = fish.Health;
            fish.ConsumeFood(food.Object);
            var healthAfter = fish.Health;

            Assert.AreEqual(healthBefore + healthValue, healthAfter);
        }

        [Test]
        public void FishHealthDoesNotChangeAfterDeath_WhenConsumingFood()
        {
            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds * 1000);
            fish.ConsumeFood(food.Object);

            Assert.AreEqual(0, fish.Health);
        }

        [Test]
        public void FishIsDeadAfterDeath_WhenConsumingFood()
        {
            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds * 1000);
            fish.ConsumeFood(food.Object);

            Assert.IsFalse(fish.IsAlive);
        }

        [Test]
        public void FishHasZeroHealth_WhenDead()
        {
            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds * 1000);

            Assert.AreEqual(0, fish.Health);
        }
    }
}