using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Slider StaminaSlider;
    public Slider HPSlider;
    public GameObject Entities;

    private PlayerStats plSt;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        plSt = Entities.GetComponentInChildren<PlayerStats>();
        StaminaSlider.value = plSt.Stamina.Value;
        HPSlider.value = plSt.Hp;
    }
}
