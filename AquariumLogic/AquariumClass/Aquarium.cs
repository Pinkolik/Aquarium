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
                if (!fish.IsAlive) continue;
                var fishDrawable = fishesDictionary[fish];
                if (!foodDictionary.IsNullOrEmpty() && fish.IsHungry)
                {
                    //if (IsInside())
                    var foodPair = FindClosestFood(fishDrawable);
                    var foodDrawable = foodPair.Value;
                    if (fishDrawable.HasCollisionWith(foodDrawable))
                        fish.ConsumeFood(foodPair.Key);
                    else
                        fish.SetTargetVector(fishDrawable.GetCenterPoint()
                            .GetVectorToPoint(foodDrawable.GetCenterPoint()));
                }

                fishesDictionary[fish] =
                    fishesDictionary[fish].UpdatePosition(fishDrawable.Position.AddVector(fish.Velocity));
            }
        }

        private KeyValuePair<IFood, IDrawable> FindClosestFood(IDrawable fishDrawable)
        {
            var minDistance = float.MaxValue;
            var fishCenterPoint = fishDrawable.GetCenterPoint();
            var result = new KeyValuePair<IFood, IDrawable>();
            foreach (var foodPair in foodDictionary)
            {
                var foodDrawable = foodPair.Value;
                var foodCenterPoint = foodDrawable.GetCenterPoint();
                var distance = fishCenterPoint.GetVectorToPoint(foodCenterPoint).Length();
                if (!(distance < minDistance)) continue;
                minDistance = distance;
                result = foodPair;
            }

            return result;
        }
        
        public Size Size { get; }
    }
}
