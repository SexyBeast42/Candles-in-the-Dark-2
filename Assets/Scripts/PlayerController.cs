using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //FieldOfViewCall
    [SerializeField] private FieldOfView fieldOfView;
    
    //Get player's RigidBody
    private Rigidbody2D rb;
    
    //Player movements
    private Vector3 moveDir;
    private Vector3 lastMoveDir;
    private Vector3 rollDir;
    private float rollCD = 3;
    public const float moveSpeed = 20f;
    public float rollSpeed;
    
    //Player attack
    public float playerDamage = 1f;
    public float playerRangeX;
    public float playerRangeY;
    public LayerMask enemyLayers;
    public Transform attackPos;

    //Player rotation
    public Camera cam;
    private Vector2 mousePos;

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
        playerAction = PlayerAction.Normal;
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
 
        fieldOfView.SetOrigin(transform.position);
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
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(playerDamage);
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
                if (moveX != 0 || moveY != 0)
                {
                    lastMoveDir = moveDir;
                }

                //Player roll
                if (rollCD > 0 && Input.GetKeyDown(KeyCode.Space))
                {
                    rollDir = lastMoveDir;
                    rollSpeed = 250f;
                    
                    playerAction = PlayerAction.Rolling;
                    
                    StartCoroutine(RollCooldown());
                } 
                
                break;
            
            //Handling player rolling
            
            // Player invunerable when rolling
            
            case PlayerAction.Rolling:
                //print(playerAction);
                float rollSpeedDropMultiplier = 5f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 20f;

                if (rollSpeed < rollSpeedMinimum)
                {
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
        rollCD--;
        yield return new WaitForSeconds(5f);
        rollCD++;
    }

    private void HandlePlayerInteraction()
    {
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("interaction");
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
