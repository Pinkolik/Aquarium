using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;

namespace AquariumLogic.AquariumClass
{
    public interface IAquarium
    {
        IEnumerable<IFish> Fishes { get; }

        void AddFish(IFish fish);
    }
}
