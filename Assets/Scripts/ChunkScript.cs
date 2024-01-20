using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Classes;
using UnityEditor;


public class ChunkScript : MonoBehaviour
{
    public GameObject Land;
    public GameObject Water;
    public GameObject Sand;
    public GameObject Mountain;
    public GameObject Sheep;
    public int ID;

    private Vector2[][] NodeVectorsHeight = new Vector2[GenerateParams.CountOctaves][];
    private Vector2[][] NodeVectorsVegetation = new Vector2[GenerateParams.CountOctaves][];

    private static int _id;
    private int sizeCh = GenerateParams.SizeChunk;
    private GenerateMap genMap;
    private float x0;
    private float y0;
    private int countOct = GenerateParams.CountOctaves;
    private List<GameObject> cells = new List<GameObject>();
    private List<float> noiseValuesHeight;
    private List<float> noiseValuesVegetation;

    private float waterLevel = GenerateParams.WaterLevel;
    private float sandLevel = GenerateParams.SandLevel;
    private float landLevel = GenerateParams.LandLevel;

    public enum CellType
    {
        Land,
        Water,
        Sand,
        Mountain
    }

    void Start()
    {
        Initialization();

        GenerateCellsWithNoise();
    }

    private void Initialization()
    {
        ID = ++_id;
        x0 = transform.position.x;
        y0 = transform.position.y;
        for (int k = 0; k < countOct; k++)
        {
            int countAllNodes = (int)Mathf.Pow(Mathf.Pow(2, k) + 1, 2);
            NodeVectorsHeight[k] = new Vector2[countAllNodes];
            NodeVectorsVegetation[k] = new Vector2[countAllNodes];
        }
        genMap = GetComponentInParent<GenerateMap>();
    }

    private void GenerateCellsWithNoise()
    {
        SetNodeVectors();
        CreateCellsOnTheField();
    }

    private void CreateCellsOnTheField()
    {
        MyPerlin perlinHeight = new MyPerlin(NodeVectorsHeight); // шум для высот
        MyPerlin perlinVegetation = new MyPerlin(NodeVectorsVegetation); // шум для деревьев
        noiseValuesHeight = perlinHeight.GetNoiseValues(gameObject);
        noiseValuesVegetation = perlinVegetation.GetNoiseValues(gameObject);

        for (int i = 0; i < sizeCh; i++)
        {
            for (int j = 0; j < sizeCh; j++)
            {
                float x = x0 - sizeCh / 2 + 0.5f + i;
                float y = y0 - sizeCh / 2 + 0.5f + j;

                if (noiseValuesHeight[i * sizeCh + j] < waterLevel)
                {
                    CreateAndSetType(CellType.Water, i, j, x, y);
                }
                else if (noiseValuesHeight[i * sizeCh + j] < sandLevel)
                {
                    CreateAndSetType(CellType.Sand, i, j, x, y);
                }
                else if (noiseValuesHeight[i * sizeCh + j] < landLevel)
                {
                    CreateAndSetType(CellType.Land, i, j, x, y);
                }   
                else
                {
                    CreateAndSetType(CellType.Mountain, i, j, x, y);
                }
            }
        }
    }

    private void CreateAndSetType(CellType cellType, int i, int j, float x, float y)
    {
        switch (cellType)
        {
            case CellType.Water:
                cells.Add(Instantiate(Water, new Vector3(x, y, 0), Quaternion.identity, transform));
                CellScipt celSc = cells[i * sizeCh + j].GetComponent<CellScipt>();
                celSc.cellType = cellType;
                celSc.valueHeight = noiseValuesHeight[i * sizeCh + j];
                celSc.valueVegetation = noiseValuesVegetation[i * sizeCh + j];
                break;
            case CellType.Sand:
                cells.Add(Instantiate(Sand, new Vector3(x, y, 0), Quaternion.identity, transform));
                celSc = cells[i * sizeCh + j].GetComponent<CellScipt>();
                celSc.cellType = cellType;
                celSc.valueHeight = noiseValuesHeight[i * sizeCh + j];
                celSc.valueVegetation = noiseValuesVegetation[i * sizeCh + j];
                break;
            case CellType.Land:
                cells.Add(Instantiate(Land, new Vector3(x, y, 0), Quaternion.identity, transform));
                celSc = cells[i * sizeCh + j].GetComponent<CellScipt>();
                celSc.cellType = cellType;
                celSc.valueHeight = noiseValuesHeight[i * sizeCh + j];
                celSc.valueVegetation = noiseValuesVegetation[i * sizeCh + j];
                break;
            case CellType.Mountain:
                cells.Add(Instantiate(Mountain, new Vector3(x, y, 0), Quaternion.identity, transform));
                celSc = cells[i * sizeCh + j].GetComponent<CellScipt>();
                celSc.cellType = cellType;
                celSc.valueHeight = noiseValuesHeight[i * sizeCh + j];
                celSc.valueVegetation = noiseValuesVegetation[i * sizeCh + j];
                break;

        }


    }

    public void SetNodeVectors()
    {
        for (int i = 0; i < ID; i++)
        {
            GameObject chunk = genMap.map[i];
            if (IsFindedChunk(chunk, "left"))
            {
                SetNodeVectorsFromOtherChunk(chunk, "left");
            }
            if (IsFindedChunk(chunk, "right"))
            {
                SetNodeVectorsFromOtherChunk(chunk, "right");
            }
            if (IsFindedChunk(chunk, "top"))
            {
                SetNodeVectorsFromOtherChunk(chunk, "top");
            }
            if (IsFindedChunk(chunk, "bottom"))
            {
                SetNodeVectorsFromOtherChunk(chunk, "bottom");
            }
        }
        SetNewNodeVectors();
    }

    private void SetNewNodeVectors()
    {
        for (int k = 0; k < countOct; k++)
        {
            int countAllNodes = (int)Mathf.Pow(Mathf.Pow(2, k) + 1, 2);
            for (int i = 0; i < countAllNodes; ++i)
            {
                if (NodeVectorsHeight[k][i] == new Vector2())
                {
                    NodeVectorsHeight[k][i] = GetRandomVector();
                }
                if (NodeVectorsVegetation[k][i] == new Vector2())
                {
                    NodeVectorsVegetation[k][i] = GetRandomVector();
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

    private void SetNodeVectorsFromOtherChunk(GameObject otherChunk, string where)
    {
        ChunkScript chSc = otherChunk.GetComponent<ChunkScript>();
        for (int k = 0; k < countOct; k++)
        {
            int countNodes = (int)Mathf.Pow(2, k) + 1;
            for (int i = 0; i < countNodes; i++)
            {
                int index = countNodes * (countNodes - 1) + i;
                int indextb1 = (i + 1) * countNodes - 1;
                int indextb2 = i * countNodes;
                switch (where)
                {
                    case "left":
                        NodeVectorsHeight[k][i] = chSc.NodeVectorsHeight[k][index];
                        NodeVectorsVegetation[k][i] = chSc.NodeVectorsVegetation[k][index];
                        break;
                    case "right":
                        NodeVectorsHeight[k][index] = chSc.NodeVectorsHeight[k][i];
                        NodeVectorsVegetation[k][index] = chSc.NodeVectorsVegetation[k][i];
                        break;
                    case "top":
                        NodeVectorsHeight[k][indextb1] = chSc.NodeVectorsHeight[k][indextb2];
                        NodeVectorsVegetation[k][indextb1] = chSc.NodeVectorsVegetation[k][indextb2];
                        break;
                    case "bottom":
                        NodeVectorsHeight[k][indextb2] = chSc.NodeVectorsHeight[k][indextb1];
                        NodeVectorsVegetation[k][indextb2] = chSc.NodeVectorsVegetation[k][indextb1];
                        break;

                }
            }
        }
    }

    private bool IsFindedChunk(GameObject chunk, string where)
    {
        ChunkScript chSc = chunk.GetComponent<ChunkScript>();
        if (chSc.NodeVectorsHeight[0][0] != new Vector2())
        {
            switch (where)
            {
                case "left":
                    return chunk.transform.position.x == x0 - sizeCh && chunk.transform.position.y == y0;
                case "right":
                    return chunk.transform.position.x == x0 + sizeCh && chunk.transform.position.y == y0;
                case "top":
                    return chunk.transform.position.x == x0 && chunk.transform.position.y == y0 + sizeCh;
                case "bottom":
                    return chunk.transform.position.x == x0 && chunk.transform.position.y == y0 - sizeCh;
                default: return false;
            }
        }
        return false;
    }


    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {

    }
}
