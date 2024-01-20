using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnimalActions : MonoBehaviour
{
    public Slider SliderHP;

    private AnimalStats anSt;
    private AnimalMovement anMov;
    private GenerateAnimals genAn;

    // Start is called before the first frame update
    void Start()
    {
        anSt = GetComponent<AnimalStats>();
        anMov = GetComponent<AnimalMovement>();
        genAn = GetComponentInParent<GenerateAnimals>();

        SliderHP = GetComponentInChildren<Slider>();
        SliderHP.value = anSt.HP; // отображение здоровья овцы
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anMov.movementType == AnimalMovement.MovementType.RunAway) // отображение интерфейса овцы
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        else gameObject.transform.GetChild(0).gameObject.SetActive(false);

        SliderHP.value = anSt.HP;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GenerateAnimals genAn = GetComponentInParent<GenerateAnimals>();
        if (IsPlayer(collision))
        {
            foreach (var sheep in genAn.sheeps)
            {
                if (sheep == gameObject)
                {
                    break;
                }
            }
        }
    }

    public void AnimalDead()
    {
        foreach (var sheep in genAn.sheeps)
        {
            if (sheep == gameObject)
            {
                Destroy(sheep);
                genAn.sheeps.Remove(sheep);
                break;
            }
        }
    }
    private bool IsPlayer(Collider2D collision)
    {
        return collision.tag == "Player" && collision.GetType() == typeof(BoxCollider2D);
    }
}
