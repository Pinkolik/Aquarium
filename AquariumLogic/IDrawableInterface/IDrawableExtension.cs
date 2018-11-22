using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.PointExtensionClass;
using AquariumLogic.SizeExtensionClass;

namespace AquariumLogic.IDrawableInterface
{
    public static class IDrawableExtension
    {
        public static Point GetCenterPoint(this IDrawable drawable)
        {
            return new Point(drawable.Size.Width / 2 + drawable.Position.Y,
                drawable.Size.Height / 2 + drawable.Position.Y);
        }

        public static bool HasCollisionWith(this IDrawable drawable, IDrawable obj)
        {
            var containerPos = drawable.Position;
            var containerSize = drawable.Size;
            var relativeObjPos = obj.Position.AddVector(new Vector2(-containerPos.X, -containerPos.Y));
            return !(containerSize.IsOutOfBounds(relativeObjPos)
                    && containerSize.IsOutOfBounds(relativeObjPos.AddVector(new Vector2(obj.Size.Width, 0)))
                    && containerSize.IsOutOfBounds(
                        relativeObjPos.AddVector(new Vector2(obj.Size.Width, obj.Size.Height)))
                    && containerSize.IsOutOfBounds(relativeObjPos.AddVector(new Vector2(0, obj.Size.Height))));
        }
    }
}
