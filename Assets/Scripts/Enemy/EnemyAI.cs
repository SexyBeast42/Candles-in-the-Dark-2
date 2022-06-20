using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    
    //Enemy's movements
    public float moveSpeed = 1.5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 roamPosition;
    
    //Enemy's Vision
    public LayerMask targetMask, obstacleMask;
    public float viewRadius;

    //Enemy's health
    public HealthBarEnemy Healthbar;
    public float Hitpoints;
    public GameObject floatingPoints;
    public float MaxHitpoints = 5f;
    
    //Enemy's attack
    [SerializeField] private float distToPlayer, enemyRangeX, enemyRangeY, enemyDamage = 1f;
    public Transform firePoint;
    private bool canAttack;

    //Enemy action
    private enum State
    {
        roaming,
        chasing,
        attacking,
    }
    
    private State state;

    void Awake()
    {
        Hitpoints = MaxHitpoints;
        // Healthbar.SetHealth(Hitpoints,MaxHitpoints);
        state = State.roaming;
        rb = GetComponent<Rigidbody2D>();
        roamPosition = GetRoamingPosition();
        canAttack = true;
    }

    void Update()
    {
        // print(name + " " + state);
        
        switch (state)
        {
            case State.roaming:
                SetTarget(roamPosition);
                float reachedPositionDistance = 1f;
                if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
                {
                    roamPosition = GetRoamingPosition();
                    SetTarget(roamPosition);
                }
                
                FindTarget();
                break;
            
            case State.chasing:
                SetTarget(player.position);

                // Debug.Log("chasing");
                if (Vector3.Distance(transform.position, player.position) < viewRadius)
                {
                    // print("move to attack state");
                    // player.transform.LookAt(player, Vector3.forward);
                    // float fireRate = 5f;
                    // StartCoroutine(Attacking(fireRate));
                    // Debug.Log("in range");                 
                    state = State.attacking;
                }

                float stopChaseDistance = 10f;
                if (Vector3.Distance(transform.position, player.position) > stopChaseDistance)
                {
                    state = State.roaming;
                }
                break;
            case State.attacking:
                // if (canAttack)
                // {
                //     SetTarget(player.position);
                //
                //     stopChaseDistance = 2f;
                //     var currentPos = transform.position;
                //     if (Vector3.Distance(transform.position, player.position) > stopChaseDistance)
                //     {
                //         SetTarget(currentPos);
                //         AttackTarget();
                //     }
                // }
                // else
                // {
                //     state = State.chasing;
                // }
                // StartCoroutine(Attacking());
                if (canAttack)
                {
                    AttackTarget();
                }
                else
                {
                    state = State.chasing;
                }
                break;
        }
    }
    
    private void FixedUpdate()
    {
        // EnemyDetection();
        
        MoveCharacter(movement);
    }
    
    void MoveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
    
    private void SetTarget(Vector3 distance)
    {        
        Vector3 direction = distance - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        rb.rotation = angle * Time.deltaTime;
        movement = direction.normalized;
    }
    
    private Vector3 GetRoamingPosition()
    {
        return transform.position + new Vector3(
            Random.Range(-1f,1f), 
            Random.Range(-1f,1f)).normalized * 
            Random.Range(10f, 10f);
    }

    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, player.position) < viewRadius)
        {
            state = State.chasing;
        }
    }

    private void AttackTarget()
    {
        // Transform target = player.transform;
        //
        // print("attacking");
        //
        // Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(target.position, 
        //     new Vector2(enemyRangeX, enemyRangeY), 0, targetMask);
        //         
        // for (int i = 0; i < enemiesToDamage.Length; i++)
        // {
        //     enemiesToDamage[i].GetComponent<PlayerController>().TakeDamage(enemyDamage);
        //     StartCoroutine(Cooldown());
        // }
        
        
        //     var currentPos = transform.position;
        //     if (Vector3.Distance(transform.position, player.position) > stopChaseDistance)
        
        float stopChaseDistance = 2f;
        
        SetTarget(Vector3.zero);

        GameObject[] target = GameObject.FindGameObjectsWithTag("Player");
        
        target[0].GetComponent<PlayerController>().TakeDamage(enemyDamage);

        StartCoroutine(Cooldown());
        
        state = State.chasing;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(firePoint.position, new Vector2(enemyRangeX, enemyRangeY));
    }

    IEnumerator Cooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(2);
        canAttack = true;
    }

    IEnumerator Attacking()
    {
        canAttack = false;
        
        yield return new WaitForSeconds(2f);
        print("Coroutine finished");
        canAttack = true;
    }

    // public void EnemyDetection()
    // {
    //     print("work");
    //     Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
    //
    //     for (int i = 0; i < targetsInViewRadius.Length; i++)
    //     {
    //         Transform target = targetsInViewRadius[i].transform;
    //         Vector3 dirToTarget = (target.position - transform.position).normalized;
    //         float distToTarget = Vector3.Distance(transform.position, target.position);
    //         Debug.DrawRay(transform.position, dirToTarget * 1000, Color.red);
    //
    //         Debug.Log("Player is in front?");
    //         if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
    //         {
    //             Debug.Log("Can see player"); 
    //         }
    //     }
    // }
    //
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.white;
    //     Gizmos.DrawWireSphere(transform.position, viewRadius);
    // }
    
    public void TakeDamage(float damage, Vector2 target)
    {
        // Debug.Log(gameObject.name + " got damaged.");
        Hitpoints -= damage;
        // Healthbar.SetHealth(Hitpoints, MaxHitpoints);

        rb.AddForce(target, ForceMode2D.Impulse);
        StartCoroutine(DazedTime());
            
        
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

}