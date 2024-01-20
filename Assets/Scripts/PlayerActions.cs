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
    public float radiusLight = 1.5f;

    private PlayerStats plSt;
    private bool lanternOn;

    void Start()
    {
        MapObject = GetComponentInParent<GenerateAnimals>().Map;
        plSt = GetComponent<PlayerStats>();
        BoxCollider2D boxColl = gameObject.GetComponent<BoxCollider2D>();
        CircleCollider2D circleColl = gameObject.GetComponent<CircleCollider2D>();
        lanternOn = false;
        lantern.intensity = 2f;
        lantern.range = 5.5f;
        
    }

    void Update()
    {
        ActionsFromKey();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (true)
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
            if (collision.tag == "Chunk")
            {

            }
            if (collision.tag == "Cell")
            {
                Debug.Log($"cell {collision.transform.position}");
            }
        }

    }



    private void ActionsFromKey()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchLight();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            /// минус дерево должно быть
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
}
