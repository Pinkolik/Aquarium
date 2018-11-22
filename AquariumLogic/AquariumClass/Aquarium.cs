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
using AquariumLogic.SizeExtensionClass;
using Castle.Core.Internal;

namespace AquariumLogic.AquariumClass
{
    public class Aquarium : IAquarium
    {
        public IEnumerable<KeyValuePair<IFish, IDrawable>> Fishes => fishesDictionary.AsEnumerable();
        public IEnumerable<KeyValuePair<IFood, IDrawable>> Food => foodDictionary.AsEnumerable();
        public long IterationCount { get; private set; }
        public int IterateIntervalInMs { get; }
        private readonly Dictionary<IFish, IDrawable> fishesDictionary = new Dictionary<IFish, IDrawable>();
        private readonly Dictionary<IFood, IDrawable> foodDictionary = new Dictionary<IFood, IDrawable>();
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

        public Bitmap BackgroundImage { get; }

        public void AddFish(IFish fish, IDrawable drawable)
        {
            if (Size.IsOutOfBounds(drawable))
                throw new ArgumentOutOfRangeException();
            if (fishesDictionary.ContainsKey(fish)) return;
            fishesDictionary.Add(fish, drawable);
            fish.StartLiving();
        }

        public void AddFood(IFood food, IDrawable drawable)
        {
            if (Size.IsOutOfBounds(drawable))
                throw new ArgumentOutOfRangeException();
            if (!foodDictionary.ContainsKey(food))
                foodDictionary.Add(food, drawable);
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
                var foodDrawable = foodDictionary[food];
                foodDictionary[food] = foodDictionary[food]
                    .UpdatePosition(foodDrawable.Position.AddVector(new Vector2(0, (float) food.Weight)));
            }
        }

        private void UpdateFishesPosition()
        {
            foreach (var fish in fishesDictionary.Keys.ToList())
            {
                if (!fish.IsAlive) continue;;
                var fishDrawable = fishesDictionary[fish];
                if (!foodDictionary.IsNullOrEmpty() && fish.IsHungry)
                {
                    //if (IsInside())
                    var foodDrawable = FindClosestFood(fishDrawable);
                    fish.SetTargetVector(fishDrawable.Position.GetVectorToPoint(foodDrawable.Position));
                }

                fishesDictionary[fish] =
                    fishesDictionary[fish].UpdatePosition(fishDrawable.Position.AddVector(fish.Velocity));
            }
        }

        private IDrawable FindClosestFood(IDrawable fishDrawable)
        {
            var minDistance = float.MaxValue;
            var fishCenterPoint = new Point(fishDrawable.Size.Width + fishDrawable.Position.Y,
                fishDrawable.Size.Height + fishDrawable.Position.Y);
            IDrawable result = null;
            foreach (var foodDrawable in foodDictionary.Values)
            {
                var distance = fishDrawable.Position.GetVectorToPoint(foodDrawable.Position).Length();
                if (!(distance < minDistance)) continue;
                minDistance = distance;
                result = foodDrawable;
            }

            return result;
        }

        private bool IsOutOfBounds(Point position)
        {
            return position.X < 0 || position.Y < 0 || position.X >= Size.Width || position.Y >= Size.Height;
        }
        public Size Size { get; }
    }
}
