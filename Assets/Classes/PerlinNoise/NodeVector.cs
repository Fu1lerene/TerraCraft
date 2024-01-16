using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Classes
{
    internal class NodeVector
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Vector { get; set; }

        public NodeVector(int x, int y, Vector2 vector)
        {
            X = x;
            Y = y;
            Vector = vector;
        }
    }
}
