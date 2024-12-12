using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Pistol : WeaponBase
{

    // Class for Pistol Inherit from WeaponBase

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] public PlayerManager playerManager;

    private StarterAssetsInputs starterAssetsInputs;

    [SerializeField] public PlayerHUD hud;
    [SerializeField] public TextMeshProUGUI missionText;

    // Music

    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;

    // Mission 3 Variable

    public int onTarget = 0;
    public bool mission3Status = false;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject shootingRange;
    public float shootDistance = 20f;

    // Enemy Patrol

    //[SerializeField] EnemyPatrol3 enemyPatrol;

    [SerializeField] BossAreaScript bossScript;


    // Recoil

    public GunRecoil gunRecoil;


    private void Start()
    {
        bulletDamage = 70;
        fireRate = 1;
        bulletSpeed = 600;
        bulletDrop = 50;
        ammoCount = 7;
        magazineSize = 7;
        totalAmmo = 14;
        //playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.R) && playerManager.GetActiveWeapon() == "Pistol")
        {
            if (totalAmmo > 0)
            {
                Reload();
            }
            else
            {
                Debug.Log("Pistol Out of ammo! Cannot reload.");
            }
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found! Please add AudioSource component to the Pistol object.");
        }

    }

    public override void Shoot()
    {

        if (ammoCount <= 0)
        {
            if (totalAmmo > 0)
            {
                Reload();
            }
            else
            {
                Debug.Log("Out of ammo! Cannot reload.");
            }
            return;
        }


        if (CanShoot())
        {

            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            Transform hitTransform = null;

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                mouseWorldPosition = raycastHit.point;
                hitTransform = raycastHit.transform;
            }

            ammoCount--;

            Debug.Log("Shoot with a pistol!");

            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            // Recoil

            StartCoroutine(gunRecoil.ApplyRecoilKickBack());

            // Hitscan

            if (hitTransform != null)
            {

                // Checking where player shoot

                float distanceToEnemy = Vector3.Distance(player.transform.position, hitTransform.position);
                float distanceToPlayer = Vector3.Distance(shootingRange.transform.position, player.transform.position);
                Debug.Log($"PlayerPosition: {shootingRange.transform.position}");
                Debug.Log($"PlayerPosition: {player.transform.position}");
                Debug.Log($"Distance to player: {distanceToPlayer}");
                Debug.Log($"Shoot Distance: {shootDistance}");

                Transform vfx;
                if (distanceToPlayer <= 13f && hitTransform != null && hitTransform.GetComponent<BulletTarget>() != null)
                    {

                    vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);
                    onTarget++;
                    Debug.Log(onTarget);

                    if (onTarget <= 10)
                    {
                        string updateMission = "Shoot 10 Rounds at the shooting target!(" + onTarget + "/10)!";
                        hud.SetMission(updateMission);
                    }

                    if (onTarget == 10)
                    {

                        Mission currentMission = MissionManager.instance.GetCurrentMission();

                        if (currentMission != null && currentMission.missionID == 3)
                        {
                            hud.ChangeTextColor();
                            MissionManager.instance.CompleteCurrentMission();
                            Debug.Log("Mission 3 Completed");
                            mission3Status = true;
                        }

                    }

                    //hud.UpdateTargetShoot(onTarget);

                }

                // shoot enemy

                else if (hitTransform.CompareTag("Enemy") && distanceToEnemy <= 35f)
                {

                    vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);

                    // Mendapatkan script EnemyPatrol3 dari objek yang terkena
                    EnemyPatrol3 enemy = hitTransform.GetComponentInParent<EnemyPatrol3>();

                    if (enemy != null)
                    {
                        enemy.TakeDamage(bulletDamage);
                        Destroy(vfx.gameObject, 0.3f);
                    }

                    else if (distanceToEnemy > 35f)
                    {
                        Debug.Log("Kejauhan nembaknya");
                    }

                    else
                    {
                        Debug.Log("Enemy tidak memiliki script EnemyPatrol3!");
                    }

                }

                else if (hitTransform.CompareTag("PortalEnemy") && distanceToEnemy <= 35f)
                {

                    vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);

                    PortalEnemy enemy = hitTransform.GetComponentInParent<PortalEnemy>();

                    if (enemy != null)
                    {
                        enemy.TakeDamage(bulletDamage);
                        Destroy(vfx.gameObject, 0.3f);
                    }

                    else if (distanceToEnemy > 35f)
                    {
                        Debug.Log("Kejauhan nembaknya");
                    }

                    else
                    {
                        Debug.Log("Enemy tidak memiliki script PortalEnemy!");
                    }

                }

                else if (hitTransform.CompareTag("BossEnemy") && distanceToEnemy <= 35f)
                {

                    vfx = null;

                    if (bossScript.GetBossStarted())
                    {
                        vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);

                        //BossScript enemy = hitTransform.GetComponentInParent<BossScript>();
                        BossEnemyModified enemy = hitTransform.GetComponentInParent<BossEnemyModified>();

                        if (enemy != null)
                        {
                            enemy.TakeDamage(bulletDamage);
                            Destroy(vfx.gameObject, 0.3f);
                        }

                        else if (distanceToEnemy > 35f)
                        {
                            Debug.Log("Kejauhan nembaknya");
                        }

                        else
                        {
                            Debug.Log("Enemy tidak memiliki script BossEnemy!");
                        }
                    }

                }

                else
                {
                    vfx = Instantiate(vfxHitRed, raycastHit.point, Quaternion.identity);
                }

                Destroy(vfx.gameObject, 0.3f);

            }

            //starterAssetsInputs.shoot = false;

        }

    }

    public override bool CanShoot()
    {
        return ammoCount > 0 && Time.time >= nextFireTime;
    }

    public int GetBullet()
    {
        return ammoCount;
    }

    public int GetTotalAmmo()
    {
        return totalAmmo;
    }

    public void SetTotalAmmo()
    {
        totalAmmo = totalAmmo + 7;
    }

    public bool Mission3Completed()
    {
        return mission3Status;
    }

    public int CountOnTarget()
    {
        return onTarget;
    }

}
