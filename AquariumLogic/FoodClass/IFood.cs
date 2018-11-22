using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.FoodClass
{
    public interface IFood
    {
        double HealthValue { get; }
        double Weight { get;  }
        event EventHandler OnConsumed;

        void Consume();
    }
}
