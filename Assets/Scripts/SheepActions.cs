using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepActions : MonoBehaviour
{
    public GameObject SheepObj;

    private List<GameObject> sheeps;
    private AnimalStats anSt;
    // Start is called before the first frame update
    void Start()
    {
        anSt = GetComponent<AnimalStats>();
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
                if (sheep == SheepObj)
                {
                    PlayerStats plSt = collision.gameObject.GetComponent<PlayerStats>();
                    anSt.HP -= plSt.Damage;
                    if (anSt.HP <= 0)
                    {
                        Destroy(sheep);
                        sheeps.Remove(sheep);
                    }
                    break;
                }
            }
        }
        genAn.sheeps = sheeps;
    }
}
