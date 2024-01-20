using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalActions : MonoBehaviour
{
    public GameObject SheepObj;
    public Slider SliderHP;

    private List<GameObject> sheeps;
    private AnimalStats anSt;
    private AnimalMovement anMov;
    // Start is called before the first frame update
    void Start()
    {
        anSt = GetComponent<AnimalStats>();
        anMov = GetComponent<AnimalMovement>();

        SliderHP = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Slider>();
        SliderHP.value = anSt.HP; // отображение здоровья овцы
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anMov.movementType == "run away") // отображение интерфейса овцы
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        else gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GenerateAnimals genAn = GetComponentInParent<GenerateAnimals>();
        if (collision.tag == "Player")
        {
            foreach (var sheep in genAn.sheeps)
            {
                if (sheep == SheepObj)
                {
                    PlayerStats plSt = collision.gameObject.GetComponent<PlayerStats>();
                    
                    anSt.HP -= plSt.Damage;
                    Debug.Log(anSt.HP);
                    SliderHP.value = anSt.HP;
                    Debug.Log(SliderHP.value);
                    if (anSt.HP <= 0)
                    {
                        Destroy(sheep);
                        genAn.sheeps.Remove(sheep);
                    }
                    break;
                }
            }
        }
    }
}
