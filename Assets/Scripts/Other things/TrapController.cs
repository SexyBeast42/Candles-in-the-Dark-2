using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class TrapController : MonoBehaviour
{
    public float dmg = 1;
    
    public Animator anim;

    private bool canDmg;

    private void Start()
    {
        canDmg = true;
        
        if (GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (canDmg && col.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Activated", true);
            
            col.GetComponent<PlayerController>().TakeDamage(dmg);

            StartCoroutine(TrapCooldown());
        }
    }

    IEnumerator TrapCooldown()
    {
        canDmg = false;
        
        yield return new WaitForSeconds(2);

        anim.SetBool("Activated", false);
        anim.SetBool("Cooldown", true);

        yield return new WaitForSeconds(1);

        anim.SetBool("Cooldown", false);
        canDmg = true;
    }
}
