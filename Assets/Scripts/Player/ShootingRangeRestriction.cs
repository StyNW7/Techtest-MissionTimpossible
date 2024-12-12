using UnityEngine;

public class ShootingRangeRestriction : MonoBehaviour
{

    [SerializeField] private Transform shootingRangeCenter;
    [SerializeField] private float restrictedRadius = 10f;

    private Vector3 lastSafePosition;
    [SerializeField] private GameObject blocker;

    void Start()
    {
        lastSafePosition = transform.position;
        blocker.SetActive(false);
    }

    void Update()
    {

        float distance = Vector3.Distance(transform.position, shootingRangeCenter.position);

        if (distance < restrictedRadius)
        {
            Debug.Log("You cannot enter the Shooting Range!");
            transform.position = lastSafePosition;
            blocker.SetActive(true);
        }
        else
        {
            lastSafePosition = transform.position;
            blocker.SetActive(false);
        }

    }

}
