using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AquariumLogic.FoodClass
{
    [TestFixture]
    public class Food_Should
    {
        private int healthValue;
        private int weight;
        private Food food;

        [SetUp]
        public void SetUp()
        {
            healthValue = 10;
            weight = 10;
            food = new Food(healthValue, weight);
        }

        [Test]
        public void OnConsumedInvoked_WhenConsumed()
        {
            var wasInvoked = false;
            food.OnConsumed += (sender, args) => wasInvoked = true;

            food.Consume();

            Assert.True(wasInvoked);
        }
    }
}
