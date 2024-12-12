using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifetime = 5f;

    private void Start()
    {
        Rigidbody bullet = GetComponent<Rigidbody>();
        bullet.linearVelocity = transform.forward * bulletSpeed;
        Destroy(gameObject, bulletLifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.GetComponent<BulletTarget>() != null)
        //{
        //    Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        //}
        //else
        //{
        //    Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        //}
        Destroy(gameObject);
    }
}
