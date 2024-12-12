using System.Collections;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{

    [Header("Recoil Settings")]

    public float recoilAmount = 2f;
    public float recoilSpeed = 5f;
    public float recoilSmoothness = 10f;

    [Header("Kickback Settings")]

    public Transform gunTransform;
    public Vector3 kickbackAmount = new Vector3(0f, 0.5f, -0.1f);
    public float kickbackSpeed = 10f;

    [Header("Camera Shake Settings")]

    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;

    private Vector3 originalGunPosition;
    private Vector3 currentRecoil = Vector3.zero;
    private Vector3 targetRecoil = Vector3.zero;
    private float shakeTimer = 0f;

    private void Start()
    {
        if (gunTransform != null)
        {
            originalGunPosition = gunTransform.localPosition;
        }
    }

    private void Update()
    {

        ////Recoil rotation (smooth transition)

        //currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, Time.deltaTime * recoilSmoothness);
        //Camera.main.transform.localRotation = Quaternion.Euler(currentRecoil);

        //// Reset recoil target (back to zero)

        //targetRecoil = Vector3.Lerp(targetRecoil, Vector3.zero, Time.deltaTime * recoilSpeed);

        //// Camera shake logic

        //if (shakeTimer > 0)
        //{
        //    Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity;
        //    Camera.main.transform.localPosition += shakeOffset;
        //    shakeTimer -= Time.deltaTime;
        //}

        //// Smooth return of gun to original position

        //if (gunTransform != null)
        //{
        //    gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, originalGunPosition, Time.deltaTime * kickbackSpeed);
        //}

    }

    // Apply recoil and kickback effect
    public void ApplyRecoil(float verticalRecoil, float horizontalRecoil)
    {

        //targetRecoil += new Vector3(-verticalRecoil, horizontalRecoil, 0f);

        //shakeTimer = shakeDuration;

        // Apply kickback
        if (gunTransform != null)
        {
            gunTransform.localPosition += kickbackAmount;
        }

    }

    public IEnumerator ApplyRecoilKickBack()
    {

        // Apply kickback
        if (gunTransform != null)
        {
            gunTransform.localPosition += kickbackAmount;
        }

        yield return new WaitForSeconds(0.2f);

        // Apply Kickfront
        if (gunTransform != null)
        {
            gunTransform.localPosition -= kickbackAmount;
        }

    }

}
