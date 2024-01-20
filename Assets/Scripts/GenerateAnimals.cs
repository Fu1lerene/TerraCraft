using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using System.Threading;

public class GenerateAnimals : MonoBehaviour
{
    public GameObject Sheep;
    public GameObject Map;
    public GameObject Land;

    public List<GameObject> sheeps = new List<GameObject>();
    private int sizeCh = GenerateParams.SizeChunk;
    private float sheepChance = 0.005f; // вероятность спавна овцы на траве
    private CellScipt cellSc;


    void Start()
    {

    }

    public void SpawnAnimalOnCell(GameObject cell)
    {
        cellSc = cell.GetComponent<CellScipt>();
        float rnd = Random.value;
        if ((cellSc.type == "Land") && (sheepChance > rnd)) // Спавн овцы
        {
            Vector3 pos = cell.transform.position;
            sheeps.Add(Instantiate(Sheep, pos, Quaternion.identity, transform));

        }
    }

    public void ToggleAnimalsInChunks(List<GameObject> chunks, bool status) // включение и выключение животных в чанке
    {
        foreach (var chunk in chunks)
        {
            float xCh = chunk.transform.position.x;
            float yCh = chunk.transform.position.y;
            foreach (var sheep in sheeps)
            {
                float xSh = sheep.transform.position.x;
                float ySh = sheep.transform.position.y;
                if ((xSh < xCh + sizeCh / 2) && (xSh > xCh - sizeCh / 2) &&
                   (ySh < yCh + sizeCh / 2) && (ySh > yCh - sizeCh / 2))
                {
                    sheep.SetActive(status);
                }
            }
        }

    } 

    void Update()
    {

    }
}
