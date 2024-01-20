using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Classes
{
    static class GenerateParams
    {
        public static int SizeChunk = 32;
        public static int LoadingDistance = 2;
        public static int CountOctaves = 3;
        public static int StartCountChunks = 2;

        public static float DensityForest = - 0.4f;
        public static float DensityGrass = 0.01f;
        public static float WaterLevel = 0.3f;
        public static float SandLevel = 0.35f;
        public static float LandLevel = 0.8f;

        public enum WorldType
        {
            Default,
            Island,
            Line,
            Circles
        }
    }
}
