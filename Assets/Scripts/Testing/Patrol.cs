using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolScript : MonoBehaviour
{

    static int MinX, MaxX, MinZ, MaxZ;
    static float GridSize = 3f;
    static float MaxSteep = 1.5f;
    static float EndX, EndZ;
    static float StopAstarChasing, StopAstarPatroling;
    static float ShootingRange;
    static float ChasingRange;

    static public Transform target;

    private SoldierHealth hs;

    // Waypoints

    public Transform[] waypoints;
    public GameObject parentPatrolPoints;
    private int pointIndex = 0;
    private int currentNodeIndex = 0;

    // Enemy  Object

    private Animator anim;
    public GameObject Player;
    float MoveSpeed;
    float RotateSpeed;
    [SerializeField] private LayerMask ground;
    private bool isGrounded;
    private float gravity = -9.8f;
    private float groundDistance;
    private Vector3 velocity;
    private CharacterController controller;
    bool IsChasing;
    bool IsShooting;
    bool IsPatroling;

    // A STAR

    public Pathfinding3 pathFinding;
    public List<Vector3> Path;
    float AStarCoolDown;
    float LastAStar;
    float PointDistance;
    float speed;
    public List<Vector3> PatrolPoints;
    int PatrolIndex;
    float PointVisitRange;

    // HP system

    float hp;
    float maxHp;

    [SerializeField] GameObject PistolAmmo;
    [SerializeField] GameObject RifleAmmo;

    // Player Manager

    [SerializeField] PlayerManager playerManager;

    void Start()
    {

        // Component

        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        target = GameObject.Find("char").transform;

        // Attribute and Property for A*

        MinX = 0;
        MaxX = 500;
        MinZ = 0;
        MaxZ = 500;

        MoveSpeed = 3f;
        RotateSpeed = 10f;

        ShootingRange = 25;
        ChasingRange = 35;

        StopAstarChasing = 20;
        StopAstarPatroling = 3.5f;

        IsChasing = false;
        IsShooting = false;
        IsPatroling = false;

        AStarCoolDown = 2f;
        LastAStar = 1;

        PointDistance = 3f;
        PointVisitRange = 1f;

        // A* path

        Path = new List<Vector3>();

        maxHp = 100;
        hp = maxHp;
        Field = new Node[MaxX + 2, MaxZ + 2];

        // Init points

        parentPatrolPoints = GameObject.Find("PatrolPoints");

        PatrolIndex = -1;

        foreach (Transform PatrolPoint in parentPatrolPoints.transform)
        {
            PatrolPoints.Add(PatrolPoint.position);
        }

        // Health

        hs = gameObject.GetComponent<SoldierHealth>();

        // For checking if there is an active terrain

        if (Terrain.activeTerrain == null)
        {
            Debug.LogError("Terrain not found!");
            return;
        }

    }

    void Update()
    {

        // Logic Enemy

        float distX = Player.transform.position.x - transform.position.x;
        float distZ = Player.transform.position.z - transform.position.z;

        // a = akar kuadrat dari b^2 + c^2

        float dist = Mathf.Sqrt(distX * distX + distZ * distZ);

        Debug.Log("Enemy distance to player " + dist);

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
            Patroling();
        }

        // Gravitasi

        //isGrounded = Physics.CheckSphere(transform.position, groundDistance, ground);
        //if (isGrounded && velocity.y < 0)
        //{
        //    velocity.y = 2f;
        //}
        //velocity.y += gravity * Time.deltaTime;

        //controller.Move(velocity * Time.deltaTime);

    }

    // A* Property: Heuristic Value

    public static float getHeuristic(float X, float Z)
    {
        X += (GridSize / 2);
        Z += (GridSize / 2);
        float GCost = Mathf.Sqrt(Mathf.Pow((X - EndX), 2) + Mathf.Pow((Z - EndZ), 2) * 1f);
        float HCost = Mathf.Sqrt(Mathf.Pow((X - target.position.x), 2) + Mathf.Pow((Z - target.position.z), 2) * 1f);
        //return GCost + HCost;
        return GCost;
    }

    // Nested Class: Node Class

    public class Node
    {

        public int PrevX, PrevZ;
        public int NextX, NextZ;
        public int X, Z;
        public bool Visited;
        public float Heuristic;
        public Node() { }
        public Node(int X, int Z)
        {
            this.X = X;
            this.Z = Z;
            this.PrevX = -1;
            this.PrevZ = -1;
            this.NextX = -1;
            this.NextZ = -1;
            this.Visited = false;
            this.Heuristic = getHeuristic(X, Z);
        }
        public void SetPrev(int X, int Z)
        {
            this.PrevX = X;
            this.PrevZ = Z;
        }
        public void SetNext(int X, int Z)
        {
            this.NextX = X;
            this.NextZ = Z;
        }
        public void MarkVisit()
        {
            this.Visited = true;
        }

    }
    
    // Class node udah selesai

    public Node[,] Field;

    public Node GetField(int X, int Z)
    {
        if (Field[X, Z] == null)
        {
            Field[X, Z] = new Node(X, Z);
        }
        return Field[X, Z];
    }

    public bool CheckPos(int CurrentX, int CurrentZ, int PrevX, int PrevZ)
    {
        Vector3 Current = new Vector3(CurrentX, 0, CurrentZ);
        Vector3 Prev = new Vector3(PrevX, 0, PrevZ);
        if (
            CurrentX >= MinX && CurrentX <= MaxX && CurrentZ >= MinZ && CurrentZ <= MaxZ
            && GetField(CurrentX, CurrentZ).Visited == false
            && astarMapper.ValidField[CurrentX, CurrentZ]
            && Mathf.Abs(Terrain.activeTerrain.SampleHeight(Current) - Terrain.activeTerrain.SampleHeight(Prev)) < MaxSteep // not too steep
        ) return true;
        return false;
    }

    public float getDistance(float X1, float Z1, float X2, float Z2)
    {
        return Mathf.Sqrt((Mathf.Pow((X1 - X2), 2f) + Mathf.Pow((Z1 - Z2), 2f)));
    }


    // Path Finding using A*

    public List<Vector3> pathfind(int StartX, int StartZ, int X2, int Z2, float StopAstarRange)
    {

        List<Vector3> Result = new List<Vector3>();
        EndX = X2;
        EndZ = Z2;
        Field = new Node[MaxX + 2, MaxZ + 2];
        bool found = false;
        int LastX = -1, LastZ = -1;

        GetField(StartX, StartZ).MarkVisit();
        List<Node> Queue = new List<Node>();
        Node TempNode = new Node(StartX, StartZ);
        Queue.Add(TempNode);

        while (Queue.Count > 0 && !found)
        {

            // Biar dapet palign murah, init mahal dulu
            
            float LeastHeuristic = 10000000f;
            int LeastIndex = -1;
            for (int i = 0; i < Queue.Count; i++)
            {
                if (Queue[i].Heuristic < LeastHeuristic)
                {
                    LeastIndex = i;
                    LeastHeuristic = Queue[i].Heuristic;
                }
            }

            // Check current node

            Node Check = Queue[LeastIndex];

            Debug.Log("visiting " + Check.X + " " + Check.Z);

            Queue.RemoveAt(LeastIndex);

            if (getDistance(Check.X, Check.Z, EndX, EndZ) <= StopAstarRange)
            {

                found = true;
                Debug.Log("found " + Check.X + " " + Check.Z);
                LastX = Check.X;
                LastZ = Check.Z;
                break;

            }

            for (int i = -1; i <= 1; i++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (i == 0 && k == 0) continue;
                    int TempX = Check.X + i;
                    int TempZ = Check.Z + k;
                    if (CheckPos(TempX, TempZ, Check.X, Check.Z))
                    {
                        int CountSide = 0;
                        if (k == -1 || k == 1) CountSide++;
                        if (i == -1 || i == 1) CountSide++;
                        if (CountSide == 2)
                        {
                            if (!(CheckPos(TempX - i, TempZ, Check.X, Check.Z) && CheckPos(TempX, TempZ - k, Check.X, Check.Z))) continue;
                        }
                        GetField(TempX, TempZ).MarkVisit();
                        GetField(TempX, TempZ).SetPrev(Check.X, Check.Z);
                        TempNode = new Node(TempX, TempZ);
                        Queue.Add(TempNode);
                    }
                    else
                    {
                         Debug.Log("Invalid arouund");
                    }
                }
            }
        }


        if (found)
        {
            int CurrentX = LastX;
            int CurrentZ = LastZ;
            int PrevX = -1, PrevZ = -1, NextX, NextZ;
            while (GetField(CurrentX, CurrentZ).PrevX > 0)
            {
                Debug.Log("Back tracking: " + CurrentX + " " + CurrentZ);
                PrevX = GetField(CurrentX, CurrentZ).PrevX;
                PrevZ = GetField(CurrentX, CurrentZ).PrevZ;
                GetField(PrevX, PrevZ).SetNext(CurrentX, CurrentZ);
                CurrentX = PrevX;
                CurrentZ = PrevZ;
            }

            CurrentX = StartX;
            CurrentZ = StartZ;

            while (GetField(CurrentX, CurrentZ).NextX > 0 && GetField(CurrentX, CurrentZ).NextZ > 0)
            {
                Result.Add(new Vector3(CurrentX, 0, CurrentZ));
                NextX = GetField(CurrentX, CurrentZ).NextX;
                NextZ = GetField(CurrentX, CurrentZ).NextZ;
                CurrentX = NextX;
                CurrentZ = NextZ;
            }
        }

        else
        {
             Debug.Log("Not found");
        }

        return Result;

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

    // Function untuk enemy nembak

    void Shooting()
    {
        Debug.Log("Shoot Player");
        FaceToPlayer();
        IsShooting = true;
        IsChasing = false;
        IsPatroling = false;
        anim.SetFloat("Shooting", 1f);
        PatrolWeapon weapon = GetComponentInParent<PatrolWeapon>();
        weapon.ShootToPlayer();

    }

    // Function Enemy Chasing

    void Chasing()
    {

        Debug.Log("Chase Player");

        // Chasing baru dimulai

        if (!IsChasing)
        {
            IsShooting = false;
            IsChasing = true;
            IsPatroling = false;
            anim.SetFloat("Shooting", 1f);
            Path = pathfind(
                Mathf.FloorToInt(transform.position.x),
                Mathf.FloorToInt(transform.position.z),
                Mathf.FloorToInt(Player.transform.position.x),
                Mathf.FloorToInt(Player.transform.position.z),
                StopAstarChasing
            );
        }

        // Update pathfinder

        if (Time.time > LastAStar + AStarCoolDown)
        {
            LastAStar = Time.time;
            Path = pathfind(
                Mathf.FloorToInt(transform.position.x),
                Mathf.FloorToInt(transform.position.z),
                Mathf.FloorToInt(Player.transform.position.x),
                Mathf.FloorToInt(Player.transform.position.z),
                StopAstarChasing
            );
        }

        Vector3 NextPoint;
        float height;
        bool Repeat = false;

        do
        {
            if (Path.Count <= 0) return;
            Repeat = false;
            NextPoint = Path[0];
            height = Terrain.activeTerrain.SampleHeight(NextPoint);
            NextPoint.y = height;
            if (getDistance(
                NextPoint.x,
                NextPoint.z,
                transform.position.x,
                transform.position.z
            ) < PointDistance)
            {
                Repeat = true;
                Path.RemoveAt(0);
            }
        } while (Repeat);

        FaceToPosition(NextPoint);
        speed = MoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, NextPoint, speed);
        Debug.Log("Enemy chasing to" + Path[0].x + " " + NextPoint.y + " " + Path[0].y);

    }

    // Patrol Function
    void Patroling()
    {

        if (PatrolIndex == -1)
        {
            // first patrol
            Debug.Log("first patrol");
            PatrolIndex = Random.Range(0, PatrolPoints.Count - 1);
            Debug.Log(PatrolPoints[PatrolIndex].x + " and " + PatrolPoints[PatrolIndex].z);
        }

        // Udah berhenti patroli atau baru mau lagi patroli

        if (!IsPatroling)
        {

            IsShooting = false;
            IsChasing = false;
            IsPatroling = true;
            Path = pathfind(
                Mathf.FloorToInt(transform.position.x),
                Mathf.FloorToInt(transform.position.z),
                Mathf.FloorToInt(PatrolPoints[PatrolIndex].x),
                Mathf.FloorToInt(PatrolPoints[PatrolIndex].z),
                StopAstarPatroling
            );

            Debug.Log("Jumlah path: " + Path.Count);

            if (Path == null || Path.Count == 0)
            {
                Debug.Log("Path kosong! Tidak dapat melanjutkan patrol.");
                MoveToWaypoint();
                return;
            }

        }

        // Lagi patroli

        else
        {

            Vector3 NextPoint;
            float height;
            bool Repeat = false;
            do

            {
                if (Path.Count <= 0) return;
                Repeat = false;
                NextPoint = Path[0];
                height = Terrain.activeTerrain.SampleHeight(NextPoint);
                NextPoint.y = height;
                if (getDistance(
                    NextPoint.x,
                    NextPoint.z,
                    transform.position.x,
                    transform.position.z
                ) < PointDistance)
                {
                    Repeat = true;
                    Path.RemoveAt(0);
                }
            } while (Repeat);

            FaceToPosition(NextPoint);
            speed = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, NextPoint, speed);
            Debug.Log("Patroling to" + Path[0].x + " " + Path[0].y);

        }

        if (getDistance(
            PatrolPoints[PatrolIndex].x,
            PatrolPoints[PatrolIndex].z,
            transform.position.x,
            transform.position.z
        ) < PointVisitRange)

        {
            // Ubah destinasi yang dirandom waypoint-nya
            PatrolIndex = Random.Range(0, PatrolPoints.Count - 1);
            Path = pathfind(
                Mathf.FloorToInt(transform.position.x),
                Mathf.FloorToInt(transform.position.z),
                Mathf.FloorToInt(PatrolPoints[PatrolIndex].x),
                Mathf.FloorToInt(PatrolPoints[PatrolIndex].z),
                StopAstarPatroling
            );
        }

    }

    public void TakeDamage(float damageReceived)
    {
        Debug.Log("take damage of " + damageReceived);
        hp -= damageReceived;
        if (hp < 0) Die();
    }

    void Die()
    {
        playerManager.SetPatrolEnemyKill();
        int rand = Random.Range(1, 5);
        if (rand == 3) Instantiate(RifleAmmo, transform.position, Quaternion.identity);
        if (rand == 4) Instantiate(PistolAmmo, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void MoveToWaypoint()
    {

        Vector3 targetPosition = PatrolPoints[PatrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        // Jika sudah sampai di waypoint, pindah ke waypoint berikutnya
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            pointIndex = (pointIndex + 1) % waypoints.Length; // Loop waypoints
            CalculatePathToNextWaypoint();
        }

    }

    public void CalculatePathToNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            Debug.Log("Ga ada waypoints");
            return;
        }

        Transform nextWaypoint = waypoints[pointIndex];
        //Path = pathFinding.FindPath(transform.position, nextWaypoint.position);
        currentNodeIndex = 0;
    }

}