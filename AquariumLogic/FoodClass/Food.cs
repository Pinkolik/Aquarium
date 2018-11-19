using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquariumLogic.FoodClass
{
    public class Food : IFood
    {
        public double HealthValue { get; }
        public double Weight { get; }
        public event EventHandler OnConsumed;

        public void Consume()
        {
            OnConsumed?.Invoke(this, EventArgs.Empty);
        }

        public Food(double healthValue, double weight)
        {
            HealthValue = healthValue;
            Weight = weight;
        }
    }
}
