using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FoodClass;
using System.Numerics;

namespace AquariumLogic.FishClass
{
    public interface IFish
    {
        double Health { get; }
        double MaxHealth { get; }
        Vector2 Velocity { get;  }

        void StartLiving();
        void ConsumeFood(IFood food);
    }
}
