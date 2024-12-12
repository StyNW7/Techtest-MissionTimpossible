using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyPatrol3 : MonoBehaviour
{

    // A* Props

    public Transform[] waypoints;
    public float speed = 2f;
    public float chasingSpeed = 3f;

    private int currentWaypointIndex = 0;
    private List<Node3> currentPath = new List<Node3>();
    [SerializeField] Pathfinding3 pathfinding;
    [SerializeField] GridManager3 grid;

    private int currentNodeIndex = 0;

    // Enemy

    private SoldierHealth hs;

    private Animator anim;
    public GameObject Player;
    float RotateSpeed;

    bool IsChasing;
    bool IsShooting;
    bool IsPatroling;

    static float ShootingRange;
    static float ChasingRange;

    // HP system

    float hp;
    float maxHp;

    // Ammo

    [SerializeField] GameObject PistolAmmo;
    [SerializeField] GameObject RifleAmmo;

    // Player Manager

    [SerializeField] PlayerManager playerManager;
    public PlayerHUD hud;

    void Start()
    {

        anim = GetComponent<Animator>();

        IsChasing = false;
        IsShooting = false;
        IsPatroling = false;

        RotateSpeed = 10f;

        ShootingRange = 25;
        ChasingRange = 35;

        hs = gameObject.GetComponent<SoldierHealth>();

        maxHp = 100;
        hp = maxHp;

        //pathfinding = FindObjectOfType<Pathfinding3>();
        //grid = FindObjectOfType<GridManager3>();

        CalculatePathToNextWaypoint();

    }

    void Update()
    {

        // Logic Enemy

        float distX = Player.transform.position.x - transform.position.x;
        float distZ = Player.transform.position.z - transform.position.z;

        // a = akar kuadrat dari b^2 + c^2

        float dist = Mathf.Sqrt(distX * distX + distZ * distZ);

        //Debug.Log("Enemy distance to player " + dist);

        // 3 Condition

        if (dist <= ShootingRange)
        {
            // Masih hidup
            if (hs.currhealth > 0)
            {
                Shooting();
            }
        }

        else if (dist <= ChasingRange)
        {
            Chasing();
        }

        else
        {
            Patrol();
        }

    }

    // Enemy hadep ke player

    void FaceToPosition(Vector3 Pos)
    {
        var lookTo = Pos - transform.position;
        lookTo.y = 0;
        var rotation = Quaternion.LookRotation(lookTo);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotateSpeed);
    }
    void FaceToPlayer()
    {
        FaceToPosition(Player.transform.position);
    }

    private void Shooting()
    {

        Debug.Log("Shoot Player");
        FaceToPlayer();
        
        if (!IsShooting)
        {
            IsShooting = true;
            IsChasing = false;
            IsPatroling = false;
            anim.SetBool("EnemyAim", true);
        }

        //anim.SetFloat("Shooting", 1f);
        PatrolWeapon weapon = GetComponentInChildren<PatrolWeapon>();
        Debug.Log(weapon);
        weapon.ShootToPlayer();

    }

    private void Chasing()
    {

        Debug.Log("Chase Player");

        // Chasing baru dimulai

        if (!IsChasing)
        {
            IsShooting = false;
            IsChasing = true;
            IsPatroling = false;
            
        }

        FaceToPlayer();

        Vector3 targetPosition = Player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, chasingSpeed * Time.deltaTime);

    }


    // Patrol Function

    private void Patrol()
    {

        if (!IsPatroling)
        {
            IsShooting = false;
            IsChasing = false;
            IsPatroling = true;
            anim.SetBool("EnemyWalk", true);
        }

        if (currentPath.Count == 0)
        {
            MoveToWaypoint();
        }

        else
        {
            
            if (currentNodeIndex < currentPath.Count)
            {

                Node3 currentNode = currentPath[currentNodeIndex];
                Vector3 targetPosition = currentNode.worldPosition;

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    currentNodeIndex++;
                }

            }

            else
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                CalculatePathToNextWaypoint();
            }

        }
    }

    // Move to waypoint
    private void MoveToWaypoint()
    {

        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            CalculatePathToNextWaypoint();
        }

    }

    // Next waypoint

    private void CalculatePathToNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            Debug.Log("Ga ada waypoints");
            return;
        }

        Transform nextWaypoint = waypoints[currentWaypointIndex];
        currentPath = pathfinding.FindPath(transform.position, nextWaypoint.position);
        currentNodeIndex = 0;
    }

    public void TakeDamage(float damageReceived)
    {
        Debug.Log("Enemy take damage of " + damageReceived);
        hp -= damageReceived;
        hs.takedmg((int)damageReceived);
        if (hp < 0) Die();
    }

    void Die()
    {

        //anim.SetBool("Die", true);
        playerManager.SetPatrolEnemyKill();

        string missionText = "Eliminate the soldiers that are attacking the village! (" +
            playerManager.GetPatrolEnemyKill() + "/16)";

        hud.SetMission(missionText);

        if (playerManager.GetPatrolEnemyKill() == 16)
        {
            hud.ChangeTextColor();
        }

        int rand = Random.Range(1, 5);
        Debug.Log("Random ammo: " + rand);

        GameObject ammo = null;

        // 50% chance (1-4)
        if (rand == 3) ammo = Instantiate(RifleAmmo, transform.position, Quaternion.identity);
        if (rand == 4) ammo = Instantiate(PistolAmmo, transform.position, Quaternion.identity);

        if (ammo != null)
        {
            AddGlowEffect(ammo);
        }

        Destroy(this.gameObject);

    }

    void AddGlowEffect(GameObject ammo)
    {
        
        Light glowLight = ammo.AddComponent<Light>();
        glowLight.type = LightType.Point;
        glowLight.range = 5f;
        glowLight.intensity = 2f;
        glowLight.color = Color.yellow;

        //Destroy(glowLight, 10f); // Glow effect hilang setelah 10 detik

        StartCoroutine(DisableLightAfterDelay(glowLight, 10f));

    }

    IEnumerator DisableLightAfterDelay(Light glowLight, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (glowLight != null)
        {
            glowLight.enabled = false; // Matikan cahaya
        }
    }

}
