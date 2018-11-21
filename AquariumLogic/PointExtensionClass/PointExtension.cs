using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AquariumLogic.PointExtensionClass
{
    public static class PointExtension
    {
        public static Point AddVector(this Point point, Vector2 vector)
        {
            return new Point((int)(point.X + vector.X), (int)(point.Y + vector.Y));
        }

        public static Vector2 GetVectorToPoint(this Point from, Point to)
        {
            return new Vector2(to.X, to.Y) - new Vector2(from.X, from.Y);
        }
    }
}
