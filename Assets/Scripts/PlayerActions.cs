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
    public GameObject player;
    public GameObject MapObject;
    public Light lantern;
    public Slider StaminaSlider;
    public Slider HPSlider;
    public GameObject entities;

    private bool lanternOn;
    public float intensityLight = 0.0f;
    public float radiusLight = 3.0f;

    private Player pl;
    private List<Chunk> activeMap;
    private Chunk currentChunk;
    private Vector2 currentPos;
    public Cell currentCell;
    void Start()
    {
        lanternOn = false;
        lantern.intensity = 2.5f;
        lantern.range = 5.5f;
        pl = entities.GetComponent<CreatePlayer>().pl;
        
    }

    // Update is called once per frame
    void Update()
    {
        StaminaSlider.value = pl.Stamina.Value;
        HPSlider.value = pl.HP;
        activeMap = MapObject.GetComponent<GenerateMap>().activeMap;

        //Debug.Log(activeMap.Count);
        //Debug.Log(activeMap[0].X);
        currentCell = GetThisCell();
        ActionsFromKey();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pl.HP -= 10;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private Cell GetThisCell()
    {
        currentPos = MapObject.GetComponent<GenerateMap>().PosCurrentChunk;
        foreach (Chunk chunk in activeMap)
        {
            if (chunk.X == currentPos.x && chunk.Y == currentPos.y)
            {
                currentChunk = chunk;
                foreach (Cell cell in currentChunk.Cells)
                {
                    float x = Mathf.Floor(player.transform.position.x + 0.5f);
                    float y = Mathf.Floor(player.transform.position.y + 0.5f);
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
