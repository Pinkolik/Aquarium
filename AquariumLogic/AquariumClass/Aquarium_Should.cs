﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;
using AquariumLogic.FoodClass;
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
        private Size size;

        [SetUp]
        public void SetUp()
        {
            size = new Size(100, 100);
            aquarium = new Aquarium(size);
            mockFish = new Mock<IFish>();
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
            aquarium.AddFish(mockFish.Object, new Point(0, 0));

            Assert.AreEqual(1, aquarium.Fishes.Count());
        }

        [Test]
        public void FishHasCorrectPos_AfterAddingFish()
        {
            var position = new Point(1, 2);
            aquarium.AddFish(mockFish.Object, position);

            Assert.AreEqual(aquarium.Fishes.First(pair => pair.Key == mockFish.Object).Value, position);
        }

        [Test]
        public void FishCountDoesNotChange_AfterAddingSameFish()
        {
            aquarium.AddFish(mockFish.Object, new Point(0, 0));
            aquarium.AddFish(mockFish.Object, new Point(0, 1));

            Assert.AreEqual(1, aquarium.Fishes.Count());
        }

        [Test]
        public void ThrowsException_WhenAddingFishOutOfBounds()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => aquarium.AddFish(mockFish.Object, new Point(-1, -1)));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                aquarium.AddFish(mockFish.Object, new Point(size.Width, size.Height)));
        }

        [Test]
        public void AquariumContainsFood_AfterAddingFood()
        {
            aquarium.AddFood(mockFood.Object, new Point(0, 0));

            Assert.AreEqual(1, aquarium.Food.Count());
        }

        [Test]
        public void FoodHasCorrectPos_AfterAddingFood()
        {
            var position = new Point(1, 2);
            aquarium.AddFood(mockFood.Object, position);

            Assert.AreEqual(aquarium.Food.First(pair => pair.Key == mockFood.Object).Value, position);
        }

        [Test]
        public void FoodCountDoesNotChange_AfterAddingSameFood()
        {
            aquarium.AddFood(mockFood.Object, new Point(0, 0));
            aquarium.AddFood(mockFood.Object, new Point(0, 1));

            Assert.AreEqual(1, aquarium.Food.Count());
        }

        [Test]
        public void ThrowsException_WhenAddingFoodOutOfBounds()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => aquarium.AddFood(mockFood.Object, new Point(-1, -1)));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                aquarium.AddFood(mockFood.Object, new Point(size.Width, size.Height)));
        }

        [Test]
        public void FishStartLivingInvoked_WhenFishAdded()
        {
            aquarium.AddFish(mockFish.Object, new Point(0,0));
            
            mockFish.Verify(f => f.StartLiving(), Times.Once);
        }

        [Test]
        public void FishSetTargetVectorToFoodInvoked_WhenFishIsHungry()
        {
            var fishPos = new Point(1, 1);
            var foodPos = new Point(4, 4);
            mockFish.Setup(f => f.IsHungry).Returns(true);
            aquarium.AddFish(mockFish.Object, fishPos);
            aquarium.AddFood(mockFood.Object, foodPos);

            aquarium.Iterate();

            var expected = new Vector2(foodPos.X, foodPos.Y) - new Vector2(fishPos.X, fishPos.Y);
            mockFish.Verify(f => f.SetTargetVector(expected), Times.Once);
        }
    }
}