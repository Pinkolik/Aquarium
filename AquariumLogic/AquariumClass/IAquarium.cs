using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;
using AquariumLogic.FoodClass;

namespace AquariumLogic.AquariumClass
{
    public interface IAquarium
    {
        IEnumerable<KeyValuePair<IFish, Point>> Fishes { get; }
        IEnumerable<KeyValuePair<IFood, Point>> Food { get; }

        void AddFish(IFish fish, Point position);
        void AddFood(IFood food, Point position);
        void Iterate();
    }
}
