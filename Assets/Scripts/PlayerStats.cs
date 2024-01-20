using Assets.Classes.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float Hp = 100;
    public float Speed = 2;
    public float Damage = 10;
    public Stamina Stamina;

    private float maxValStam = 100;
    private float fallRateStam = 10;
    private float regenRateStam = 8;
    void Start()
    {
        Stamina = new Stamina(maxValStam, fallRateStam, regenRateStam);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
