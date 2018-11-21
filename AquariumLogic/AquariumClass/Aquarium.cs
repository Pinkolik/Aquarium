using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AquariumLogic.FishClass;
using AquariumLogic.FoodClass;
using AquariumLogic.IDrawableInterface;
using AquariumLogic.PointExtensionClass;

namespace AquariumLogic.AquariumClass
{
    public class Aquarium : IAquarium, IDrawable
    {
        public IEnumerable<KeyValuePair<IFish, Point>> Fishes => fishesDictionary.AsEnumerable();
        public IEnumerable<KeyValuePair<IFood, Point>> Food => foodDictionary.AsEnumerable();
        public long IterationCount { get; private set; }
        public int IterateIntervalInMs { get; }
        private readonly Dictionary<IFish, Point> fishesDictionary = new Dictionary<IFish, Point>();
        private readonly Dictionary<IFood, Point> foodDictionary = new Dictionary<IFood, Point>();
        private readonly Timer autoIterateTimer = new Timer();


        public Aquarium(Size size, bool autoIterate = false, int iterateIntervalInMs = 100)
        {
            Size = size;
            IterateIntervalInMs = iterateIntervalInMs;
            if (autoIterate)
                InitializeAutoIterateTimer(iterateIntervalInMs);
        }

        private void InitializeAutoIterateTimer(int iterateIntervalInMs)
        {
            autoIterateTimer.Interval = iterateIntervalInMs;
            autoIterateTimer.Elapsed += (sender, args) => Iterate();
            autoIterateTimer.Enabled = true;
            Iterate();
        }

        public void AddFish(IFish fish, Point position)
        {
            if (IsOutOfBounds(position))
                throw new ArgumentOutOfRangeException();
            if (fishesDictionary.ContainsKey(fish)) return;
            fishesDictionary.Add(fish, position);
            fish.StartLiving();
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
            UpdateFishesPosition();

            UpdateFoodPosition();

            IterationCount++;
        }

        private void UpdateFoodPosition()
        {
            foreach (var food in foodDictionary.Keys.ToList())
            {
                var foodPosition = foodDictionary[food];
                foodDictionary[food] = foodPosition.AddVector(new Vector2(0, (float) food.Weight));
            }
        }

        private void UpdateFishesPosition()
        {
            foreach (var fish in fishesDictionary.Keys.ToList())
            {
                var fishPosition = fishesDictionary[fish];
                if (fish.IsHungry)
                {
                    var foodPosition = FindClosestFood(fishPosition);
                    fish.SetTargetVector(fishPosition.GetVectorToPoint(foodPosition));
                }
                fishesDictionary[fish] = fishPosition.AddVector(fish.Velocity);
            }
        }

        private Point FindClosestFood(Point fishPosition)
        {
            var minDistance = float.MaxValue;
            var result = new Point(0,0);
            foreach (var foodPosition in foodDictionary.Values)
            {
                var distance = fishPosition.GetVectorToPoint(foodPosition).Length();
                if (!(distance < minDistance)) continue;
                minDistance = distance;
                result = foodPosition;
            }

            return result;
        }

        private bool IsOutOfBounds(Point position)
        {
            return position.X < 0 || position.Y < 0 || position.X >= Size.Width || position.Y >= Size.Height;
        }

        public Size Size { get; }
        public Bitmap Texture { get; }
    }
}
