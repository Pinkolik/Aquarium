using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AquariumLogic.IDrawableInterface;

namespace AquariumLogic.AquariumClass
{
    public class AquariumObject : IDrawable
    {
        public Point Position { get; }
        public Size Size { get; }
        public Uri TextureUri { get; }

        public AquariumObject(Point position, Size size, Uri textureUri)
        {
            Position = position;
            Size = size;
            TextureUri = textureUri;
        }

        public IDrawable UpdatePosition(Point newPosition)
        {
            return new AquariumObject(newPosition, Size, TextureUri);
        }
    }
}
