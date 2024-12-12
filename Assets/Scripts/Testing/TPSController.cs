using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class TPSController : MonoBehaviour
{

    // Components

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform bulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private ThirdPersonController2 thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private PlayerManager playerManager;
    private Animator animator;

    // Weapon

    [SerializeField] public Pistol pistol;
    [SerializeField] public Rifle riffle;

    // Camera

    public Transform playerBody;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;

    [SerializeField] public PlayerHUD hud;

    // Mission 3 Variable
    public int onTarget = 0;

    // Effect

    public DamageEffect damageEffect;
    public ScreenShake screenShake;
    public Image damageImage;
    private Color originalColor;

    private void Awake()
    {

        playerManager = GetComponent<PlayerManager>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController2>();
        animator = GetComponent<Animator>();

        playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("PlayerManager ga adaaaa");
        }

        if (damageEffect == null)
            damageEffect = GetComponent<DamageEffect>();
        if (screenShake == null)
            screenShake = GetComponent<ScreenShake>();

    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        // Death Animation
        animator.SetTrigger("DieTrigger");
        StartCoroutine(Freeze());
    }

    public void ScreenShake()
    {
        screenShake.TriggerShake();
    }

    IEnumerator Freeze()
    {
        damageImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("DeathScene");
    }

    private void Update()
    {

        //RotateCamera();

        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        // Aiming

        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        // Shoot logic

        if (starterAssetsInputs.shoot)
        {

            Debug.Log("Want to shoot");

            //playerManager.ShootActiveWeapon();

            starterAssetsInputs.shoot = false;

            if (playerManager.GetActiveWeapon() == "Pistol")
            {
                // Recoil
                //mouseWorldPosition += new Vector3(2f, 0f, 0f);
                ApplyRecoil(1.5f);
                pistol.Shoot();
            }

            else if (playerManager.GetActiveWeapon() == "Riffle")
            {
                // Recoil
                //mouseWorldPosition += new Vector3(2f, 0f, 0f);
                ApplyRecoil(1f);
                riffle.Shoot();
            }

        }

    }

    // Recoil

    private void ApplyRecoil(float recoilAmount)
    {
        Transform cameraTransform = Camera.main.transform;

        Vector3 currentRotation = cameraTransform.localEulerAngles;
        currentRotation.x -= recoilAmount;
        cameraTransform.localEulerAngles = currentRotation;
    }

    public int Mission3Condition()
    {
        return onTarget;
    }

    private void Shoot()
    {

        //Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        //if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        //{
        //    Transform hitTransform = raycastHit.transform;

        //    // Tampilkan efek jika mengenai target
        //    Transform vfx = Instantiate(
        //        hitTransform.GetComponent<BulletTarget>() != null ? vfxHitGreen : vfxHitRed,
        //        raycastHit.point,
        //        Quaternion.identity
        //    );
        //    Destroy(vfx.gameObject, 0.3f);

        //    // Logika peluru
        //    Vector3 aimDirection = (raycastHit.point - spawnBulletPosition.position).normalized;
        //    Instantiate(bulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        //}

    }

    //private void RotateCamera()
    //{

    //    float mouseX = StarterAssetsInputs.look.x * mouseSensitivity * Time.deltaTime;
    //    float mouseY = StarterAssetsInputs.look.y * mouseSensitivity * Time.deltaTime;

    //    xRotation -= mouseY;
    //    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    //    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    //    playerBody.Rotate(Vector3.up * mouseX);

    //}

}


// Archieved


// Hitscan

//if (hitTransform != null)
//{
//    Transform vfx;
//    if (hitTransform.GetComponent<BulletTarget>() != null)
//    {
//        vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);
//    }
//    else
//    {
//        vfx = Instantiate(vfxHitRed, raycastHit.point, Quaternion.identity);
//    }
//    Destroy(vfx.gameObject, 0.3f);
//}

//// Projectile

////Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
////Transform bullet = Instantiate(bulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
//starterAssetsInputs.shoot = false;
