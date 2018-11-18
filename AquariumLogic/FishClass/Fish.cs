using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using AquariumLogic.FoodClass;

namespace AquariumLogic.FishClass
{
    public class Fish : IFish
    {
        public double Health { get; private set; }
        public double MaxHealth { get; }
        public Vector2 Velocity { get; }
        private readonly int timeToLiveInSeconds;
        private readonly Timer healthTimer;
        private const int HealthTimerIntervalInMs = 100;

        public Fish(double maxHealth, int timeToLiveInSeconds)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            this.timeToLiveInSeconds = timeToLiveInSeconds;
            healthTimer = new Timer(HealthTimerIntervalInMs);
            healthTimer.Elapsed += (sender, args) => ReduceHealth();
        }

        private void ReduceHealth()
        {
            Health = Math.Max(Health - MaxHealth / (timeToLiveInSeconds * 1000.0 / HealthTimerIntervalInMs), 0);
        }

        public void StartLiving()
        {
            healthTimer.Start();
            ReduceHealth();
        }

        public void ConsumeFood(IFood food)
        {
            throw new NotImplementedException();
        }
    }
}