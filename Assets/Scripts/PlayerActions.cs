using Assets.Classes;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject player;
    public GameObject MapObject;
    public Light lantern;
    public bool lanternOn;

    private List<Chunk> activeMap;
    private Chunk currentChunk;
    void Start()
    {
        lanternOn = false;
        //activeMap = MapObject.GetComponent<GenerateMap>().;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lanternOn = !lanternOn;
            lantern.intensity = Convert.ToSingle(lanternOn);
        }

    }
}
