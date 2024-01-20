using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro.Examples;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CellScipt : MonoBehaviour
{
    public float valueHeight;
    public float valueVegetation;
    public GameObject Tree;
    public GameObject Grass;
    public GameObject Sheep;
    //public GameObject thisCell;

    public string type;
    public GameObject Vegetation;

    private float densForest = GenerateParams.DensityForest;
    private float densGrass = GenerateParams.DensityGrass;

    private GenerateAnimals genAn;
    private ChunkScript chSc;
    private GenerateMap genMap;
    private GameObject ent;

    void Start()
    {
        chSc = GetComponentInParent<ChunkScript>(); //
        genMap = chSc.GetComponentInParent<GenerateMap>(); //
        genAn = genMap.genAn; // конструкци€ из этих трЄх строк сомнительна€ дл€ получени€ genAn.

        type = name; // установка типа текущей €чейке (так удобнее потом делать проверки, какой у €чейки тип
        CreateTree();
        genAn.SpawnAnimalOnCell(gameObject);
        RandomColoring();
    }

    void Update()
    {
        
    }

    private void CreateTree()
    {
        if (type == "Land")
        {
            float rnd1 = Random.value;
            float rnd2 = Random.value;
            if (valueVegetation + densForest > rnd1) // спавн дерева здесь
            {
                Vegetation = Instantiate(Tree, transform.position, Quaternion.identity, transform);
                RandomizeTree();
            }
            else if (densGrass > rnd2) // спавн травы
            {
                Vegetation = Instantiate(Grass, transform.position, Quaternion.identity, transform);
            }

        }
    }

    private void RandomizeTree()
    {
        float rnd1 = Random.value * 0.3f - 0.15f;
        float rnd2 = Random.value * 0.3f - 0.15f;

        Vegetation.transform.localScale = new Vector3(1f + rnd1, 1f + rnd1, 1f + rnd1);
        SpriteRenderer spr = Vegetation.GetComponent<SpriteRenderer>();
        spr.color = new Color(0.15f, 0.4f + rnd2, 0.18f, 1f);
    }

    private void RandomColoring()
    {
        float clr = Random.value * 0.2f - 0.2f;
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (type == "Water")
        {
            float deep = valueHeight * 0.8f;
            spr.color = new Color(0.25f + deep, 0.45f + clr + deep, 0.87f + deep, 1);
        }
        else if (type == "Sand")
        {
            spr.color = new Color(0.93f, 0.89f, 0.42f + clr, 1);
        }
        else if (type == "Land")
        {
            float deep = valueHeight * 0.2f - 0.15f;
            spr.color = new Color(0.32f + deep, 0.84f + clr + deep, 0.34f + deep, 1);
        }
        else
        {
            float deep = valueHeight * 0.2f;
            spr.color = new Color(0.5f + Mathf.Abs(clr) - deep, 0.5f + Mathf.Abs(clr) - deep, 0.5f + Mathf.Abs(clr) - deep, 1);
        }
    }
}
