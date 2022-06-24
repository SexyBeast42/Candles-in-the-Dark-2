using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //FieldOfViewCall
    [SerializeField] private FieldOfView fieldOfView;
    
    //Player light
    private PlayerLightController lc;
    
    //Get player's RigidBody
    private Rigidbody2D rb;
    
    //Player movements
    private Vector3 moveDir, lastMoveDir, rollDir;
    private float rollCD = 3;
    public const float moveSpeed = 20f;
    public float rollSpeed;
    private bool isInvunerable;
    private PlayerRoll rollUI;
    
    //Player attack
    public float playerDamage = 1f, playerRangeX, playerRangeY;
    public LayerMask enemyLayers;
    public Transform attackPos;
    
    //Player health
    public Health health;

    //Player rotation
    public Camera cam;
    private Vector2 mousePos;
    
    //Events
    public UnityEvent EnemyHit;

    //Animator
    public Animator animator;

    //To determine whether the player is allowed to do something or not
    private enum PlayerAction
    {
        Normal,
        Rolling,
        Attacking,
    }

    private PlayerAction playerAction;

    private void Awake()
    {        
        rb = GetComponent<Rigidbody2D>();
        lc = GetComponentInChildren<PlayerLightController>();
        health = GetComponent<Health>();
        rollUI = GetComponent<PlayerRoll>();
        
        // healthBar = GetComponentInChildren<PlayerHealthbar>();
        // healthBar.SetMaxHealth(hitPoints);
        
        playerAction = PlayerAction.Normal;

        EnemyHit = new UnityEvent();
        EnemyHit.AddListener(lc.IncreaseCurrentRadius);
    }

    void Update()
    {
        switch (playerAction)
        {
            case PlayerAction.Normal:
                HandlePlayerMovement();
                HandlePlayerAttack();
                HandlePlayerInteraction();
                
                //For player rotation
                mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                break;
            
            case PlayerAction.Rolling:
                HandlePlayerMovement();
                break;
            
            case PlayerAction.Attacking:
                HandlePlayerAttack();
                break;
        }
    }

    private void FixedUpdate()
    {
        //Movement of the player
        switch (playerAction)
        {
            case PlayerAction.Normal:
                rb.velocity = moveDir * moveSpeed;
                HandlePlayerRotation();
                break;
            
            case PlayerAction.Rolling:
                rb.velocity = rollDir * rollSpeed;
                break;
            
            case PlayerAction.Attacking:
                break;
        }
    }

    private void HandlePlayerAttack()
    {
        switch (playerAction)
        {
            case PlayerAction.Normal:
                if (Input.GetMouseButtonDown(0))
                {
                    animator.SetTrigger("attack");
                    playerAction = PlayerAction.Attacking;
                }
                break;
            
            case PlayerAction.Rolling:
                break;
            
            case PlayerAction.Attacking:
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, 
                    new Vector2(playerRangeX, playerRangeY), 0, enemyLayers);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //enemiesToDamage[i].GetComponent<EnemyAI>().TakeDamage(playerDamage, transform.position);
                    if (enemiesToDamage[i].GetComponent<EnemyHealth>() != null)
                    {
                        print("attaking");
                        enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage();
                    }
                }

                if (enemiesToDamage.Length != 0)
                {
                    // print("In EnemyHit.Invoke()");
                    EnemyHit.Invoke();
                }
                
                playerAction = PlayerAction.Normal;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector2(playerRangeX, playerRangeY));
    }

    //Function to handle how the player moves
    private void HandlePlayerMovement()
    {
        switch (playerAction)
        {
            //Handling player walking
            case PlayerAction.Normal:
                float moveX = 0f;
                float moveY = 0f;

                if (Input.GetKey(KeyCode.W))
                {
                    moveY = +1f;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    moveY = -1f;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    moveX = -1f;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    moveX = +1f;
                }

                moveDir = new Vector3(moveX, moveY).normalized;
                animator.SetFloat("Vertical", moveY);
                animator.SetFloat("Horizontal", moveX);
                animator.SetFloat("Speed", moveDir.sqrMagnitude);


                if (moveX != 0 || moveY != 0)
                {
                    lastMoveDir = moveDir;
                }

                //Player roll
                if (rollCD > 0 && Input.GetKeyDown(KeyCode.Space))
                {
                    rollDir = lastMoveDir;
                    rollSpeed = 75f;
                    
                    playerAction = PlayerAction.Rolling;
                    
                    StartCoroutine(RollCooldown());
                } 
                
                break;
            
            //Handling player rolling

            case PlayerAction.Rolling:
                //print(playerAction);
                // GetComponent<BoxCollider2D>().enabled = false; //Player invunerable while rolling, but may break collisions
                isInvunerable = true;
                
                float rollSpeedDropMultiplier = 5f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 20f;

                if (rollSpeed < rollSpeedMinimum)
                {
                    // GetComponent<BoxCollider2D>().enabled = true;
                    isInvunerable = false;
                    playerAction = PlayerAction.Normal;
                }
                break;
            
            case PlayerAction.Attacking:
                break;
        }
    }

    private void HandlePlayerRotation()
    {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
    
    //Cooldown for the player's roll
    IEnumerator RollCooldown()
    {
        rollUI.RemoveRoll();
        rollCD--;
        
        yield return new WaitForSeconds(5f);

        rollUI.AddRoll();
        rollCD++;
    }

    private void HandlePlayerInteraction()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("interaction");
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (!isInvunerable)
        {
            health.TakeDamage(damage);
        }
    }


    //Get the position of the mouse in the world
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
