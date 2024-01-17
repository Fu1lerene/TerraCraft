using Assets.Classes;
using Assets.Classes.Animals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAnimals : MonoBehaviour
{
    public GameObject sheep;
    public GameObject map;
    public GameObject entities;
    public GameObject land;


    public List<Chunk> activeMap;
    private List<Animal> sheeps = new List<Animal>();
    private int sizeCh = GenerateParams.SizeChunk;


    void Start()
    {
        activeMap = map.GetComponent<GenerateMap>().activeMap;
        SpawnAnimals(activeMap);
    }

    private void SpawnAnimals(List<Chunk> chunks)
    {
        foreach (var chunk in chunks)
        {
            int count = 0;
            foreach (var cell in chunk.Cells) /// Количество допустимых клеток
            {
                if (cell.Type == land)
                    count++;
            }

            float dolya = (float)count / (sizeCh * sizeCh);

            for (int i = 0; i < 5 * dolya; i++)
            {
                float x = Mathf.Floor(chunk.X * sizeCh + Random.Range(0, sizeCh - 1));
                float y = Mathf.Floor(chunk.Y * sizeCh + Random.Range(0, sizeCh - 1));
                foreach (var cell in chunk.Cells)
                {
                    if (cell.X == x && cell.Y == y)
                    {
                        if (cell.Type == land)
                        {
                            sheeps.Add(new Sheep(x, y));
                            break;
                        }
                        else
                        {
                            i--;
                            break;
                        }
                    }
                }
            }
        }
        foreach (var sh in sheeps) /// Спавн животных
        {
            sh.AnimalObj = Instantiate(sheep, new Vector3(sh.X, sh.Y, 0), Quaternion.identity, entities.transform);
        }
    }

    void Update()
    {
        
    }
}
