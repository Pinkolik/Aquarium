using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.IDrawableInterface;
using AquariumLogic.PointExtensionClass;

namespace AquariumLogic.SizeExtensionClass
{
    public static class SizeExtension
    {
        public static bool IsOutOfBounds(this Size size, IDrawable obj)
        {
            return size.IsOutOfBounds(obj.Position)
                   || size.IsOutOfBounds(obj.Position.AddVector(new Vector2(obj.Size.Width, 0)))
                   || size.IsOutOfBounds(obj.Position.AddVector(new Vector2(obj.Size.Width, obj.Size.Height)))
                   || size.IsOutOfBounds(obj.Position.AddVector(new Vector2(0, obj.Size.Height)));

        }

        public static bool IsOutOfBounds(this Size size, Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X >= size.Width || point.Y >= size.Height;
        }
    }
}
