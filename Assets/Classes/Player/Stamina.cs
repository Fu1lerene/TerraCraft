using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Classes.Player
{
    public class Stamina
    {
        public float Value { get; set; }
        public float MaxValue { get; set; }
        public float FallRate { get; set; }
        public float RegenRate { get; set; }
        
        public Stamina(float maxValue = 100, float fallRate = 10f, float regeRate = 8f)
        {
            Value = maxValue;
            MaxValue = maxValue;
            FallRate = fallRate;
            RegenRate = regeRate;
        }
    }
}
