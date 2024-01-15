using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Classes
{
    internal class Chunk
    {
        private int sizeCh = GenerateParams.SizeChunk;
        private int CountOct = GenerateParams.CountOctaves;

        public int X { get; set; }
        public int Y { get; set; }
        public List<Cell> Cells { get; set; }
        public GameObject ChunkV { get; set; }
        public List<List<Vector2>> Nodes { get; set; }
        //public List<List<Vector2>> NodesTrees { get; set; }

        public Chunk(int x, int y, GameObject chunkV)
        {
            X = x;
            Y = y;
            ChunkV = chunkV;
            Nodes = new List<List<Vector2>>();
            Cells = new List<Cell>();

            GenerateCells(x, y);
            GenerateEmptyNodes();
        }

        private void GenerateCells(int x, int y)
        {
            for (int i = 0; i < sizeCh; i++)
            {
                for (int j = 0; j < sizeCh; j++)
                {
                    Cells.Add(new Cell(i + x * sizeCh, j + y * sizeCh));
                }
            }
        }

        private void GenerateEmptyNodes()
        {
            for (int k = 0; k < CountOct; k++)
            {
                Nodes.Add(new List<Vector2>()); // лист узлов для генерации высот
                int countAllNodes = (int)Math.Pow(Math.Pow(2, k) + 1, 2);
                for (int i = 0; i < countAllNodes; i++)
                {
                    Nodes[k].Add(new Vector2(0, 0));
                }
            }
        }

        public void SetNodesHeight(Chunk chunk, List<List<Vector2>> nodes)
        {
            chunk.Nodes = nodes;
        }
    }
}
