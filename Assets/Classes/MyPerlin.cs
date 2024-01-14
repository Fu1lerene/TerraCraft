using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Classes
{
    internal class MyPerlin
    {
        private int countOct = GenerateParams.CountOctaves;
        private int countNodes;
        private int sizeCh = GenerateParams.SizeChunk;
        private List<Chunk> map;

        public MyPerlin(List<Chunk> map)
        {
            this.map = map;
        }

        public List<float> GetNoiseValues(Chunk currentChunk)
        {
            SetAllNodes(currentChunk);
            return GetNoiseForChunk(currentChunk);
        }

        private List<float> GetNoiseForChunk(Chunk currentChunk)
        {
            List<float> noiseValues = new List<float>();
            List<List<float>> octaveValues = new List<List<float>>();

            for (int k = 0; k < countOct; k++) // проход по всем октавам
            {
                int countNodes = (int)Mathf.Pow(2, k) + 1; // число узлов в сетке
                int sizeSquareOctave = sizeCh / (countNodes - 1); // размер квадрата в сетке

                SetNoiseValueOnOctave(currentChunk, octaveValues, k, countNodes, sizeSquareOctave);
            }

            for (int i = 0; i < currentChunk.Cells.Count; i++) // комбинация октавных шумов
            {
                noiseValues.Add(0f);
                for (int k = 0; k < countOct; ++k)
                {
                    noiseValues[i] += octaveValues[k][i] / Mathf.Pow(2, k);
                }
            }

            //for (int i = 0; i < currentChunk.Cells.Count; i++)
            //    noiseValues[i] += 0.5f;

            return noiseValues;
        }

        private void SetNoiseValueOnOctave(Chunk currentChunk, List<List<float>> octaveValues, int k, int countNodes, int sizeSquareOctave)
        {
            octaveValues.Add(new List<float>());

            for (int i = 0; i < currentChunk.Cells.Count; i++) // проход по всем клеткам чанка
            {
                List<Vector2> nodeVectors = new List<Vector2>();
                List<Vector2> pointVectors = new List<Vector2>();
                List<float> dotValues = new List<float>();

                float localPointFx = currentChunk.Cells[i].X - (sizeCh * currentChunk.X);
                float localPointFy = currentChunk.Cells[i].Y - (sizeCh * currentChunk.Y);
                float px = (float)(localPointFx % sizeSquareOctave) / sizeSquareOctave; // координаты ячейки в текущем квадрате (от 0 до 1)
                float py = (float)(localPointFy % sizeSquareOctave) / sizeSquareOctave;

                int sx = (int)Mathf.Floor(localPointFx / sizeSquareOctave); // координата квадрата в сетке.
                int sy = (int)Mathf.Floor(localPointFy / sizeSquareOctave);

                pointVectors.Add(new Vector2(px, py));
                pointVectors.Add(new Vector2(px, 1 - py));
                pointVectors.Add(new Vector2(1 - px, py));
                pointVectors.Add(new Vector2(1 - px, 1 - py));

                int index1 = countNodes * sx + sy;
                int index2 = index1 + 1;
                int index3 = index1 + countNodes;
                int index4 = index3 + 1;

                nodeVectors.Add(currentChunk.Nodes[k][index1]);
                nodeVectors.Add(currentChunk.Nodes[k][index2]);
                nodeVectors.Add(currentChunk.Nodes[k][index3]);
                nodeVectors.Add(currentChunk.Nodes[k][index4]);

                for (int j = 0; j < 4; j++)
                {
                    dotValues.Add(Vector2.Dot(pointVectors[j], nodeVectors[j]));
                }

                float res1 = Lerp(dotValues[0], dotValues[1], py);
                float res2 = Lerp(dotValues[2], dotValues[3], py);
                float res3 = Lerp(res1, res2, px);
                octaveValues[k].Add(res3);
            }
        }

        private float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private void SetAllNodes(Chunk currentChunk)
        {
            SetSomeBoundNodes(currentChunk);

            for (int k = 0; k < countOct; k++)
            {
                int countAllNodes = (int)Mathf.Pow(Mathf.Pow(2, k) + 1, 2);

                for (int i = 0; i < countAllNodes; i++)
                {
                    if (currentChunk.Nodes[k][i] == new Vector2(0, 0))
                    {
                        currentChunk.Nodes[k][i] = GetRandomVector();
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
            foreach(var otherChunk in map)
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
                            currentChunk.Nodes[k][indexrl] = otherChunk.Nodes[k][i];
                            break;
                        case "left":
                            indexrl = (countNodes - 1) * countNodes + i;
                            currentChunk.Nodes[k][i] = otherChunk.Nodes[k][indexrl];
                            break;
                        case "top":
                            indextb1 = (i + 1) * countNodes - 1;
                            indextb2 = i * countNodes;
                            currentChunk.Nodes[k][indextb1] = otherChunk.Nodes[k][indextb2];
                            break;
                        case "bottom":
                            indextb1 = (i + 1) * countNodes - 1;
                            indextb2 = i * countNodes;
                            currentChunk.Nodes[k][indextb2] = otherChunk.Nodes[k][indextb1];
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
            if (otherChunk.Nodes[0][0] != new Vector2())
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
