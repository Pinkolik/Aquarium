﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AquariumLogic.FishClass;
using AquariumLogic.FoodClass;
using AquariumLogic.IDrawableInterface;
using AquariumLogic.PointExtensionClass;
using Moq;
using NUnit.Framework;

namespace AquariumLogic.AquariumClass
{
    [TestFixture]
    public class Aquarium_Should
    {
        private Aquarium aquarium;
        private Mock<IFish> mockFish;
        private Mock<IFood> mockFood;
        private Mock<IDrawable> mockFishDrawable;
        private Size aquariumSize;
        private Size fishSize;
        private Mock<IDrawable> mockFoodDrawable;
        private Size foodSize;

        [SetUp]
        public void SetUp()
        {
            aquariumSize = new Size(800, 600);
            fishSize = new Size(60, 20);
            foodSize = new Size(5, 5);
            aquarium = new Aquarium(aquariumSize);
            mockFish = new Mock<IFish>();
            mockFishDrawable = new Mock<IDrawable>();
            mockFishDrawable.Setup(f => f.Size).Returns(fishSize);
            mockFoodDrawable = new Mock<IDrawable>();
            mockFoodDrawable.Setup(f => f.Size).Returns(foodSize);
            mockFish.Setup(f => f.IsAlive).Returns(true);
            mockFood = new Mock<IFood>();
        }

        [Test]
        public void AquariumIsEmpty_AfterCreation()
        {
            Assert.AreEqual(0, aquarium.Fishes.Count());
            Assert.AreEqual(0, aquarium.Food.Count());
        }

        [Test]
        public void AquariumContainsFish_AfterAddingFish()
        {
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);

            Assert.AreEqual(1, aquarium.Fishes.Count());
        }

        [Test]
        public void AquariumIterateCalled_WhenAutoIterateIsTrue()
        {
            var autoIterate = true;
            var iterateIntervalInMs = 100;
            var iterationCount = 5;
            var autoAquarium = new Aquarium(aquariumSize, autoIterate, iterateIntervalInMs);
            Thread.Sleep(iterateIntervalInMs*iterationCount);

            Assert.AreEqual(iterationCount, autoAquarium.IterationCount);
        }

        [Test]
        public void FishHasCorrectPos_AfterAddingFish()
        {
            var position = new Point(1, 2);
            mockFishDrawable.Setup(d => d.Position).Returns(position);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);

            Assert.AreEqual(aquarium.Fishes.First(pair => pair.Key == mockFish.Object).Value.Position, position);
        }

        [Test]
        public void FishCountDoesNotChange_AfterAddingSameFish()
        {
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);

            Assert.AreEqual(1, aquarium.Fishes.Count());
        }

        [Test]
        public void ThrowsException_WhenAddingFishOutOfBounds()
        {
            mockFishDrawable.SetupSequence(d => d.Position)
                .Returns(new Point(-1, -1))
                .Returns(new Point(aquariumSize.Width, aquariumSize.Height));

            Assert.Throws<ArgumentOutOfRangeException>(() => aquarium.AddFish(mockFish.Object, mockFishDrawable.Object));
            Assert.Throws<ArgumentOutOfRangeException>(() => aquarium.AddFish(mockFish.Object, mockFishDrawable.Object));
        }

        [Test]
        public void AquariumContainsFood_AfterAddingFood()
        {
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);

            Assert.AreEqual(1, aquarium.Food.Count());
        }

        [Test]
        public void FoodHasCorrectPos_AfterAddingFood()
        {
            var position = new Point(1, 2);
            mockFoodDrawable.Setup(d => d.Position).Returns(position);
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);

            Assert.AreEqual(aquarium.Food.First(pair => pair.Key == mockFood.Object).Value.Position, position);
        }

        [Test]
        public void FoodCountDoesNotChange_AfterAddingSameFood()
        {
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);

            Assert.AreEqual(1, aquarium.Food.Count());
        }

        [Test]
        public void ThrowsException_WhenAddingFoodOutOfBounds()
        {
            mockFoodDrawable.SetupSequence(d => d.Position)
                .Returns(new Point(-1, -1))
                .Returns(new Point(aquariumSize.Width, aquariumSize.Height));

            Assert.Throws<ArgumentOutOfRangeException>(() => aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object));
            Assert.Throws<ArgumentOutOfRangeException>(() => aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object));
        }

        [Test]
        public void FishStartLivingInvoked_WhenFishAdded()
        {
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);
            
            mockFish.Verify(f => f.StartLiving(), Times.Once);
        }

        [Test]
        public void FishSetTargetVectorToFoodInvoked_WhenFishIsHungry()
        {
            var fishPos = new Point(1, 1);
            var foodPos = new Point(50, 50);
            mockFishDrawable.Setup(f => f.Position).Returns(fishPos);
            mockFoodDrawable.Setup(f => f.Position).Returns(foodPos);
            mockFoodDrawable.Setup(f => f.UpdatePosition(It.IsAny<Point>())).Returns(mockFoodDrawable.Object);
            mockFish.Setup(f => f.IsHungry).Returns(true);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);

            aquarium.Iterate();

            var expected = mockFishDrawable.Object.GetCenterPoint()
                .GetVectorToPoint(mockFoodDrawable.Object.GetCenterPoint());
            mockFish.Verify(f => f.SetTargetVector(expected), Times.Once);
        }

        [Test]
        public void FishSetTargetVectorToClosestFoodInvoked_WhenFishIsHungry()
        {
            var farFood = new Mock<IFood>();
            var fishPos = new Point(1, 1);
            var closeFoodPos = new Point(40, 40);
            var farFoodPos = new Point(50, 50);
            var mockFarFoodDrawable = new Mock<IDrawable>();
            mockFish.Setup(f => f.IsHungry).Returns(true);
            mockFishDrawable.Setup(f => f.Position).Returns(fishPos);
            mockFoodDrawable.Setup(f => f.Position).Returns(closeFoodPos);
            mockFoodDrawable.Setup(f => f.UpdatePosition(It.IsAny<Point>())).Returns(mockFoodDrawable.Object);
            mockFarFoodDrawable.Setup(f => f.Position).Returns(farFoodPos);
            mockFarFoodDrawable.Setup(f => f.Size).Returns(foodSize);
            mockFarFoodDrawable.Setup(f => f.UpdatePosition(It.IsAny<Point>())).Returns(mockFarFoodDrawable.Object);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);
            aquarium.AddFood(farFood.Object, mockFarFoodDrawable.Object);

            aquarium.Iterate();

            var expected = mockFishDrawable.Object.GetCenterPoint()
                .GetVectorToPoint(mockFoodDrawable.Object.GetCenterPoint());
            mockFish.Verify(f => f.SetTargetVector(expected), Times.Once);
        }

        [Test]
        public void FishSetTargetVectorIsNotInvoked_WhenFishIsDead()
        {
            var fishVelocity = new Vector2(2,3);
            var startPos = new Point(0,0);
            mockFish.Setup(f => f.Velocity).Returns(fishVelocity);
            mockFish.Setup(f => f.IsHungry).Returns(true);
            mockFish.Setup(f => f.IsAlive).Returns(false);
            mockFishDrawable.Setup(f => f.Position).Returns(startPos);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);

            aquarium.Iterate();

            mockFish.Verify(f => f.SetTargetVector(It.IsAny<Vector2>()), Times.Never);
        }

        [Test]
        public void FishSetTargetVectorIsNotInvoked_WhenNoFood()
        {
            var fishVelocity = new Vector2(2,3);
            var startPos = new Point(0,0);
            mockFish.Setup(f => f.Velocity).Returns(fishVelocity);
            mockFish.Setup(f => f.IsHungry).Returns(true);
            mockFishDrawable.Setup(f => f.Position).Returns(startPos);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);

            aquarium.Iterate();

            mockFish.Verify(f => f.SetTargetVector(It.IsAny<Vector2>()), Times.Never);
        }

        [Test]
        public void FishConsumesFood_WhenFoodInsideFish()
        {
            var fishPos = new Point(0, 0);
            var foodPos = new Point(10, 10);
            mockFish.Setup(f => f.IsHungry).Returns(true);
            mockFishDrawable.Setup(f => f.Position).Returns(fishPos);
            mockFoodDrawable.Setup(f => f.Position).Returns(foodPos);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);
            aquarium.AddFood(mockFood.Object, mockFishDrawable.Object);

            aquarium.Iterate();

            mockFish.Verify(f => f.ConsumeFood(mockFood.Object), Times.Once);
        }

        [Test]
        public void FishDoesNotConsumeFood_WhenFoodInsideFishAndFishIsNotHungry()
        {
            var fishPos = new Point(0, 0);
            var foodPos = new Point(10, 10);
            mockFishDrawable.Setup(f => f.Position).Returns(fishPos);
            mockFoodDrawable.Setup(f => f.Position).Returns(foodPos);
            mockFoodDrawable.Setup(f => f.UpdatePosition(It.IsAny<Point>())).Returns(mockFoodDrawable.Object);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);

            aquarium.Iterate();

            mockFish.Verify(f => f.ConsumeFood(It.IsAny<IFood>()), Times.Never);
        }

        [Test]
        public void FoodDeleted_AfterFishEatsFood()
        {
            var fishPos = new Point(0, 0);
            var foodPos = new Point(10, 10);
            mockFish.Setup(f => f.IsHungry).Returns(true);
            mockFishDrawable.Setup(f => f.Position).Returns(fishPos);
            mockFoodDrawable.Setup(f => f.Position).Returns(foodPos);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);
            aquarium.AddFood(mockFood.Object, mockFishDrawable.Object);

            aquarium.Iterate();

            Assert.False(aquarium.Food.Select(f => f.Key).Contains(mockFood.Object));
        }

        [Test]
        public void FishPositionChangesCorrectly_WhenIterate()
        {
            var fishVelocity = new Vector2(2,3);
            var startPos = new Point(0,0);
            var expected = startPos.AddVector(fishVelocity);
            var newMockFishDrawable = new Mock<IDrawable>();
            mockFish.Setup(f => f.Velocity).Returns(fishVelocity);
            mockFishDrawable.Setup(f => f.Position).Returns(startPos);
            newMockFishDrawable.Setup(f => f.Position).Returns(expected);
            mockFishDrawable.Setup(f => f.UpdatePosition(expected)).Returns(newMockFishDrawable.Object);
            aquarium.AddFish(mockFish.Object, mockFishDrawable.Object);

            aquarium.Iterate();

            
            Assert.AreEqual(expected, aquarium.Fishes.First(p => p.Key == mockFish.Object).Value.Position);
        }

        [Test]
        public void FoodPositionChangesCorrectly_WhenIterate()
        {
            var foodWeight = 2;
            var startPos = new Point(0,0);
            var newMockFoodDrawable = new Mock<IDrawable>();
            var expected = startPos.AddVector(new Vector2(0, foodWeight));
            mockFood.Setup(f => f.Weight).Returns(foodWeight);
            newMockFoodDrawable.Setup(f => f.Position).Returns(expected);
            mockFoodDrawable.Setup(f => f.UpdatePosition(expected)).Returns(newMockFoodDrawable.Object);
            mockFoodDrawable.Setup(f => f.Position).Returns(startPos);
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);

            aquarium.Iterate();

            
            Assert.AreEqual(expected, aquarium.Food.First(p => p.Key == mockFood.Object).Value.Position);
        }

        [Test]
        public void FoodPositionDoesNotChange_WhenOnBottom()
        {
            var foodWeight = 2;
            var startPos = new Point(0,aquarium.Size.Height-foodSize.Height-1);
            var newMockFoodDrawable = new Mock<IDrawable>();
            var newPos = startPos.AddVector(new Vector2(0, foodWeight));
            mockFood.Setup(f => f.Weight).Returns(foodWeight);
            newMockFoodDrawable.Setup(f => f.Position).Returns(newPos);
            newMockFoodDrawable.Setup(f => f.Size).Returns(foodSize);
            mockFoodDrawable.Setup(f => f.UpdatePosition(newPos)).Returns(newMockFoodDrawable.Object);
            mockFoodDrawable.Setup(f => f.Position).Returns(startPos);
            aquarium.AddFood(mockFood.Object, mockFoodDrawable.Object);

            aquarium.Iterate();

            
            Assert.AreEqual(startPos, aquarium.Food.First(p => p.Key == mockFood.Object).Value.Position);
        }

        [Test]
        [Ignore("Not implemented")]
        public void FishStaysInBounds_WhenIsMovingOutOfBounds()
        {
                
        }

        [Test]
        [Ignore("Not implemented")]
        public void FishVelocityChanged_WhenFishHitsBounds()
        {
            
        }
    }
}