using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;
using AquariumLogic.FoodClass;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.AquariumClass
{
    public class Aquarium : IAquarium, IDrawable
    {
        public IEnumerable<KeyValuePair<IFish, Point>> Fishes => fishesDictionary.AsEnumerable();
        public IEnumerable<KeyValuePair<IFood, Point>> Food => foodDictionary.AsEnumerable();
        private readonly Dictionary<IFish, Point> fishesDictionary = new Dictionary<IFish, Point>();
        private readonly Dictionary<IFood, Point> foodDictionary = new Dictionary<IFood, Point>();

        public Aquarium(Size size)
        {
            Size = size;
        }

        public void AddFish(IFish fish, Point position)
        {
            if (IsOutOfBounds(position))
                throw new ArgumentOutOfRangeException();
            if (!fishesDictionary.ContainsKey(fish))
            {
                fishesDictionary.Add(fish, position);
                fish.StartLiving();
            }
        }

        public void AddFood(IFood food, Point position)
        {
            if (IsOutOfBounds(position))
                throw new ArgumentOutOfRangeException();
            if (!foodDictionary.ContainsKey(food))
                foodDictionary.Add(food, position);
        }

        public void Iterate()
        {
            foreach (var pair in fishesDictionary)
            {
                var fish = pair.Key;
                var fishPosition = pair.Value;
                if (!fish.IsHungry) continue;
                var foodPosition = FindClosestFood(pair.Value);
                fish.SetTargetVector(GetPointToPointVector(pair.Value, foodPosition));
            }
        }

        private Point FindClosestFood(Point fishPosition)
        {
            var minDistance = float.MaxValue;
            var result = new Point(0,0);
            foreach (var pair in foodDictionary)
            {
                var distance = GetPointToPointVector(fishPosition, pair.Value).Length();
                if (!(distance < minDistance)) continue;
                minDistance = distance;
                result = pair.Value;
            }

            return result;
        }

        private Vector2 GetPointToPointVector(Point from, Point to)
        {
            return new Vector2(to.X, to.Y) - new Vector2(from.X, from.Y);
        }

        private bool IsOutOfBounds(Point position)
        {
            return position.X < 0 || position.Y < 0 || position.X >= Size.Width || position.Y >= Size.Height;
        }

        public Size Size { get; }
        public Bitmap Texture { get; }
    }
}
