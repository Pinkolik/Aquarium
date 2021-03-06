﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FoodClass;
using System.Numerics;
using System.Windows;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.FishClass
{
    public interface IFish
    {
        double Health { get; }
        double MaxHealth { get; }
        Vector Velocity { get;  }
        bool IsAlive { get; }
        bool IsHungry { get; }
        event EventHandler OnHungry;

        void StartLiving();
        void ChangeVelocity();
        void ConsumeFood(IFood food);
        void SetTargetVector(Vector targetVector);
    }
}
