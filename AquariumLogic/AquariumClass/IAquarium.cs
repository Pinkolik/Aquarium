﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.FishClass;
using AquariumLogic.FoodClass;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.AquariumClass
{
    public interface IAquarium
    {
        IReadOnlyDictionary<IFish, IDrawable> Fishes { get; }
        IReadOnlyDictionary<IFood, IDrawable> Food { get; }
        long IterationCount { get; }
        int IterateIntervalInMs { get; }
        Size Size { get; }
        Uri BackgroundImageUri { get; }

        void AddFish(IFish fish, IDrawable drawable);
        void AddFood(IFood food, IDrawable drawable);
        void Iterate();
    }
}
