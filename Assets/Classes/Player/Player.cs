using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Classes.Player
{
    public class Player
    {
        public float HP { get; set; }
        public float Speed { get; set; }
        public Stamina Stamina { get; set; }

        public Player() 
        {
            Stamina = new Stamina();
            HP = 100;
            Speed = 2;
        }
    }
}
