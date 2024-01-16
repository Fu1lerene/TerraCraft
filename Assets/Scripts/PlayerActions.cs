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
    void Start()
    {
        lanternOn = false;
        lantern.intensity = 2.5f;
        lantern.range = 5.5f;
        pl = player.GetComponent<PlayerMovement>().pl;
        activeMap = MapObject.GetComponent<GenerateMap>().activeMap;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchLight();
        StaminaSlider.value = pl.Stamina.Value;
        HPSlider.value = pl.HP;
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
}
