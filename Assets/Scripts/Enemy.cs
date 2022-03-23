using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    
    //Enemy's movements
    public float moveSpeed = 1.5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    
    //Enemy's health
    public HealthBarEnemy Healthbar;
    public float Hitpoints;
    public float MaxHitpoints = 5f;

    void Awake()
    {
        Hitpoints = MaxHitpoints;
        Healthbar.SetHealth(Hitpoints,MaxHitpoints);
        rb = GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        FindTarget();
    }
    
    private void FixedUpdate()
    {
        MoveCharacter(movement);
    }
    
    void MoveCharacter(Vector2 direction)
    {
        rb.MovePosition ((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " got damaged.");
        Hitpoints -= damage;
        Healthbar.SetHealth(Hitpoints, MaxHitpoints);

        StartCoroutine(DazedTime());
        
        if(Hitpoints <= 0){
            //Debug.Log(gameObject.name + " dieded.");
            Destroy(gameObject);
        }
    }

    IEnumerator DazedTime()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(.6f);
        moveSpeed = 1.5f;
    }

    private void FindTarget()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        direction.Normalize();
        movement = direction;
    }
    
    /// Enemy has 8 raycast rays in all directions
    /// They are smart enough to hide behind walls
    /// Smart enough to not group up but to distance themselves from other enemies
    /// Enemy can drop health potions rarely
    /// 
    /// Enemy AI Cases
    /// They have a wander function
    /// When the player gets in range, they are in attack mode
    /// They move in closer to the player, but stop in a distance in 
}
