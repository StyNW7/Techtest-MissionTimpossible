//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyAI : MonoBehaviour
//{
//    public GridSystem grid;
//    public Transform[] patrolPoints;
//    public Transform player;
//    public float noticeRadius = 35f;
//    public float speed = 5f;

//    private List<Node> currentPath;
//    private int currentPatrolIndex;
//    private bool chasingPlayer = false;

//    private Pathfinding pathfinding;

//    void Start()
//    {
//        pathfinding = GetComponent<Pathfinding>();
//        currentPatrolIndex = 0;

//        Debug.Log("Enemy AI Initialized. Starting patrol...");
//        SetPatrolPath();
//    }

//    void Update()
//    {
//        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//        if (distanceToPlayer <= noticeRadius)
//        {
//            Debug.Log("Player detected! Chasing...");
//            chasingPlayer = true;
//            SetChasePath();
//        }

//        else if (chasingPlayer && distanceToPlayer > noticeRadius)
//        {
//            Debug.Log("Player lost. Returning to patrol...");
//            chasingPlayer = false;
//            SetPatrolPath();
//        }

//        if (!chasingPlayer && currentPath != null && currentPath.Count == 0)
//        {
//            Debug.Log($"Reached patrol point {currentPatrolIndex}. Moving to next...");
//            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
//            SetPatrolPath();
//        }

//        MoveAlongPath();
//    }

//    void SetChasePath()
//    {
//        currentPath = pathfinding.FindPath(transform.position, player.position);
//        if (currentPath == null)
//        {
//            Debug.LogWarning("No path found to player!");
//        }
//    }

//    void SetPatrolPath()
//    {
//        currentPath = pathfinding.FindPath(transform.position, patrolPoints[currentPatrolIndex].position);
//        if (currentPath == null)
//        {
//            Debug.LogWarning($"No path found to patrol point {currentPatrolIndex}!");
//        }
//    }

//    void MoveAlongPath()
//    {
//        if (currentPath == null || currentPath.Count == 0) return;

//        Vector3 targetPos = currentPath[0].worldPosition;
//        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

//        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
//        {
//            currentPath.RemoveAt(0);
//        }
//    }
//}
