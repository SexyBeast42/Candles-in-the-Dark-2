using Unity.Mathematics;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject bulletObj;

    public void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletObj, 
            transform.position, 
            quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().
            AddForce(
                -transform.forward.normalized * 
                bullet.GetComponent<ArrowBehaviour>().GetMoveSpeed(), 
                ForceMode2D.Impulse);
    }
}
