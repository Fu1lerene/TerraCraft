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
        //private GenerateParams.WorldType worldType = GenerateParams.WorldType.Default;

        private Vector2[][] nodesGrid;

        public MyPerlin(Vector2[][] nodesGrid)
        {
            this.nodesGrid = nodesGrid;
        }

        public List<float> GetNoiseValues(GameObject currentChunk)
        {
            List<float> noiseValues = new List<float>();
            List<List<float>> octaveValues = new List<List<float>>();

            for (int k = 0; k < countOct; k++) // проход по всем октавам
            {
                int countNodes = (int)Mathf.Pow(2, k) + 1; // число узлов в сетке
                int sizeSquareOctave = sizeCh / (countNodes - 1); // размер квадрата в сетке
                SetNoiseValueOnOctave(currentChunk, octaveValues, k, countNodes, sizeSquareOctave);
            }
            for (int i = 0; i < sizeCh*sizeCh; i++) // комбинация октавных шумов
            {
                noiseValues.Add(0f);
                for (int k = 0; k < countOct; ++k)
                {
                    noiseValues[i] += octaveValues[k][i] / Mathf.Pow(2, k);
                }
            }
            return NormalizeNoise(noiseValues);
        }
        //private List<float> ChangeValuesByWorldType(Chunk chunk, List<float> values)
        //{
        //    float minValue = -2;
        //    float maxValue = 2;
        //    float islandSize = 4 * sizeCh;
        //    float lineSize = 4 * sizeCh;
        //    float cyrclesSize = 2 * sizeCh;

        //    for (int i = 0; i <  chunk.Cells.Count; i++)
        //    {
        //        //int x0;

        //        int x = (int)chunk.Cells[i].X;
        //        int y = (int)chunk.Cells[i].Y;               
        //        switch (worldType)
        //        {
        //            case "default":
        //                break;
        //            case "island":
        //                float x0 = GenerateParams.StartCountChunks * sizeCh;
        //                float y0 = GenerateParams.StartCountChunks * sizeCh;
        //                values[i] = values[i] - ((float) Mathf.Pow(x - x0, 2) / Mathf.Pow(islandSize, 2) +
        //                                        ((float) Mathf.Pow(y - y0, 2) / Mathf.Pow(islandSize, 2))) + 0.25f;
        //                break;
        //            case "line":
        //                y0 = GenerateParams.StartCountChunks*sizeCh;
        //                values[i] = values[i] + Mathf.Atan((y - y0)/lineSize);
        //                break;
        //            case "circles":
        //                y0 = GenerateParams.StartCountChunks * sizeCh;
        //                x0 = (GenerateParams.StartCountChunks - 1/2) * sizeCh;
        //                values[i] = values[i] + (Mathf.Cos(Mathf.Sqrt(Mathf.Pow(x-x0,2) + Mathf.Pow(y - y0, 2))/cyrclesSize) - 0.7f);
        //                break;
        //        }
        //        if (values[i] < minValue)
        //            values[i] = minValue;
        //        if (values[i] > maxValue) 
        //            values[i] = maxValue;
        //    }
        //    return values;
        //}
        private List<float> NormalizeNoise(List<float> values)
        {
            float minParam = -0.75f;
            float maxParam = 0.75f;
            for (int i = 0; i < sizeCh*sizeCh; i++) // нормировка шума
            {
                values[i] = (values[i] - minParam) / (maxParam - minParam);
                if (values[i] > 1)
                    values[i] = 1;
                if (values[i] < 0)
                    values[i] = 0;
            }
            return values;
        }
        private void SetNoiseValueOnOctave(GameObject chunk, List<List<float>> octaveValues, int k, int countNodes, int sizeSquareOctave)
        {
            octaveValues.Add(new List<float>());

            for (int i = 0; i < sizeCh*sizeCh; i++) // проход по всем клеткам чанка
            {
                List<Vector2> nodeVectors = new List<Vector2>();
                List<Vector2> pointVectors = new List<Vector2>();
                List<float> dotValues = new List<float>();

                float localPointFx = Mathf.Floor(i / sizeCh);
                float localPointFy = i % sizeCh;
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

                nodeVectors.Add(nodesGrid[k][index1]);
                nodeVectors.Add(nodesGrid[k][index2]);
                nodeVectors.Add(nodesGrid[k][index3]);
                nodeVectors.Add(nodesGrid[k][index4]);

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
        public List<float> SmoothingNoise(List<float> oldValues)
        {
            List<float> newValues = new List<float>();
            for (int i = 0; i < sizeCh; i++)
            {
                for (int j = 0; j < sizeCh; j++)
                {
                    int horizontalEmpty = 0;
                    int verticalEmpty = 0;
                    if (i == 0) // не брать клетку слева
                        horizontalEmpty = -1;
                    if (i == sizeCh - 1) // не брать клетку справа
                        horizontalEmpty = 1;
                    if (j == 0) // не брать клетку снизу
                        verticalEmpty = -1;
                    if (j == sizeCh - 1) // не брать клетку сверху
                        verticalEmpty = 1;
                    float k1 = (float)1 / 5;
                    float k2 = (1 - k1) / 4;
                    float k3 = (1 - k1) / 3;
                    float k4 = (1 - k1) / 2;


                    switch (horizontalEmpty, verticalEmpty)
                    {
                        case (0, 0):
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j - 1] * k2 + oldValues[i * sizeCh + j + 1] * k2 +
                                          oldValues[(i - 1) * sizeCh + j] * k2 + oldValues[(i + 1) * sizeCh + j] * k2);
                            break;
                        case (-1, 0):
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j - 1] * k3 + oldValues[i * sizeCh + j + 1] * k3 +
                                          oldValues[(i + 1) * sizeCh + j] * k3);
                            break;
                        case (-1, -1):
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j + 1] * k4 +
                                          oldValues[(i + 1) * sizeCh + j] * k4);
                            break;
                        case (-1, 1):
                            //Debug.Log($"{i} , {j}");
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j - 1] * k4 +
                                          oldValues[(i + 1) * sizeCh + j] * k4);
                            break;
                        case (0, -1):
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j + 1] * k3 +
                                          oldValues[(i - 1) * sizeCh + j] * k3 + oldValues[(i + 1) * sizeCh + j] * k3);
                            break;
                        case (0, 1):
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j - 1] * k3 +
                                          oldValues[(i - 1) * sizeCh + j] * k3 + oldValues[(i + 1) * sizeCh + j] * k3);
                            break;
                        case (1, -1):
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j + 1] * k4 +
                                          oldValues[(i - 1) * sizeCh + j] * k4);
                            break;
                        case (1, 0):
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j - 1] * k3 + oldValues[i * sizeCh + j + 1] * k3 +
                                          oldValues[(i - 1) * sizeCh + j] * k3);
                            break;
                        case (1, 1):
                            newValues.Add(oldValues[i * sizeCh + j] * k1 + oldValues[i * sizeCh + j - 1] * k4 +
                                          oldValues[(i - 1) * sizeCh + j] * k4);
                            break;
                    }
                }
            }
            return newValues;
        }

    }
}
