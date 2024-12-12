using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Rifle : WeaponBase
{

    // Class for Riffle Inherit from WeaponBase

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

    // Mission 4 Variable

    public int onShooting = 0;
    public bool mission4Status = false;

    // NPC Interact

    public NPCInteract npc;

    [SerializeField] BossAreaScript bossAreaScript;


    // Recoil

    public GunRecoil gunRecoil;

    private void Start()
    {

        bulletDamage = 35;
        fireRate = 9;
        bulletSpeed = 500;
        bulletDrop = 50;
        ammoCount = 30;
        magazineSize = 30;
        totalAmmo = 30;

        //playerManager = GetComponent<PlayerManager>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found! Please add AudioSource component to the Pistol object.");
        }

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && playerManager.GetActiveWeapon() == "Riffle")
        {
            if (totalAmmo > 0)
            {
                this.Reload();
            }
            else
            {
                Debug.Log("Riffle Out of ammo! Cannot reload.");
            }
        }
    }

    public override void Shoot()
    {

        if (ammoCount <= 0)
        {
            if (totalAmmo > 0)
            {
                this.Reload();
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

            Debug.Log("Shoot with a riffle!");

            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }


            // Recoil

            StartCoroutine(gunRecoil.ApplyRecoilKickBack());


            // Bullet shoot logic


            if (hitTransform != null)
            {

                Transform vfx;
                if (npc != null && !npc.GetVillageArea() && hitTransform.GetComponent<BulletTarget>() != null)
                {

                    vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);

                }

                // In the village area to shoot patrol enemy

                else if (npc != null && npc.GetVillageArea() && hitTransform.CompareTag("Enemy"))
                {

                    Debug.Log("Berhasil kena tag Enemy");

                    vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);

                    // Mendapatkan script EnemyPatrol3 dari objek yang terkena
                    EnemyPatrol3 enemy = hitTransform.GetComponentInParent<EnemyPatrol3>();

                    if (enemy != null)
                    {
                        enemy.TakeDamage(bulletDamage);
                        Destroy(vfx.gameObject, 0.3f);
                    }

                    else
                    {
                        Debug.LogWarning("Enemy tidak memiliki script EnemyPatrol3!");
                    }

                }

                else if (npc != null && npc.GetVillageArea() && hitTransform.CompareTag("PortalEnemy"))
                {

                    Debug.Log("Berhasil kena tag Portal Enemy");

                    vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);

                    PortalEnemy enemy = hitTransform.GetComponentInParent<PortalEnemy>();

                    if (enemy != null)
                    {
                        enemy.TakeDamage(bulletDamage);
                        Destroy(vfx.gameObject, 0.3f);
                    }

                    else
                    {
                        Debug.LogWarning("Enemy tidak memiliki script PortalEnemy!");
                    }

                }

                else if (npc == null && hitTransform.CompareTag("BossEnemy"))
                {

                    vfx = null;

                    if (bossAreaScript.GetBossStarted())
                    {
                        Debug.Log("Berhasil kena tag Boss Enemy");

                        vfx = Instantiate(vfxHitGreen, raycastHit.point, Quaternion.identity);

                        //BossScript enemy = hitTransform.GetComponentInParent<BossScript>();
                        BossEnemyModified enemy = hitTransform.GetComponentInParent<BossEnemyModified>();

                        if (enemy != null)
                        {
                            enemy.TakeDamage(bulletDamage);
                            Destroy(vfx.gameObject, 0.3f);
                        }

                        else
                        {
                            Debug.LogWarning("Enemy tidak memiliki script BossEnemy!");
                        }
                    }

                }

                // Shoot anything

                else
                {
                    vfx = Instantiate(vfxHitRed, raycastHit.point, Quaternion.identity);
                }

                Destroy(vfx.gameObject, 0.3f);

                // No need shoot on target, as long as shoot with a riffle


                if (npc != null && !npc.GetVillageArea())
                {

                    onShooting++;
                    Debug.Log(onShooting);

                    if (onShooting <= 50)
                    {
                        string updateMission = $"Shoot 50 Bullets with the rifle! ({onShooting}/50)";
                        hud.SetMission(updateMission);
                    }

                    if (onShooting == 50)
                    {

                        Mission currentMission = MissionManager.instance.GetCurrentMission();

                        if (currentMission != null && currentMission.missionID == 4)
                        {
                            hud.ChangeTextColor();
                            MissionManager.instance.CompleteCurrentMission();
                            Debug.Log("Mission 4 Completed");
                            mission4Status = true;
                        }

                    }

                }

            }

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
        totalAmmo = totalAmmo + 30;
    }

    public bool Mission4Completed()
    {
        return mission4Status;
    }

    public int CountOnTarget()
    {
        return onShooting;
    }

}
