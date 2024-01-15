using Assets;
using Assets.Classes;
//using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateMap : MonoBehaviour
{
    public GameObject entities;
    public GameObject land;
    public GameObject water;
    public GameObject sand;
    public GameObject mountain;
    public GameObject Map;
    public GameObject ChunkPref;
    public GameObject player;

    private List<Chunk> map = new List<Chunk>();
    private List<Chunk> activeMap = new List<Chunk>();
    private Vector2 currentChunk = new Vector2();
    private Vector2 prevChunk = new Vector2();

    private Transform transformMap;
    private int sizeCh = GenerateParams.SizeChunk;
    private int distLoad = GenerateParams.LoadingDistance;
    private MyPerlin perlin;
    private int startCountChunks = GenerateParams.StartCountChunks;
    private float waterLevel = 0.3f;
    private float sandLevel = 0.35f;
    private float landLevel = 0.8f;
    //private float landLevel = 0.8f;
    

    void Start()
    {
        CreateStartChunks();
        SetAndCreateCellsOnTheMap(map);
        currentChunk = new Vector2(Mathf.Floor(player.transform.position.x / sizeCh),
                               Mathf.Floor(player.transform.position.y / sizeCh));
    }

    private void Update()
    {
        LoadAndShowActualChunks();
    }

    private void RandomColoring(Chunk chunk)
    {
        for (int i = 0; i < chunk.Cells.Count; i++)
        {
            float clr = Random.value * 0.2f - 0.2f;
            SpriteRenderer spr = chunk.Cells[i].CellObject.GetComponent<SpriteRenderer>();
            if (chunk.Cells[i].Type == water)
            {
                float deep = chunk.Cells[i].Value * 0.8f;
                spr.color = new UnityEngine.Color(0.25f + deep, 0.45f + clr + deep, 0.87f + deep, 1);
            }
            if (chunk.Cells[i].Type == land)
            {
                float deep = chunk.Cells[i].Value * 0.2f - 0.15f;
                spr.color = new UnityEngine.Color(0.32f + deep, 0.84f + clr + deep, 0.34f + deep, 1);
            }
            if (chunk.Cells[i].Type == sand)
            {
                float deep = chunk.Cells[i].Value * 0.1f;
                spr.color = new UnityEngine.Color(0.93f, 0.89f, 0.42f + clr, 1);
            }
            if (chunk.Cells[i].Type == mountain)
            {
                float deep = chunk.Cells[i].Value * 0.2f;
                spr.color = new UnityEngine.Color(0.5f + Mathf.Abs(clr) - deep, 0.5f + Mathf.Abs(clr) - deep, 0.5f + Mathf.Abs(clr) - deep, 1);
            }
        }
    }

    private void LoadAndShowActualChunks()
    {
        prevChunk = currentChunk;
        currentChunk = new Vector2(Mathf.Floor(player.transform.position.x / sizeCh),
                                   Mathf.Floor(player.transform.position.y / sizeCh));

        if (currentChunk.x > prevChunk.x) // �������� ������ 
        {
            ShowActualChunks(1, 0);
        }
        if (currentChunk.x < prevChunk.x) // �������� �����
        {
            ShowActualChunks(-1, 0);
        }
        if (currentChunk.y > prevChunk.y) // �������� ������
        {
            ShowActualChunks(0, 1);
        }
        if (currentChunk.y < prevChunk.y) // �������� �����
        {
            ShowActualChunks(0, -1);
        }
    }

    private void ShowActualChunks(int dirx, int diry)
    {
        List<Chunk> newChunks = new List<Chunk>();
        for (int i = -distLoad; i < distLoad + 1; i++)
        {
            bool existFlag = false;
            for (int k = 0; k < map.Count; k++)
            {
                bool checkX = map[k].X == currentChunk.x + distLoad * dirx + i * diry;
                bool checkY = map[k].Y == currentChunk.y + distLoad * diry + i * dirx;
                if (checkX && checkY) // ��������� ��� ������������� �����
                {
                    existFlag = true;
                    map[k].ChunkV.SetActive(true);
                    activeMap.Add(map[k]);
                    break;
                }
            }
            if (!existFlag) // �������� ������ �����
            {
                int x = (int)currentChunk.x + distLoad * dirx + i * diry;
                int y = (int)currentChunk.y + distLoad * diry + i * dirx;
                newChunks.Add(new Chunk(x, y, Instantiate(ChunkPref, transformMap)));
            }
            int x1 = (int)currentChunk.x - dirx - distLoad * dirx + i * diry;
            int y1 = (int)currentChunk.y - diry - distLoad * diry + i * dirx;
            DeactivateChunk(x1, y1);
        }

        map.AddRange(newChunks);
        activeMap.AddRange(newChunks);
        SetAndCreateCellsOnTheMap(newChunks);
    }

    private void DeactivateChunk(int x, int y)
    {
        foreach (Chunk chunk in map)
        {
            if ((chunk.X == x) && (chunk.Y == y))
            {
               //chunk.ChunkV.SetActive(false);
               activeMap.Remove(chunk);
            }
        }
    }

    private void SetAndCreateCellsOnTheMap(List<Chunk> chunks)
    {
        perlin = new MyPerlin(map);
        foreach (Chunk chunk in chunks)
        {
            List<float> noiseValues = perlin.GetNoiseValues(chunk);
            for (int i = 0; i < chunk.Cells.Count; i++)
            {
                chunk.Cells[i].Value = noiseValues[i];
            }
            SetTypeToCellsInChunk(chunk);
            CreateCellsOnTheMap(chunk);
            RandomColoring(chunk);
        }
    }

    private static void CreateCellsOnTheMap(Chunk chunk)
    {
        Transform transformChunk = chunk.ChunkV.GetComponent<Transform>();
        for (int i = 0; i < chunk.Cells.Count; i++)
        {
            chunk.Cells[i].CellObject = Instantiate(chunk.Cells[i].Type, new Vector3(chunk.Cells[i].X, chunk.Cells[i].Y, 0), Quaternion.identity, transformChunk);
        }
    }

    private void SetTypeToCellsInChunk(Chunk chunk)
    {
        foreach (var cell in chunk.Cells)
        {
            if (cell.Value < waterLevel)
                cell.Type = water;
            else if (cell.Value < sandLevel)
                cell.Type = sand;
            else if (cell.Value < landLevel)
                cell.Type = land;
            else
                cell.Type = mountain;
        }
    }

    private void CreateStartChunks()
    {
        transformMap = Map.transform;
        for (int i = 0; i < 2 * startCountChunks + 1; i++)
        {
            for (int j = 0; j < 2 * startCountChunks + 1; j++)
            {
                Chunk newChunk = new Chunk(i, j, Instantiate(ChunkPref, transformMap));
                map.Add(newChunk);
                if ((i <= startCountChunks + distLoad) && (i >= startCountChunks - distLoad) &&
                    (j <= startCountChunks + distLoad) && (j >= startCountChunks - distLoad))
                {
                    newChunk.ChunkV.SetActive(true);
                    activeMap.Add(newChunk);
                }
                else
                {
                    newChunk.ChunkV.SetActive(false);
                }
            }
        }
    }
}
