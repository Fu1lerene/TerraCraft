using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Classes.Player;
using Assets.Classes;

public class AbstractActions : MonoBehaviour
{
    public GameObject Map;
    public GameObject Entities;
    public GameObject Chunk;

    private GameObject player;
    private List<Chunk> activeMap;
    private List<GameObject> sheeps;
    private Vector2 posCurrentChunk;
    private Vector2 posPrevChunk;
    private int sizeCh = GenerateParams.SizeChunk; 
    private GenerateMap genMap;
    private GenerateAnimals genAn;
    private int distLoad = GenerateParams.LoadingDistance;
    private List<Chunk> map;
    void Start()
    {
        GetComoponentsFromMap();

        posCurrentChunk = new Vector2(Mathf.Floor((player.transform.position.x + 0.5f) / sizeCh),
                            Mathf.Floor((player.transform.position.y + 0.5f) / sizeCh));

        genMap.CreateStartChunks();
        map = genMap.map;
        genMap.SetAndCreateCellsOnTheMap(map);

        genAn.SpawnAnimals(map);
    }

    private void GetComoponentsFromMap()
    {
        player = Entities.GetComponent<CreatePlayer>().player;
        genAn = Entities.GetComponent<GenerateAnimals>();
        genMap = Map.GetComponent<GenerateMap>();
        activeMap = genMap.activeMap;
        sheeps = genAn.sheeps;
    }

    void Update()
    {
        posPrevChunk = posCurrentChunk;
        posCurrentChunk = new Vector2(Mathf.Floor((player.transform.position.x + 0.5f) / sizeCh),
                                     Mathf.Floor((player.transform.position.y + 0.5f) / sizeCh));

        LoadAndShowActualChunks();
        genMap.posCurrentChunk = posCurrentChunk;
        genMap.activeMap = activeMap;
        map = genMap.map;
    }
    private void LoadAndShowActualChunks()
    {
        if (posCurrentChunk.x > posPrevChunk.x) // добавить справа 
        {
            ShowActualChunks(1, 0);
        }
        if (posCurrentChunk.x < posPrevChunk.x) // добавить слева
        {
            ShowActualChunks(-1, 0);
        }
        if (posCurrentChunk.y > posPrevChunk.y) // добавить сверху
        {
            ShowActualChunks(0, 1);
        }
        if (posCurrentChunk.y < posPrevChunk.y) // добавить снизу
        {
            ShowActualChunks(0, -1);
        }
    }

    private void ShowActualChunks(int dirx, int diry)
    {
        List<Chunk> newChunks = new List<Chunk>();
        List<Chunk> loadChunks = new List<Chunk>();
        List<Chunk> oldChunks = new List<Chunk>();
        for (int i = -distLoad; i < distLoad + 1; i++)
        {
            bool existFlag = false;
            for (int k = 0; k < map.Count; k++)
            {
                bool checkX = map[k].X == posCurrentChunk.x + distLoad * dirx + i * diry;
                bool checkY = map[k].Y == posCurrentChunk.y + distLoad * diry + i * dirx;
                if (checkX && checkY) // подгрузка уже существующего чанка
                {
                    existFlag = true;
                    loadChunks.Add(map[k]);
                    break;
                }
            }
            if (!existFlag) // создание нового чанка
            {
                int x = (int)posCurrentChunk.x + distLoad * dirx + i * diry;
                int y = (int)posCurrentChunk.y + distLoad * diry + i * dirx;
                newChunks.Add(new Chunk(x, y, Instantiate(Chunk, Map.transform)));
            }
            int x1 = (int)posCurrentChunk.x - dirx - distLoad * dirx + i * diry;
            int y1 = (int)posCurrentChunk.y - diry - distLoad * diry + i * dirx;
            foreach (var chunk in activeMap)
            {
                if (chunk.X == x1 && chunk.Y == y1)
                {
                    oldChunks.Add(chunk);
                }
            }
        }

        LoadObjects(loadChunks);
        CreateObjects(newChunks);
        DeactivateObjects(oldChunks);
    }
    private void DeactivateObjects(List<Chunk> chunks)
    {
        foreach(var chunk in chunks)
        {
            chunk.ChunkV.SetActive(false);
            activeMap.Remove(chunk);

            /*
             удалить из activeSheeps
             
             
             */
        }
    }
    private void CreateObjects(List<Chunk> chunks)
    {
        genMap.SetAndCreateCellsOnTheMap(chunks);
        genAn.SpawnAnimals(chunks);
        activeMap.AddRange(chunks);
    }

    private void LoadObjects(List<Chunk> chunks)
    {
        foreach (var chunk in chunks)
        {
            chunk.ChunkV.SetActive(true);
        }
        activeMap.AddRange(chunks);
    }
}
