using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    
    private float _moveSpeed = 3f;
    private Vector3 _movement;
    private bool _canMove;

    void Awake()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        _canMove = true;
    }

    void FixedUpdate()
    {
        if (_canMove)
        {
            MoveTo(_movement);
        }
    }

    private void MoveTo(Vector3 destination)
    {
        rb.MovePosition(transform.position + destination * _moveSpeed * Time.deltaTime);
    }

    public void SetDestination(Vector3 destination)
    {
        _canMove = true;

        Vector3 direction = (destination - transform.position).normalized;

        _movement = direction;
    }

    public void StopMoving()
    {
        _canMove = false;
    }
}
