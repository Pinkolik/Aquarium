using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AquariumLogic.FoodClass;
using AquariumLogic.IDrawableInterface;
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
        private int maxVelocity;
        private int minVelocity;

        [SetUp]
        public void SetUp()
        {
            maxHealth = 100;
            timeToLiveInSeconds = 1;
            minVelocity = 10;
            maxVelocity = 100;
            fish = new Fish(maxHealth,
                timeToLiveInSeconds,
                minVelocity,
                maxVelocity);

            food = new Mock<IFood>();
            healthValue = 50;
            food.Setup(f => f.HealthValue).Returns(healthValue);
        }

        [Test]
        public void FishVelocityIsZero_BeforeChangeVelocity()
        {
            Assert.AreEqual(new Vector2(0,0), fish.Velocity);
        }

        [Test]
        [Repeat(20)]
        public void FishVelocityIsInRange_AfterChangeVelocity()
        {
            fish.ChangeVelocity();

            Assert.GreaterOrEqual(fish.Velocity.Length(), minVelocity);
            Assert.LessOrEqual(fish.Velocity.Length(), maxVelocity);
        }

        [Test]
        public void FishVelocityDirectionIsCorrect_WhenSetTargetVector()
        {
            float targetX = 100;
            float targetY = 200;
            var targetVector = new Vector2(targetX, targetY);

            fish.SetTargetVector(targetVector);

            var expected = targetX / targetY;
            var actual = fish.Velocity.X / fish.Velocity.Y;
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FishVelocityLengthIsMax_WhenSetTargetVector()
        {
            float targetX = 100;
            float targetY = 200;
            var targetVector = new Vector2(targetX, targetY);

            fish.SetTargetVector(targetVector);

            Assert.AreEqual(maxVelocity, fish.Velocity.Length());
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
            healthValue = 10;

            fish.StartLiving();
            var healthBefore = fish.Health;
            fish.ConsumeFood(food.Object);
            var healthAfter = fish.Health;

            Assert.AreEqual(Math.Min(healthBefore + healthValue, maxHealth), healthAfter);
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
        
        [Test]
        public void FoodConsumeInvoked_WhenFishConsumesFood()
        {
            fish.ConsumeFood(food.Object);
            
            food.Verify(f => f.Consume(), Times.Once);
        }

        [Test]
        public void OnHungryInvoked_WhenHealthLessThanHalf()
        {
            var wasInvoked = false;
            fish.OnHungry += (sender, args) => wasInvoked = true;

            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds*1000/2);

            Assert.True(wasInvoked);
        }

        [Test]
        public void FishIsHungry_WhenHealthLessThanHalf()
        {
            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds*1000/2);

            Assert.True(fish.IsHungry);
        }

        [Test]
        public void FishIsNotHungry_AfterCreation()
        {
            Assert.False(fish.IsHungry);
        }

        [Test]
        public void FishIsNotHungry_AfterConsumingFood()
        {
            fish.StartLiving();
            Thread.Sleep(timeToLiveInSeconds * 1000 / 2);
            fish.ConsumeFood(food.Object);

            Assert.False(fish.IsHungry);
        }
    }
}