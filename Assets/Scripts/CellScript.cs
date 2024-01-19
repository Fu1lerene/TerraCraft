using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScipt : MonoBehaviour
{
    public float value;
     
    private float waterLevel = GenerateParams.WaterLevel;
    private float sandLevel = GenerateParams.SandLevel;
    private float landLevel = GenerateParams.LandLevel;

    void Start()
    {
        RandomColoring();
    }

    void Update()
    {
        
    }

    private void RandomColoring()
    {
        float clr = Random.value * 0.2f - 0.2f;
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (value < waterLevel)
        {
            float deep = value * 0.8f;
            spr.color = new Color(0.25f + deep, 0.45f + clr + deep, 0.87f + deep, 1);
        }
        else if (value < sandLevel)
        {
            spr.color = new Color(0.93f, 0.89f, 0.42f + clr, 1);
        }
        else if (value < landLevel)
        {
            float deep = value * 0.2f - 0.15f;
            spr.color = new Color(0.32f + deep, 0.84f + clr + deep, 0.34f + deep, 1);
        }
        else
        {
            float deep = value * 0.2f;
            spr.color = new Color(0.5f + Mathf.Abs(clr) - deep, 0.5f + Mathf.Abs(clr) - deep, 0.5f + Mathf.Abs(clr) - deep, 1);
        }
    }
}
