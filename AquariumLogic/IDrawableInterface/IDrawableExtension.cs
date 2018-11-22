using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquariumLogic.IDrawableInterface
{
    public static class IDrawableExtension
    {
        public static Point GetCenterPoint(this IDrawable drawable)
        {
            return new Point(drawable.Size.Width / 2 + drawable.Position.Y,
                drawable.Size.Height / 2 + drawable.Position.Y);
        }
    }
}
