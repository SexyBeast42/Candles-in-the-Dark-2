using System;
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
 
    
    //Enemy's Vision
    public Transform castPoint;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;

    //Enemy's health
    public HealthBarEnemy Healthbar;
    public float Hitpoints;
    public GameObject floatingPoints;
    public float MaxHitpoints = 5f;
    

    //Enemy's attack
    private float distToPlayer;
    public Transform FirePoint;
    
    //Enemy action
    private enum State
    {
        idle,
        attacking,
    }
    
    private State state;

    void Awake()
    {
        Hitpoints = MaxHitpoints;
        Healthbar.SetHealth(Hitpoints,MaxHitpoints);
        rb = GetComponent<Rigidbody2D>();
       
    }

    void Update()
    {
        // FindTarget();
        
        // distToPlayer = 2f;

        // MoveCharacter(EnemyDetection());
        // Melee
        // while enemy not in range, move towards player in a certain distance
        // if reached desired pos, attack player
    }
    
    private void FixedUpdate()
    {
        EnemyDetection();
        MoveCharacter(movement);
    }

    public void TakeDamage(float damage)
    {
        // Debug.Log(gameObject.name + " got damaged.");
        Hitpoints -= damage;
        Healthbar.SetHealth(Hitpoints, MaxHitpoints);

        StartCoroutine(DazedTime());
    Instantiate(floatingPoints,transform.position, Quaternion.identity);    
        if(Hitpoints <= 0){
            // Debug.Log(gameObject.name + " dieded.");
            Destroy(gameObject);
        }
    }

    IEnumerator DazedTime()
    {
        moveSpeed = 0f;
        yield return new WaitForSeconds(.6f);
        moveSpeed = 1.5f;
    }

    void MoveCharacter(Vector2 direction)
    {
        rb.MovePosition ((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private void EnemyDetection()
    {
        print("work");
        bool canSeeTarget;
        Vector3 playerLocation;

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            Debug.Log("Can see player2");
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                Debug.Log("Can see player3");
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    Debug.Log("Can see player");
                }
            }
        }
        
        //return playerLocation;
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    /*private void MoveToTarget(float distance)
    {        
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        rb.rotation = angle;
        movement = direction.normalized;
    }*/
    
    
    /// Enemy has 8 raycast rays in all directions / or use navmeshes (either way - do later aft basics established)
    /// They are smart enough to hide behind walls
    /// Smart enough to not group up but to distance themselves from other enemies
    /// Enemy can drop health potions rarely
    /// 
    /// Enemy AI Cases
    /// They have a wander function
    /// When the player gets in range, they are in attack mode
    /// They move in closer to the player, but stop in a distance in 
}
