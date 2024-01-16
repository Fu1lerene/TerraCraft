//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Classes
{
    internal class NodeVectors
    {
        private int countNodes;
        private int sizeCh = GenerateParams.SizeChunk;
        private int countOct = GenerateParams.CountOctaves;
        private List<Chunk> map;

        public NodeVectors(List<Chunk> map)
        {
            this.map = map;
        }

        public void SetNodes(Chunk currentChunk)
        {
            SetAllNodesForNodes(currentChunk);
        }
        private void SetAllNodesForNodes(Chunk currentChunk)
        {
            SetSomeBoundNodes(currentChunk);
            for (int k = 0; k < countOct; k++)
            {
                int countAllNodes = (int)Mathf.Pow(Mathf.Pow(2, k) + 1, 2);
                for (int i = 0; i < countAllNodes; i++)
                {
                    if (currentChunk.NodesHeight[k][i] == new Vector2(0, 0))
                    {
                        currentChunk.NodesHeight[k][i] = GetRandomVector();
                    }
                    if (currentChunk.NodesTrees[k][i] == new Vector2(0, 0))
                    {
                        currentChunk.NodesTrees[k][i] = GetRandomVector();
                    }
                }
            }
        }

        private Vector2 GetRandomVector()
        {
            float x1 = (float)(2 * Random.value - 1);
            float x2 = (float)(2 * Random.value - 1);

            return new Vector2(x1 / Mathf.Sqrt(x1 * x1 + x2 * x2), x2 / Mathf.Sqrt(x1 * x1 + x2 * x2));
        }

        private void SetSomeBoundNodes(Chunk currentChunk)
        {
            foreach (var otherChunk in map)
            {
                for (int i = 0; i < 2; i++) // проход по типам узлов
                {
                    if (IsFindedChunk(otherChunk, currentChunk, "right")) // найден чанк справа
                    {
                        BindChunks(otherChunk, currentChunk, "right");
                    }
                    if (IsFindedChunk(otherChunk, currentChunk, "left")) // найден чанк слева
                    {
                        BindChunks(otherChunk, currentChunk, "left");
                    }
                    if (IsFindedChunk(otherChunk, currentChunk, "top")) // найден чанк сверху
                    {
                        BindChunks(otherChunk, currentChunk, "top");
                    }
                    if (IsFindedChunk(otherChunk, currentChunk, "bottom")) // найден чанк снизу
                    {
                        BindChunks(otherChunk, currentChunk, "bottom");
                    }
                }

            }
        }

        private void BindChunks(Chunk otherChunk, Chunk currentChunk, string side)
        {
            for (int k = 0; k < countOct; k++)
            {
                countNodes = (int)Mathf.Pow(2, k) + 1;
                for (int i = 0; i < countNodes; i++)
                {
                    //Vector2 randomVector = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                    int indexrl = 0;
                    int indextb1 = 0;
                    int indextb2 = 0;
                    switch (side)
                    {
                        case "right":
                            indexrl = (countNodes - 1) * countNodes + i;
                            currentChunk.NodesHeight[k][indexrl] = otherChunk.NodesHeight[k][i];
                            currentChunk.NodesTrees[k][indexrl] = otherChunk.NodesTrees[k][i];
                            break;
                        case "left":
                            indexrl = (countNodes - 1) * countNodes + i;
                            currentChunk.NodesHeight[k][i] = otherChunk.NodesHeight[k][indexrl];
                            currentChunk.NodesTrees[k][i] = otherChunk.NodesTrees[k][indexrl];
                            break;
                        case "top":
                            indextb1 = (i + 1) * countNodes - 1;
                            indextb2 = i * countNodes;
                            currentChunk.NodesHeight[k][indextb1] = otherChunk.NodesHeight[k][indextb2];
                            currentChunk.NodesTrees[k][indextb1] = otherChunk.NodesTrees[k][indextb2];
                            break;
                        case "bottom":
                            indextb1 = (i + 1) * countNodes - 1;
                            indextb2 = i * countNodes;
                            currentChunk.NodesHeight[k][indextb2] = otherChunk.NodesHeight[k][indextb1];
                            currentChunk.NodesTrees[k][indextb2] = otherChunk.NodesTrees[k][indextb1];
                            break;
                        default:
                            Debug.Log("Ошибка");
                            break;
                    }
                }
            }
        }

        private bool IsFindedChunk(Chunk otherChunk, Chunk currentChunk, string where)
        {
            if (otherChunk.NodesHeight[0][0] != new Vector2())
            {
                switch (where)
                {
                    case "right":
                        return (otherChunk.X == currentChunk.X + 1) && (otherChunk.Y == currentChunk.Y);
                    case "left":
                        return (otherChunk.X == currentChunk.X - 1) && (otherChunk.Y == currentChunk.Y);
                    case "top":
                        return (otherChunk.X == currentChunk.X) && (otherChunk.Y == currentChunk.Y + 1);
                    case "bottom":
                        return (otherChunk.X == currentChunk.X) && (otherChunk.Y == currentChunk.Y - 1);
                    default: return false;
                }
            }
            return false;
        }
    }
}
