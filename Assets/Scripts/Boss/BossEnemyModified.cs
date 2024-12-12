using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemyModified : MonoBehaviour
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

    private BossHealth hs;

    private Animator anim;
    public GameObject Player;
    float RotateSpeed;

    bool IsChasing;
    bool IsShooting;
    bool IsPatroling;

    static float ShootingRange;
    static float ChasingRange;

    float StartShootingTime, StartChasingTime;
    float ShootingDuration, ChasingDuration;

    static public bool BossStart;

    // HP system

    float hp;
    float maxHp;

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

        hs = gameObject.GetComponent<BossHealth>();

        maxHp = 2000;
        hp = maxHp;

        ChasingDuration = 5f;
        ShootingDuration = 5f;

        StartChasingTime = Time.time;
        IsChasing = true;

        BossStart = false;

        //pathfinding = FindObjectOfType<Pathfinding3>();
        //grid = FindObjectOfType<GridManager3>();

        //CalculatePathToNextWaypoint();

    }

    void Update()
    {

        if (!BossStart) return;

        // Logic Enemy

        //float distX = Player.transform.position.x - transform.position.x;
        //float distZ = Player.transform.position.z - transform.position.z;

        //// a = akar kuadrat dari b^2 + c^2

        //float dist = Mathf.Sqrt(distX * distX + distZ * distZ);

        ////Debug.Log("Enemy distance to player " + dist);

        //// 3 Condition

        //if (dist <= ShootingRange)
        //{
        //    // Masih hidup
        //    if (hs.currhealth > 0)
        //    {
        //        Shooting();
        //    }
        //}

        //else if (dist <= ChasingRange)
        //{
        //    Chasing();
        //}

        //else
        //{
        //    Patrol();
        //}

        ChaseOrShoot();

    }

    void ChaseOrShoot()
    {

        if (IsShooting)
        {
            if (Time.time > StartShootingTime + ShootingDuration)
            {
                IsShooting = false;
                IsChasing = true;
                StartChasingTime = Time.time;
            }
        }

        else
        {
            if (
                Time.time > StartChasingTime + ChasingDuration
            )
            {

                IsShooting = true;
                IsChasing = false;
                StartShootingTime = Time.time;
            }
        }


        if (IsShooting)
        {
            anim.SetBool("Chasing", false);
            Shooting();
        }
        else
        {
            Chasing();
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
            anim.SetBool("Shooting", true);
        }

        //anim.SetFloat("Shooting", 1f);
        BossWeapon weapon = GetComponentInChildren<BossWeapon>();
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
            anim.SetBool("Chasing", true);
            anim.SetBool("Flying", true);
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
            anim.SetBool("Chasing", true);
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

        Vector3 targetPosition = Player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        //if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        //{
        //    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        //    CalculatePathToNextWaypoint();
        //}

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
        //Debug.Log("Enemy take damage of " + damageReceived);
        hp -= damageReceived;
        hs.takedmg((int)damageReceived);
        if (hp < 0)
        {
            Debug.Log("Boss mati");
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {

        anim.SetBool("Die", true);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Victoryyyyyyy");
        SceneManager.LoadScene("VictoryScene");

    }

    public void ResetAnimation()
    {
        anim.SetBool("Shooting", false);
        anim.SetBool("Chasing", false);
        anim.SetBool("Flying", false);
        anim.SetBool("Die", false);
    }

}
