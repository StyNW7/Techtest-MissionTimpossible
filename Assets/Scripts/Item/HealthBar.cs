using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public Transform greenBar;
    public Transform redBar;

    private float maxHealth;

    public void InitializeHealthBar(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        greenBar.localScale = new Vector3(healthPercentage, 1f, 1f);

        if (healthPercentage > 0.5f)
            greenBar.GetComponent<Renderer>().material.color = Color.green;
        else if (healthPercentage > 0.2f)
            greenBar.GetComponent<Renderer>().material.color = Color.yellow;
        else
            greenBar.GetComponent<Renderer>().material.color = Color.red;
    }

    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

}
