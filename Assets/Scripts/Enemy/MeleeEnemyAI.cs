using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeEnemyAI : MonoBehaviour
{
    // Get the enemy movement script
    private EnemyMovement Movement;

    // Patrol
    public GameObject[] patrolPoints;
    private bool isPatrolling;
    
    // Vision
    [SerializeField] private float _enemyViewRadius;
    public LayerMask targetMask, obstacleMask;
    
    // Chase
    public Transform player;
    
    // Attack
    public Transform firePoint;
    public float enemyAttackRadius;
    private bool _canAttack;
    
    // State
    private enum State
    {
        patrolling,
        chasing,
        attacking,
    }

    private State state;
    
    private void Awake()
    {
        if (GetComponent<EnemyMovement>() != null)
        {
            Movement = GetComponent<EnemyMovement>();
        }
        
        // Set AI stats
        _enemyViewRadius = 5f;
        enemyAttackRadius = 1f;
        isPatrolling = false;

        // Set AI state
        state = State.patrolling;
    }
    
    // Enemy State
    private void FixedUpdate()
    {
        switch (state)
        {
            case State.patrolling:
                PlayerDetection();
                if (!isPatrolling)
                {
                    StartCoroutine(Patrol());
                }
                break;
            
            case State.chasing:
                Chase();
                break;
            
            case State.attacking:
                Attack();
                break;
        }
    }

    
    // Enemy Patrol
    IEnumerator Patrol()
    {
        isPatrolling = true;
        
        foreach (GameObject point in patrolPoints)
        {
            Vector3 destination = point.transform.position;
            
            // print("going to " + point.name);
            Movement.SetDestination(destination);

            // if (Vector3.Distance(transform.position, destination) >= 1)
            // {
            //         print("Reached point: " + point.name);
            //         yield return new WaitForSeconds(2);
            // }
            
            yield return new WaitForSeconds(1);
        }

        isPatrolling = false;
    }

    
    // Enemy's Vision
    private void PlayerDetection()
    {
        Collider2D[] playerInView = Physics2D.OverlapCircleAll(transform.position, _enemyViewRadius, targetMask);

        for (int i = 0; i < playerInView.Length; i++)
        {
            Transform target = playerInView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            float distToTarget = Vector3.Distance(transform.position, target.position);

            if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
            {
                state = State.chasing;
            }
        }
    }

    // To see how big the radius of the enemy's detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _enemyViewRadius);
    }

    // Enemy's Chase
    private void Chase()
    {
        Movement.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) < _enemyViewRadius)
        {
            state = State.attacking;
        }

        float stopChasing = 7f;
        if (Vector3.Distance(transform.position, player.position) > stopChasing)
        {
            state = State.patrolling;
        }
    }
    
    // Enemy's Attack
    private void Attack()
    {
        // Temporary attack without anims
        // Check where the player is relative to the enemy.
        
    }

}
