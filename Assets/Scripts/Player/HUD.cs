using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    public TMP_Text missionText;
    public TMP_Text healthText;

    private float playerHealth = 100f;
    private string currentMission = "Find ‘Asuna’ and Talk to Her!";

    public Color resetMissionColor = Color.white;
    public Color missionColor = Color.green;

    void Start()
    {
        UpdateHUD();
    }

    void Update()
    {
        // Testing
        //if (playerHealth > 0)
        //{
        //    playerHealth -= Time.deltaTime * 5;
        //}
        UpdateHUD();
    }

    void TakeDamage(float damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene("Lose");
    }

    public void UpdateHUD()
    {
        missionText.text = "Current Mission: " + currentMission;
        healthText.text = "Health: " + Mathf.Round(playerHealth).ToString() + " / 100";
    }

    public void UpdateHUD(string mission, float health)
    {
        missionText.text = "Current Mission: " + mission;
        healthText.text = "Health: " + Mathf.Round(health).ToString() + " / 100";
    }

    // Function to set mission
    public void SetMission(string mission)
    {
        currentMission = mission;
        UpdateHUD();
    }

    // Function to set weapon
    public void SetWeapon(string weapon)
    {
        UpdateHUD();
    }

    // Function to set health
    public void SetHealth(float health)
    {
        playerHealth = health;
        UpdateHUD();
    }

    public void ChangeTextColor()
    {
        if (missionText != null)
        {
            missionText.color = missionColor;
        }
    }

    public void ResetTextColor()
    {
        if (missionText != null)
        {
            missionText.color = resetMissionColor;
        }
    }


    // Mission 3 Logic

    public void UpdateTargetShoot(int onTarget)
    {
        Debug.Log("Jumlah target sekarang: " + onTarget);
        Debug.Log(missionText.text);
        missionText.text = $"Current Mission: Shoot 10 Rounds at the shooting target!({onTarget}/10)!";
    }


}
