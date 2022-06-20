using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI2 : MonoBehaviour
{
    // Enemy Movement
    public float moveSpeed, stoppingDistance, nearDistance, startTimeBtwShots;
    private float timeBtwShots;

    // Other Enemy things
    public GameObject arrow;
    private EnemyLightController lc;
    private Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (GetComponentInChildren<EnemyLightController>() != null)
        {
            lc = GetComponentInChildren<EnemyLightController>();
        }
    }


    void FixedUpdate()
    { 
        // Tells the enemy to move away from the player
        if (Vector2.Distance(transform.position, player.position) < nearDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
        } 
        
        // Tells the enemy to move towards the player
        else if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        
        // Tells the enemy to stop moving once they're in range
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > nearDistance)
        {
            transform.position = this.transform.position;
        }

        // Shot Cooldown
        if (timeBtwShots <= 0)
        {
            // Flashes Light
            lc.FlashLight();
            
            //Spawn an arrow
            StartCoroutine(Shoot());
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
    
    IEnumerator Shoot()
    {
        timeBtwShots = startTimeBtwShots;
        
        yield return new WaitForSeconds(1);
        
        Instantiate(arrow, transform.position, Quaternion.identity);
    }
}
