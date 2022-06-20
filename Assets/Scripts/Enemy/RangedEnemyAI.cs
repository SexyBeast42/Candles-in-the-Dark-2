using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedEnemyAI : MonoBehaviour
{
    // Get the other enemy scripts
    private EnemyMovement Movement;
    private EnemyLightController lc;

    // Patrol
    public GameObject[] patrolPoints;
    private bool isPatrolling;
    
    // Vision
    [SerializeField] private float _enemyViewRadius;
    public LayerMask targetMask, obstacleMask;
    public Transform player;
    
    // Attack
    public Transform firePoint;
    public float enemyAttackRadius;
    private bool _canAttack;
    private EnemyGun gun;
    
    // State
    private enum State
    {
        patrolling,
        attacking,
    }

    private State state;
    
    private void Awake()
    {
        if (GetComponent<EnemyMovement>() != null)
        {
            Movement = GetComponent<EnemyMovement>();
        }

        if (GetComponentInChildren<EnemyLightController>() != null)
        {
            lc = GetComponentInChildren<EnemyLightController>();
        }

        if (GetComponentInChildren<EnemyGun>() != null)
        {
            gun = GetComponentInChildren<EnemyGun>();
        }
        
        // Set AI stats
        _enemyViewRadius = 10f;
        enemyAttackRadius = 1f;
        isPatrolling = false;
        _canAttack = true;

        // Set AI state
        state = State.patrolling;
    }
    
    // Enemy State
    private void FixedUpdate()
    {
        print(state);
        
        switch (state)
        {
            case State.patrolling:
                PlayerDetection();

                if (!isPatrolling)
                {
                    StartCoroutine(Patrol());
                }
                break;
            
            case State.attacking:
                if (_canAttack)
                {
                    // Stop movin
                    Movement.StopMoving();
                    
                    Attack();
                }
                
                InAttackRange();
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
                state = State.attacking;
            }
        }
    }

    // To see how big the radius of the enemy's detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _enemyViewRadius);
    }

    // Enemy's AttackRange
    private void InAttackRange()
    {
        float stopChasing = 12f;
        if (Vector3.Distance(transform.position, player.position) > stopChasing)
        {
            state = State.patrolling;
        }
    }
    
    // Enemy's Attack
    private void Attack()
    {
        // Temporary attack without anims
        // Rotate FirePoint towards the player
        // Shoot gun
        // light up
        // Cooldown true

        // Rotato
        // firePoint.RotateAround(transform.position, Vector3.forward, Time.deltaTime);
        
        // Light up
        lc.FlashLight();
        
        // Shoot
        gun.Shoot();

        // cooldown
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        _canAttack = false;

        yield return new WaitForSeconds(3);

        _canAttack = true;
    }
}
