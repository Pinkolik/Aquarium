using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquariumLogic.IDrawable
{
    public interface IDrawable
    {
        Size Size { get; }
        Bitmap Texture { get; }
    }
}
