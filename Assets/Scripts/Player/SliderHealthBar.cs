using UnityEngine;
using UnityEngine.UI;

public class SliderHealthBar : MonoBehaviour
{

    public Slider healthSlider;
    public Image fillImage;

    private float maxHealth;

    public void Start()
    {
        InitializeHealthBar(100);
    }

    public void InitializeHealthBar(float maxHealth)
    {
        this.maxHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        Debug.Log("Current Health: " + currentHealth);
        healthSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Update Health Bar");
        UpdateHealthBarColor();
    }

    public void UpdateHealthBarColor()
    {
        float healthPercentage = healthSlider.value / healthSlider.maxValue;

        Debug.Log("Health Slider Value: " + healthSlider.value);
        Debug.Log("Health Slider Max Value: " + healthSlider.maxValue);

        if (healthPercentage > 0.5f)
            fillImage.color = Color.green;
        else if (healthPercentage > 0.2f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }

}
