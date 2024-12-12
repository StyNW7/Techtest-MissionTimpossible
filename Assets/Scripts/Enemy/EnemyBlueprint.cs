using UnityEngine;

public abstract class Target : MonoBehaviour
{
    public float health; // Health saat ini
    public GameObject hpBarPrefab; // Prefab HealthBar

    private GameObject hpBar;
    private HealthBar healthBarScript;

    protected virtual void Start()
    {
        // Spawn HealthBar
        if (hpBarPrefab != null)
        {
            hpBar = Instantiate(hpBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity, transform);
            healthBarScript = hpBar.GetComponent<HealthBar>();

            if (healthBarScript != null)
            {
                healthBarScript.InitializeHealthBar(health); // Atur health maksimum
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHPBar();

        if (health <= 0)
        {
            Die();
        }
    }

    protected void UpdateHPBar()
    {
        if (healthBarScript != null)
        {
            healthBarScript.UpdateHealth(health);
        }
    }

    protected abstract void Die();
}
