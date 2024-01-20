using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]
    private float Damage = 0;
    PlayerStats plSt;
    SpriteRenderer sr;
    BoxCollider2D box;

    void Start()
    {
        plSt = GetComponentInParent<PlayerStats>();
        sr = GetComponentInParent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (sr.flipX)
        {
            box.offset = new Vector2(-0.3f, 0);
        }
        else box.offset = new Vector2(0.3f, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Sheep")
        {
            Debug.Log("Weapon");
            AnimalStats anSt = collision.gameObject.GetComponent<AnimalStats>();
            anSt.HP -= plSt.Damage + Damage;
            if (anSt.HP <= 0)
            {
                collision.gameObject.GetComponent<AnimalActions>().AnimalDead();
            }
        }
    }
}
