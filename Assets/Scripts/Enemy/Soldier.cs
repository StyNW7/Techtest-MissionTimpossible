using UnityEngine;
using UnityEngine.AI;

public class SoldierEnemy : Target
{

    public float noticeRadius = 35f;
    public float attackRadius = 25f;
    public float fireRate = 10f;
    public GameObject ammoPrefab; // Prefab for ammo
    public Transform ammoDropPoint;

    private GameObject player;
    private NavMeshAgent agent;
    private float nextFireTime = 0f;
    private bool isChasing = false;

    protected override void Start()
    {
        base.Start();
        health = 100;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= noticeRadius)
        {
            isChasing = true;
            agent.SetDestination(player.transform.position);

            if (distanceToPlayer <= attackRadius && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    private void Patrol()
    {
        // Implementasi patrol sederhana atau gunakan A* Pathfinding
    }

    private void Shoot()
    {
        Debug.Log("Soldier shooting at the player!");
        // Implementasikan logika serangan soldier (misalnya raycast)
    }

    protected override void Die()
    {
        Debug.Log("Soldier died!");
        PlayDeathAnimation();

        if (Random.value <= 0.5f) // 50% drop chance
        {
            DropAmmo();
        }

        Destroy(gameObject, 2f); // Hapus objek setelah animasi
    }

    private void PlayDeathAnimation()
    {
        // Mainkan animasi mati
    }

    private void DropAmmo()
    {
        if (ammoPrefab != null && ammoDropPoint != null)
        {
            GameObject ammo = Instantiate(ammoPrefab, ammoDropPoint.position, Quaternion.identity);
            //ammo.GetComponent<Ammo>().ActivateGlowEffect();
        }
    }

}
