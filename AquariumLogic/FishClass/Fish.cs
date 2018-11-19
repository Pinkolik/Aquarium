using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using AquariumLogic.FoodClass;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.FishClass
{
    public class Fish : IFish, IDrawable
    {
        public double Health { get; private set; }
        public double MaxHealth { get; }
        public Vector2 Velocity { get; private set; }
        public bool IsAlive { get; private set; } = true;
        private double directionAngle = 0;
        private readonly int timeToLiveInSeconds;
        private readonly Timer healthTimer;
        private readonly int minVelocity;
        private readonly int maxVelocity;
        private const int HealthTimerIntervalInMs = 100;

        public Fish(double maxHealth
            , int timeToLiveInSeconds,
            int minVelocity, int maxVelocity, 
            Size size, Bitmap texture)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            this.timeToLiveInSeconds = timeToLiveInSeconds;
            Size = size;
            Texture = texture;
            this.minVelocity = minVelocity;
            this.maxVelocity = maxVelocity;
            healthTimer = new Timer(HealthTimerIntervalInMs);
            healthTimer.Elapsed += (sender, args) => ReduceHealth();
        }

        private void ReduceHealth()
        {
            Health = Math.Max(Health - MaxHealth / (timeToLiveInSeconds * 1000.0 / HealthTimerIntervalInMs), 0);
            if (Health != 0) return;
            IsAlive = false;
            healthTimer.Stop();
        }

        public void StartLiving()
        {
            healthTimer.Start();
            ReduceHealth();
            ChangeVelocity();
        }

        public void ChangeVelocity()
        {
            var random = new Random();
            directionAngle = 2 * Math.PI * random.NextDouble();
            var velocityLength = random.Next(minVelocity, maxVelocity + 1);
            var newX = (float) (velocityLength * Math.Cos(directionAngle));
            var newY = (float) (velocityLength * Math.Sin(directionAngle));
            Velocity = new Vector2(newX, newY);
        }

        public void ConsumeFood(IFood food)
        {
            if (!IsAlive) return;
            Health = Math.Min(MaxHealth, Health + food.HealthValue);
        }

        public Size Size { get; }
        public Bitmap Texture { get; }
    }
}