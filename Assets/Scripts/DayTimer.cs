using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    private float dayTimer;
    private float dayCycleLength = 150f;
    private float ml = 0.1f;
    private float dl = 0.5f;
    private float el = 0.6f;
    private float nl = 1f;
    private float t;
    private string dayPhase;

    public Light directLight;

    void Start()
    {
        t = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        dayTimer += Time.deltaTime;
        t = dayTimer % dayCycleLength / dayCycleLength;
        if (t < ml)
            dayPhase = "morning";
        else if (t < dl)
            dayPhase = "day";
        else if (t < el)
            dayPhase = "evening";
        else dayPhase = "night";

        switch (dayPhase)
        {
            case "morning":
                directLight.intensity = (1 / ml) * t;
                break;
            case "evening":
                directLight.intensity = (1 / (dl - el)) * (t - el);
                break;
        }

    }
}
