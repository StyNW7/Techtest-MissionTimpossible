using UnityEngine;

public class Patrol : MonoBehaviour
{

    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float pointVisitRange = 1f;

    private int currentWaypoint = 0;

    void Update()
    {
        PatrolWaypoints();
    }

    void PatrolWaypoints()
    {
        
        if (waypoints.Length == 0) return;

        Vector3 targetPosition = waypoints[currentWaypoint].position;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * moveSpeed);
        }

        if (Vector3.Distance(transform.position, targetPosition) < pointVisitRange)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length; // Loop waypoints
        }

    }

}
