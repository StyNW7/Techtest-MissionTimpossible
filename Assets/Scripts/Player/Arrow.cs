using UnityEngine;

public class Arrow : MonoBehaviour
{


    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(transform.GetComponent<Rigidbody>());
    }


}
