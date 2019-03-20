using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
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


        public Aquarium(Size size, Uri backgroundImageUri, bool autoIterate = false, int iterateIntervalInMs = 100)
        {
            Size = size;
            BackgroundImageUri = backgroundImageUri;
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

        public Uri BackgroundImageUri { get; }

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
                var newPos = foodDrawable.Position.AddVector(new Vector(0, (float) food.Weight));
                var newFoodDrawable = foodDictionary[food].UpdatePosition(newPos);
                if (!Size.IsOutOfBounds(newFoodDrawable))
                    foodDictionary[food] = newFoodDrawable;
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
                    {
                        fish.ConsumeFood(foodPair.Key);
                        foodDictionary.Remove(foodPair.Key);
                    }
                    else
                        fish.SetTargetVector(fishDrawable.GetCenterPoint()
                            .GetVectorToPoint(foodDrawable.GetCenterPoint()));
                }

                var newPos = fishDrawable.Position.AddVector(fish.Velocity);
                var newFishDrawable = fishesDictionary[fish].UpdatePosition(newPos);
                if (!Size.IsOutOfBounds(newFishDrawable))
                    fishesDictionary[fish] = newFishDrawable;
                else
                    fish.ChangeVelocity();
            }
        }

        private KeyValuePair<IFood, IDrawable> FindClosestFood(IDrawable fishDrawable)
        {
            var minDistance = double.MaxValue;
            var fishCenterPoint = fishDrawable.GetCenterPoint();
            var result = new KeyValuePair<IFood, IDrawable>();
            foreach (var foodPair in foodDictionary)
            {
                var foodDrawable = foodPair.Value;
                var foodCenterPoint = foodDrawable.GetCenterPoint();
                var distance = fishCenterPoint.GetVectorToPoint(foodCenterPoint).Length;
                if (!(distance < minDistance)) continue;
                minDistance = distance;
                result = foodPair;
            }

            return result;
        }

        public Size Size { get; }
    }
}