using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //FieldOfViewCall
    [SerializeField] private FieldOfView fieldOfView;
    //Get player's RigidBody
    private Rigidbody2D rb;
    
    //Player movements
    private Vector3 moveDir;
    private Vector3 lastMoveDir;
    private Vector3 rollDir;
    public const float moveSpeed = 20f;
    public float rollSpeed;
    
    //Player attack
    

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
        Enemy enemy = GetComponent<Enemy>();
    }
    
    void Update()
    {
        switch (playerAction)
        {
            case PlayerAction.Normal:
                HandlePlayerMovement();
                HandlePlayerAttack();
                break;
            
            case PlayerAction.Rolling:
                break;
            
            case PlayerAction.Attacking:
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 mouseDir = (mousePosition - transform.position).normalized; //mouseDir to be used with anims
                    
            //When you have anims, you can make it so that the player stops moving when they are attacking
            // playerAction = PlayerAction.Attacking;
            // moveDir = Vector3.zero;
            // playerAction = PlayerAction.Normal;
            //Here you would add anims when we have, need state machine to make the anims work proper dapper like
        }
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


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rollDir = lastMoveDir;
                    rollSpeed = 250f;

                    playerAction = PlayerAction.Rolling;
                } 
                
                break;
            
            //Handling player rolling
            case PlayerAction.Rolling:
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
