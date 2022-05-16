using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D light2D;

    private bool reachUpperLightLimit = false, reachLowerLightLimit = false;

    public void Awake()
    { 
        float startLight = 3f;
        
        light2D = GetComponent<Light2D>();
        light2D.pointLightInnerRadius = startLight / 2;
        light2D.pointLightOuterRadius = 3f;
    }

    private void FixedUpdate()
    {
        // Makes sure that the radius stays in the limit
        float checkValue = light2D.pointLightOuterRadius;
        RadiusLimitChecker(checkValue);
    }

    private void RadiusLimitChecker(float checkValue)
    {
        float topLimit = 7f, lowerLimit = 3f;

        if (checkValue >= topLimit)
        {
            reachUpperLightLimit = true;
        }

        if (checkValue <= lowerLimit)
        {
            reachLowerLightLimit = true;
        }

        if (checkValue != topLimit && checkValue != lowerLimit)
        {
            reachLowerLightLimit = false;
            reachUpperLightLimit = false;
        }
    }

    public void IncreaseLightRadius(float radius)
    {
        if (!reachUpperLightLimit)
        {
            light2D.pointLightInnerRadius=+ (radius/2);
            light2D.pointLightOuterRadius=+ radius;
        }
    }
    
    public void DecreaseLightRadius(float radius)
    {
        if (!reachLowerLightLimit)
        {
            light2D.pointLightInnerRadius=- (radius/2);
            light2D.pointLightOuterRadius=- radius;
        }
    }
}
