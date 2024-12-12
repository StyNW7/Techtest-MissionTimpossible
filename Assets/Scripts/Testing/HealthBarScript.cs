using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierHealthBar : MonoBehaviour
{

    public Slider slider;
    public Image fillImage;

    public void Setmaxhealth(int health)
    {

        slider.maxValue = health;
        slider.value = health;

    }

    public void Sethealth(int health)
    {

        Debug.Log("Slider berkurang");
        slider.value = health;
        UpdateHealthBarColor();

    }

    private void UpdateHealthBarColor()
    {
        float healthPercentage = slider.value / slider.maxValue;
        Debug.Log("HealthBar: " + healthPercentage);
        if (healthPercentage > 0.7f)
            fillImage.color = Color.green;
        else if (healthPercentage > 0.5f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }

}