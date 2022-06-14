using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    // Needs the collider of the arrow to be a trigger
    // Responsible for breaking when hitting walls, and damaging players

    private float moveSpeed = 20f, bulletDMG = 1f;

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Hit Player");
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(bulletDMG);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
