using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D light2D;

    // Light decrease settings
    // private float lerpDuration = 3, currentLightRad = 7, endingLightRad = 3, valueToLerp;
    
    // Light Radius
    [SerializeField]private float currentRadiusIncreaser;
    
    public void Awake()
    { 
        float startLight = 5f;
        
        light2D = GetComponent<Light2D>();
        light2D.pointLightInnerRadius = startLight / 2;
        light2D.pointLightOuterRadius = startLight;
    }
    
    IEnumerator AutoRadiusDecreaser()
    {
        // print("started Coroutine");
        
        yield return new WaitForSeconds(2);

        // float timeElapsed = 0, checkValue = light2D.pointLightOuterRadius;;
        //
        // while (timeElapsed < lerpDuration)
        // {
        //     valueToLerp = Mathf.Lerp(currentLightRad, endingLightRad, timeElapsed / lerpDuration);
        //     timeElapsed += Time.deltaTime;
        //     
        //     DecreaseLightRadius();
        //     
        //     yield return null;
        // }
        
        InvokeRepeating("DecreaseLightRadius", 1f, 2f);

        yield return null;
        
        // print("Finished Coroutine");
    }

    public void IncreaseLightRadius()
    {
        if (currentRadiusIncreaser < 3f)
        {
            currentRadiusIncreaser++;
            light2D.pointLightInnerRadius += (currentRadiusIncreaser/2);
            light2D.pointLightOuterRadius += currentRadiusIncreaser;
        }
    }
    
    public void DecreaseLightRadius()
    {
        //print("decrease");
        
        if (currentRadiusIncreaser > 0f)
        {
            light2D.pointLightInnerRadius -= (currentRadiusIncreaser/2);
            light2D.pointLightOuterRadius -= currentRadiusIncreaser;
            currentRadiusIncreaser--;
        }
    }

    public void IncreaseCurrentRadius()
    {
        StopAllCoroutines();
        CancelInvoke("AutoRadiusDecreaser");
        
        IncreaseLightRadius();
        StartCoroutine(AutoRadiusDecreaser());
    }
}
