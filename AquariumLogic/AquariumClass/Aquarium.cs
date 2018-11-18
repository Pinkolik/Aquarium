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
        public IEnumerable<KeyValuePair<IFish, Point>> Fishes => fishes.AsEnumerable();
        private readonly Dictionary<IFish, Point> fishes = new Dictionary<IFish, Point>();

        public Aquarium(Size size)
        {
            Size = size;
        }

        public void AddFish(IFish fish, Point position)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= Size.Width || position.Y >= Size.Height)
                throw new ArgumentOutOfRangeException();
            if (!fishes.ContainsKey(fish))
                fishes.Add(fish, position);
        }

        public Size Size { get; }
        public Bitmap Texture { get; }
    }
}
