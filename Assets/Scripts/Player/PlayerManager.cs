using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;

    //public GameObject healthBarPrefab;
    [SerializeField] SliderHealthBar healthBar;

    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] public PlayerHUD hud;

    public string activeWeapon;

    public bool getPistol = false;
    public bool getRifle = false;

    // Health Manager

    [SerializeField] public TPSController playerBody;

    public DamageEffect damageEffect;
    public ScreenShake screenShake;

    [SerializeField] ThirdPersonController2 tps;

    // Player to Enemy Manager

    [SerializeField] int killPatrolEnemy = 0;

    [SerializeField] int killPortalEnemy = 0;

    // Weapon

    public WeaponChange weaponController;

    void Start()
    {

        playerBody = FindObjectOfType<TPSController>();
        currentHealth = maxHealth;

        if (hud != null)
        {
            hud.SetHealth(currentHealth);
        }

    }

    void Update()
    {
        playerBody = FindObjectOfType<TPSController>();
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(float damage)
    {

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        else Debug.Log("Health Bar null");

        if (hud != null)
        {
            //Debug.Log("Health" + currentHealth);
            hud.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            playerBody.Die();
        }
        else
        {
            playerBody.ScreenShake();
        }

    }

    

    public void SetActiveWeapon(string weapon)
    {

        if (weapon != "None") {
            tps.MoveSpeed = 5;
            tps.SprintSpeed = 8;
        }

        else
        {
            tps.MoveSpeed = 6;
            tps.SprintSpeed = 9.335f;
        }

        activeWeapon = weapon;
        Debug.Log($"Switched to: {weapon}");

    }

    public string GetActiveWeapon()
    {
        return activeWeapon;
    }

    public void SetPistol()
    {
        getPistol = !getPistol;
    }

    public void SetRiffle()
    {
        getRifle = !getRifle;
    }

    public bool CheckPistol()
    {
        return getPistol;
    }

    public bool CheckRiffle()
    {
        return getRifle;
    }

    // Enemy

    public int GetPatrolEnemyKill()
    {
        return killPatrolEnemy;
    }

    public int GetPortalEnemyKill()
    {
        return killPortalEnemy;
    }

    public void SetPatrolEnemyKill()
    {
        
        killPatrolEnemy += 1;
        Debug.Log("PatrolEnemy Died: " + killPatrolEnemy);

    }

    public void SetPortalEnemyKill()
    {
        
        killPortalEnemy += 1;
        Debug.Log("PortalEnemy Died: " + killPortalEnemy);

    }

}
