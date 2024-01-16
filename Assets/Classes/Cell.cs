using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Classes
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Value { get; set; }
        public float VegetationValue { get; set; }
        public GameObject Type { get; set; }
        public GameObject Vegetation {  get; set; }
        public GameObject CellObject { get; set; }
        public GameObject VegetationObject { get; set; }
        

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Type = null;
            Vegetation = null;
            CellObject = null;
            VegetationObject = null;
            Value = 0;
            VegetationValue = 0;
        }

    }
}
