using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D light2D;

    public bool reachLightLimit = false;
    
    public void Awake()
    { 
        float startLight = 3f;
        
        light2D = GetComponent<Light2D>();
        light2D.pointLightInnerRadius = startLight / 2;
        light2D.pointLightOuterRadius = 3f;
    }

    public void IncreaseLightRadius(float radius)
    {
        float rad = radius;

        float radiusLimit = 7f;
        if (!reachLightLimit && rad >= radiusLimit)
        {
            rad = 6f;
            
            light2D.pointLightInnerRadius=+ (rad/2);
            light2D.pointLightOuterRadius=+ rad;
        }

        else
        {
            light2D.pointLightInnerRadius=+ (rad/2);
            light2D.pointLightOuterRadius=+ rad;
        }
    }
    
    public void DecreaseLightRadius(float radius)
    {
        float rad = radius;

        float radiusLimit = 3f;
        if (!reachLightLimit && rad >= radiusLimit)
        {
            rad = 6f;
            
            light2D.pointLightInnerRadius=- (rad/2);
            light2D.pointLightOuterRadius=- rad;
        }

        else
        {
            light2D.pointLightInnerRadius=- (rad/2);
            light2D.pointLightOuterRadius=- rad;
        }
    }
    //later use coroutine to decrease it
}
