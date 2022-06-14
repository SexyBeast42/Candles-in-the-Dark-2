using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLightController : MonoBehaviour
{
    private Light2D light2D;
    
    private void Awake()
    { 
        float startLight = 6f;
        
        light2D = GetComponent<Light2D>();
        light2D.pointLightOuterRadius = startLight;
        light2D.pointLightInnerRadius = startLight / 2;
    }
    
    // Light decrease settings
    private float lerpDuration = 4f, endingLightRad = 6f;

    IEnumerator DecreaseRadius()
    {
        // print("Starting coroutine");
        
        yield return new WaitForSeconds(2f);

        float timeElapsed = 0f;
        float temporaryRadius = light2D.pointLightOuterRadius;

        while (timeElapsed < lerpDuration)
        {
            // print("Lerping Value: " + light2D.pointLightOuterRadius);
            
            light2D.pointLightOuterRadius = Mathf.Lerp(temporaryRadius, endingLightRad, timeElapsed / lerpDuration);
            light2D.pointLightInnerRadius = Mathf.Lerp(temporaryRadius / 2, endingLightRad / 2, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;

            yield return new WaitForSeconds(0.01f);
        }
        
        yield return null;
    }

    private void IncreaseLightRadius()
    {
        if (light2D.pointLightOuterRadius < 10f)
        {
            light2D.pointLightOuterRadius++;
            light2D.pointLightInnerRadius = light2D.pointLightOuterRadius / 2;
        }
        
        StartCoroutine(DecreaseRadius());
    }

    public void IncreaseCurrentRadius()
    {
        StopAllCoroutines();
        IncreaseLightRadius();
    }
}
