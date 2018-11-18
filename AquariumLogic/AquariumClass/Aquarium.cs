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
        public IEnumerable<IFish> Fishes => fishes.AsEnumerable();
        private readonly HashSet<IFish> fishes = new HashSet<IFish>();

        public Aquarium()
        {
        }

        public void AddFish(IFish fish)
        {
            fishes.Add(fish);
        }
    }
}
