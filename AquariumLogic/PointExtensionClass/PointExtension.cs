using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AquariumLogic.PointExtensionClass
{
    public static class PointExtension
    {
        public static Point AddVector(this Point point, Vector vector)
        {
            return new Point((int)(point.X + vector.X), (int)(point.Y + vector.Y));
        }

        public static Vector GetVectorToPoint(this Point from, Point to)
        {
            return new Vector(to.X, to.Y) - new Vector(from.X, from.Y);
        }
    }
}
