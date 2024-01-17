using Assets.Classes;
using Assets.Classes.Player;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
    public GameObject MapObject;
    public Light lantern;

    public float intensityLight = 0.0f;
    public float radiusLight = 3.0f;

    private PlayerStats plSt;
    private bool lanternOn;
    private List<Chunk> activeMap;
    private Chunk currentChunk;
    private Vector2 currentPos;
    public Cell currentCell;
    void Start()
    {
        MapObject = GetComponentInParent<GenerateAnimals>().Map;
        plSt = GetComponent<PlayerStats>();
        lanternOn = false;
        lantern.intensity = 2.5f;
        lantern.range = 5.5f;
        
    }

    void Update()
    {

        activeMap = MapObject.GetComponent<GenerateMap>().activeMap;

        currentCell = GetThisCell();
        ActionsFromKey();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Sheep")
        {
            AnimalStats anSt = collision.gameObject.GetComponent<AnimalStats>();
            plSt.Hp -= anSt.Damage;
            if (plSt.Hp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        
    }


    private Cell GetThisCell()
    {
        currentPos = MapObject.GetComponent<GenerateMap>().posCurrentChunk;
        foreach (Chunk chunk in activeMap)
        {
            if (chunk.X == currentPos.x && chunk.Y == currentPos.y)
            {
                currentChunk = chunk;
                foreach (Cell cell in currentChunk.Cells)
                {
                    float x = Mathf.Floor(transform.position.x + 0.5f);
                    float y = Mathf.Floor(transform.position.y + 0.5f);
                    if (cell.X == x && cell.Y == y)
                    {
                        return cell;
                    }
                }
                break;
            }
        }
        return null;
    }

    private void ActionsFromKey()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchLight();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyVegetation();
        }
    }
    private void SwitchLight()
    {
        lanternOn = !lanternOn;
        if (lanternOn)
        {
            lantern.intensity += intensityLight;
            lantern.range += radiusLight;
        }
        else
        {
            lantern.intensity -= intensityLight;
            lantern.range -= radiusLight;
        }
    }
    private void DestroyVegetation()
    {
        if (currentCell.Vegetation != null)
        {
            currentCell.Vegetation = null;
            Destroy(currentCell.VegetationObject);
        }
    }
}
