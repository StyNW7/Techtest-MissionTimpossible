using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : Target
{
    public float fireRate = 14f;
    public float moveInterval = 5f;
    public Transform battleArea;
    public GameObject bossHPBarPrefab; // HP bar khusus di layar
    private float nextMoveTime = 0f;
    private float nextFireTime = 0f;

    private GameObject player;
    private NavMeshAgent agent;

    protected override void Start()
    {
        base.Start();
        health = 2000;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        if (bossHPBarPrefab != null)
        {
            // Spawn HP bar di UI
            Instantiate(bossHPBarPrefab);
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= battleArea.localScale.x / 2) // Jika pemain berada dalam area pertempuran
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }

            if (Time.time >= nextMoveTime)
            {
                MoveToRandomPosition();
                nextMoveTime = Time.time + moveInterval;
            }
        }
    }

    private void Shoot()
    {
        Debug.Log("Boss shooting at the player!");
        // Implementasikan logika serangan boss
    }

    private void MoveToRandomPosition()
    {
        Vector3 randomPosition = battleArea.position + new Vector3(
            Random.Range(-battleArea.localScale.x / 2, battleArea.localScale.x / 2),
            0,
            Random.Range(-battleArea.localScale.z / 2, battleArea.localScale.z / 2)
        );

        agent.SetDestination(randomPosition);
    }

    protected override void Die()
    {
        Debug.Log("Boss defeated!");
        PlayDeathAnimation();
        Destroy(gameObject, 3f); // Hapus objek setelah animasi
    }

    private void PlayDeathAnimation()
    {
        // Mainkan animasi mati
    }

}
