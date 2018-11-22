using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.AquariumClass
{
    public class AquariumObject : IDrawable
    {
        public Point Position { get; }
        public Size Size { get; }
        public Bitmap Texture { get; }

        public AquariumObject(Point position, Size size, Bitmap texture)
        {
            Position = position;
            Size = size;
            Texture = texture;
        }

        public IDrawable UpdatePosition(Point newPosition)
        {
            return new AquariumObject(newPosition, Size, Texture);
        }
    }
}
