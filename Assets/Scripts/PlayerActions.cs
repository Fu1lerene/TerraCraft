using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject player;
    public Light lantern;
    public bool lanternOn;
    void Start()
    {
        lanternOn = false;
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
