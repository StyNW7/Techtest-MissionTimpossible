using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 1f;
    private Vector3 originalPosition;
    private float shakeTimeRemaining;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.position = originalPosition;
        }
    }

    public void TriggerShake()
    {
        Debug.Log("ScreenShake");
        shakeTimeRemaining = shakeDuration;
    }

}
