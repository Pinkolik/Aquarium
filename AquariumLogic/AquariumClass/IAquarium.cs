using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;

namespace AquariumLogic.AquariumClass
{
    public interface IAquarium
    {
        IEnumerable<KeyValuePair<IFish, Point>> Fishes { get; }

        void AddFish(IFish fish, Point position);
    }
}
