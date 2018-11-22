using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquariumLogic.IDrawableInterface
{
    public interface IDrawable
    {
        Point Position { get; }
        Size Size { get; }
        Bitmap Texture { get; }

        IDrawable UpdatePosition(Point newPosition);
    }
}
