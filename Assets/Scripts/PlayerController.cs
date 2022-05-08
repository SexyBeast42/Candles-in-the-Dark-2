using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class PlayerController : MonoBehaviour
{
    //FieldOfViewCall
    [SerializeField] private FieldOfView fieldOfView;
    private LightController lc;
    
    //Get player's RigidBody
    private Rigidbody2D rb;
    
    //Player movements
    private Vector3 moveDir, lastMoveDir, rollDir;
    private float rollCD = 3;
    public const float moveSpeed = 20f;
    public float rollSpeed;
    
    //Player attack
    public float playerDamage = 1f, playerRangeX, playerRangeY, increaseAmount = 3f;
    public LayerMask enemyLayers;
    public Transform attackPos;
    
    //Player health
    private float hitPoints = 5f;

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
        lc = GetComponentInChildren<LightController>();
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
        // Increase amount doesnt go over
        
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
                
                //StopAllCoroutines();
                
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyAI>().TakeDamage(playerDamage);

                    lc.IncreaseLightRadius(4f + increaseAmount);
                    increaseAmount++;
                }
                
                //StartCoroutine(DecreaseLightRange(increaseAmount));
                playerAction = PlayerAction.Normal;
                break;
        }
    }

    IEnumerator DecreaseLightRange(float amount)
    {
        increaseAmount--;
        yield return new WaitForSeconds(2);
        lc.DecreaseLightRadius(amount);
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

            case PlayerAction.Rolling:
                //print(playerAction);
                GetComponent<BoxCollider2D>().enabled = false; //Player invunerable while rolling, but may break collisions
                
                float rollSpeedDropMultiplier = 5f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 20f;

                if (rollSpeed < rollSpeedMinimum)
                {
                    GetComponent<BoxCollider2D>().enabled = true;
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
    
    public void TakeDamage(float damage)
    {
        // Debug.Log(gameObject.name + " got damaged.");
        hitPoints -= damage;

        if(hitPoints <= 0){
            // Debug.Log(gameObject.name + " dieded.");
            Destroy(gameObject);
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
