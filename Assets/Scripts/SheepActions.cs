using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Classes.Animals;

public class SheepActions : MonoBehaviour
{
    public GameObject SheepObj;

    private List<Animal> sheeps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GenerateAnimals genAn = GetComponentInParent<GenerateAnimals>();
        sheeps = genAn.sheeps;
        if (collision.tag == "Player")
        {
            foreach (var sheep in sheeps)
            {
                if (sheep.AnimalObj == SheepObj)
                {
                    PlayerStats plSt = collision.gameObject.GetComponent<PlayerStats>();
                    sheep.HP -= plSt.Damage;
                    if (sheep.HP <= 0)
                    {
                        Destroy(sheep.AnimalObj);
                        sheeps.Remove(sheep);
                    }
                    break;
                }
            }
        }
        genAn.sheeps = sheeps;
    }
}
