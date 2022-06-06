using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectable : MonoBehaviour
{
[SerializeField] private float healthValue;
public GameObject E;


private void OnTriggerEnter2D(Collider2D collision){

    if (collision.tag == "Player")
    {
   E.SetActive(true); 
   collision.GetComponent<Health>().AddHealth(healthValue);
    }
 
}
     
}



