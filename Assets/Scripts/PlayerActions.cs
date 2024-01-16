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
        pl = player.GetComponent<PlayerMovement>().pl;
        
    }

    // Update is called once per frame
    void Update()
    {
        SwitchLight();
        StaminaSlider.value = pl.Stamina.Value;
        HPSlider.value = pl.HP;
        activeMap = MapObject.GetComponent<GenerateMap>().activeMap;

        //Debug.Log(activeMap.Count);
        //Debug.Log(activeMap[0].X);
        GetThisCell();
        ActionsFromKey();
    }

    private void GetThisCell()
    {
        currentPos = MapObject.GetComponent<GenerateMap>().currentChunk;
        foreach (Chunk chunk in activeMap)
        {
            if (chunk.X == currentPos.x && (chunk.Y == currentPos.y))
            {
                currentChunk = chunk;
                foreach (Cell cell in currentChunk.Cells)
                {
                    if (((int)cell.X == (int)(player.transform.position.x + 0.5f)) && ((int)cell.Y == (int)(player.transform.position.y + 0.5f)))
                    {
                        currentCell = cell;
                        break;
                    }
                }
                break;
            }
        }
    }

    private void SwitchLight()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
    }

    private void ActionsFromKey()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if (currentCell.Vegetation != null)
            {
                currentCell.Vegetation = null;
                Destroy(currentCell.VegetationObject);
            }
        }
    }
}
