using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Classes.Player;
using Assets.Classes;

public class AbstractActions : MonoBehaviour
{
    public GameObject entities;
    public GameObject Map;

    private Player pl;
    private List<Chunk> activeMap;
    private Vector2 posCurrentChunk;
    private Vector2 posPrevChunk;
    private int sizeCh = GenerateParams.SizeChunk; 
    private GenerateMap genMap;

    void Start()
    {
        pl = entities.GetComponent<CreatePlayer>().pl;
        posCurrentChunk = new Vector2(Mathf.Floor((pl.X + 0.5f) / sizeCh), Mathf.Floor((pl.Y + 0.5f) / sizeCh));
    }


    void Update()
    {
        pl = entities.GetComponent<CreatePlayer>().pl;
        activeMap = Map.GetComponent<GenerateMap>().activeMap;
        genMap = Map.GetComponent<GenerateMap>();

        posPrevChunk = posCurrentChunk;
        posCurrentChunk = new Vector2(Mathf.Floor((pl.X + 0.5f) / sizeCh), Mathf.Floor((pl.Y + 0.5f) / sizeCh));
        Debug.Log(genMap.PosCurrentChunk);

        genMap.PosCurrentChunk = posCurrentChunk;
        genMap.PosPrevChunk = posPrevChunk;

        genMap.LoadAndShowActualChunks();
    }
}
