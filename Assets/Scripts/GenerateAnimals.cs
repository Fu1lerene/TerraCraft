using Assets.Classes;
using Assets.Classes.Animals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAnimals : MonoBehaviour
{
    public GameObject Sheep;
    public GameObject Map;
    public GameObject Land;

    public List<Animal> sheeps = new List<Animal>();
    public List<Animal> activeSheeps = new List<Animal>();
    private int sizeCh = GenerateParams.SizeChunk;


    void Start()
    {

    }

    public void SpawnAnimals(List<Chunk> chunks)
    {
        foreach (var chunk in chunks)
        {
            List<Animal> newSheeps = new List<Animal>();
            int count = 0;
            foreach (var cell in chunk.Cells) /// Количество допустимых клеток
            {
                if (cell.Type == Land)
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
                        if (cell.Type == Land)
                        {
                            newSheeps.Add(new Sheep(x, y));
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
            sheeps.AddRange(newSheeps);
            foreach (var sh in newSheeps)
                sh.AnimalObj = Instantiate(Sheep, new Vector3(sh.X, sh.Y, 0), Quaternion.identity, transform);
            activeSheeps.AddRange(newSheeps);
        }
    }

    void Update()
    {
    }
}
