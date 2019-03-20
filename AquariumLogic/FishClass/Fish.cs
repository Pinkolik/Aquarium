using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using AquariumLogic.FoodClass;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.FishClass
{
    public class Fish : IFish
    {
        public double Health { get; private set; }
        public double MaxHealth { get; }
        public Vector Velocity { get; private set; }
        public bool IsAlive { get; private set; } = true;
        public bool IsHungry { get; private set; }
        public event EventHandler OnHungry;
        private double directionAngle = 0;
        private readonly int timeToLiveInSeconds;
        private readonly Timer healthTimer;
        private readonly int minVelocity;
        private readonly int maxVelocity;
        private const int HealthTimerIntervalInMs = 100;

        public Fish(double maxHealth
            , int timeToLiveInSeconds,
            int minVelocity, int maxVelocity)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            this.timeToLiveInSeconds = timeToLiveInSeconds;
            this.minVelocity = minVelocity;
            this.maxVelocity = maxVelocity;
            healthTimer = new Timer(HealthTimerIntervalInMs);
            healthTimer.Elapsed += (sender, args) => ReduceHealth();
        }

        private void ReduceHealth()
        {
            Health = Math.Max(Health - MaxHealth / (timeToLiveInSeconds * 1000.0 / HealthTimerIntervalInMs), 0);
            CheckFishHealth();
        }

        private void CheckFishHealth()
        {
            if (Health <= MaxHealth / 2)
            {
                IsHungry = true;
                OnHungry?.Invoke(this, EventArgs.Empty);
            }
            else
                IsHungry = false;

            if (Health != 0) return;
            IsAlive = false;
            healthTimer.Stop();
        }

        public void StartLiving()
        {
            healthTimer.Start();
            ReduceHealth();
        }

        public void ChangeVelocity()
        {
            var random = new Random();
            directionAngle = 2 * Math.PI * random.NextDouble();
            var velocityLength = random.Next(minVelocity, maxVelocity + 1);
            var newX = (float) (velocityLength * Math.Cos(directionAngle));
            var newY = (float) (velocityLength * Math.Sin(directionAngle));
            Velocity = new Vector(newX, newY);
        }

        public void ConsumeFood(IFood food)
        {
            if (!IsAlive) return;
            Health = Math.Min(MaxHealth, Health + food.HealthValue);
            CheckFishHealth();
            food.Consume();
        }

        public void SetTargetVector(Vector targetVector)
        {
            Velocity = targetVector / targetVector.Length * maxVelocity;
        }
    }
}