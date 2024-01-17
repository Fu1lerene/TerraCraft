using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Classes.Animals
{
    internal class Sheep : Animal
    {
        static int Count = 0;
        public Sheep(float x, float y) : base(x, y)
        {
            Count++;
        }
        
        protected override void Movement()
        {

        }
    }
}
