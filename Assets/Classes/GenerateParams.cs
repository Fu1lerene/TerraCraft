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
        public static int LoadingDistance = 3;
        public static int CountOctaves = 3;
        public static int StartCountChunks = 7;

        public static float DensityForest = - 0.4f;
        public static float DensityGrass = 0.01f;

        public static string WorldType = "default"; // default, island, line, circles
    }
}
