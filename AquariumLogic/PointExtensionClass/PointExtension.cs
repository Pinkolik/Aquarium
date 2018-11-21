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
    }
}
