using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquariumLogic.AquariumClass
{
    public class Aquarium
    {
        public List<object> Fishes { get; set; }

        public Aquarium()
        {
            Fishes = new List<object>();
        }

        public void AddFish()
        {
            Fishes.Add(new object());
        }
    }
}
