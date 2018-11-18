using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.AquariumClass
{
    public class Aquarium : IAquarium, IDrawable
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

        public Size Size { get; }
        public Bitmap Texture { get; }
    }
}
