using UnityEngine;

public class CameraRotationBoundaries : MonoBehaviour
{
    public float rotationSpeed = 0.05f;
    public float minYAngle = -30f;
    public float maxYAngle = 30f;

    private float currentY = 0.0f;

    void Update()
    {
        currentY += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        transform.rotation = Quaternion.Euler(0, currentY, 0);
    }

}
