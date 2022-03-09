using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{
public Transform player;
public float moveSpeed = 1.5f;
private Rigidbody2D rb;
private Vector2 movement;
public HealthBarEnemy Healthbar;
public float Hitpoints;
public float MaxHitpoints = 5;

    
    void Start()
    {
        Hitpoints = MaxHitpoints;
        Healthbar.SetHealth(Hitpoints,MaxHitpoints);
      rb = this.GetComponent<Rigidbody2D>();  
    }

    void Update()
    {
        Vector3 direction = player .position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        direction.Normalize();
        movement = direction;

    }
    private void FixedUpdate(){
        moveCharacter(movement);
    }
    void moveCharacter(Vector2 direction){
        rb.MovePosition ((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
    public void TakeHit(float damage){
        Hitpoints -= damage;
        Healthbar.SetHealth(Hitpoints,MaxHitpoints);
        if(Hitpoints <= 0){
            Destroy(gameObject);
        }
    }
}
