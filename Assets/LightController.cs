using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D light2D;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    public void IncreaseLightRadius(float radius)
    {
        float rad = radius;

        light2D.pointLightInnerRadius++;
        light2D.pointLightOuterRadius++;
    }
    
    //later use coroutine to decrease it
}
