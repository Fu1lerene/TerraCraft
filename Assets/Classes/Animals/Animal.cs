using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Classes.Animals
{
    internal class Animal
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float HP { get; set; }
        public float Speed { get; set; }
        public GameObject AnimalObj {  get; set; }

        public Animal(float x, float y)
        {
            X = x;
            Y = y;
            HP = 100;
            Speed = 1;
        }

        protected virtual void Movement()
        {
            
        }
    }
}
