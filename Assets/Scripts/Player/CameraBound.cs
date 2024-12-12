using UnityEngine;

public class CameraBound : MonoBehaviour
{
    public Transform target;  // Objek yang akan diikuti kamera
    public float distance = 5.0f;  // Jarak kamera dari objek
    public float rotationSpeed = 2.0f;  // Kecepatan rotasi kamera
    public float minYAngle = -30f;  // Batas rotasi bawah
    public float maxYAngle = 30f;  // Batas rotasi atas
    public float minXAngle = -60f;  // Batas rotasi kiri
    public float maxXAngle = 60f;  // Batas rotasi kanan

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // Batasi rotasi vertikal dan horizontal
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        currentX = Mathf.Clamp(currentX, minXAngle, maxXAngle);
    }

    void LateUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + rotation * direction;
        transform.LookAt(target);
    }
}
