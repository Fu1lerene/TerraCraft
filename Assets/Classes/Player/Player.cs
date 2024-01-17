using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Classes.Player
{
    public class Player
    {
        public float X {  get; set; }
        public float Y { get; set; }
        public float HP { get; set; }
        public float Speed { get; set; }
        public Stamina Stamina { get; set; }

        public Player(float x, float y, float hp = 100, float speed = 2) 
        {
            X = x;
            Y = y;
            Stamina = new Stamina();
            HP = hp;
            Speed = speed;
        }
    }
}
