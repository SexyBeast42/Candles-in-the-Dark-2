using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectable : MonoBehaviour
{
    [SerializeField] private float healthValue;
    public GameObject E;
    bool playerInCollider = false;
    Collider2D playerCollider = null;


    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Player")
        {
            E.SetActive(true);
            playerInCollider = true;
            playerCollider = collision;
        }
    }  

    void Update()
    {
        if (playerInCollider && Input.GetKeyDown(KeyCode.E)) {   
            playerCollider.GetComponent<Health>().AddHealth(healthValue);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player"){   
            E.SetActive(false);
            playerInCollider = false;
            playerCollider = null;
        }
    }
}



