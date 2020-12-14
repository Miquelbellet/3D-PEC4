using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightManager : MonoBehaviour
{
    public GameObject directional;
    public float totalSecondsTo16Hours;
    public float startDayRotationX;
    public float endDayRotationX;
    public float minutesLeftToActivate;

    private float elapsedTime;
    private Light[] sceneLights;
    private bool lightsOn;

    void Start()
    {
        sceneLights = FindObjectsOfType<Light>();
        lightsOn = true;
        TurnOff();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float rot = (endDayRotationX - startDayRotationX) / totalSecondsTo16Hours;
        directional.transform.Rotate(-rot * Time.deltaTime, 0, 0);
        var secondsLeft = totalSecondsTo16Hours - elapsedTime;
        if (secondsLeft <= minutesLeftToActivate) TurnOn();
        if (secondsLeft <= -minutesLeftToActivate) TurnOff();
    }

    private void TurnOn()
    {
        if (!lightsOn)
        {
            lightsOn = true;
            foreach (var light in sceneLights)
            {
                if (light.tag != "MainLight") light.gameObject.SetActive(true);
            }
        }
    }

    private void TurnOff()
    {
        if (lightsOn)
        {
            lightsOn = false;
            elapsedTime = 0;
            foreach (var light in sceneLights)
            {
                if (light.tag != "MainLight") light.gameObject.SetActive(false);
            }
        }
    }
}
