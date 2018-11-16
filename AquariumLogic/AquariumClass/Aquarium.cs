using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;

namespace AquariumLogic.AquariumClass
{
    public class Aquarium : IAquarium
    {
        public IEnumerable<IFish> Fishes => fishes.AsReadOnly();
        private readonly List<IFish> fishes;

        public Aquarium()
        {
            fishes = new List<IFish>();
        }

        public void AddFish(IFish fish)
        {
            fishes.Add(fish);
        }
    }
}
