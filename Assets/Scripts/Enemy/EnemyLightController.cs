using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyLightController : MonoBehaviour
{ 
    private Light2D light2D;

    private void Awake()
    {
        if (gameObject.GetComponent<Light2D>() != null)
        {
            light2D = GetComponent<Light2D>();
        }

        light2D.enabled = false;
    }

    public void FlashLight()
    {
        StartCoroutine(Flash());
    }
    
    IEnumerator Flash()
    {
        print("Flashing Light");
        light2D.enabled = true;

        yield return new WaitForSeconds(2);

        light2D.enabled = false;
    }
}
