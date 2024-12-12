using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeAreaLoad : MonoBehaviour
{
    public PlayerManager player;
    public Pistol pistol;
    public Rifle riffle;
    public TMP_Text missionText;
    public PlayerHUD hud;
    public SliderHealthBar healthBar;

    private void Start()
    {
        // Jalankan coroutine untuk menunda eksekusi LoadPlayerData sampai frame berikutnya
        StartCoroutine(WaitAndLoadData());
    }

    private IEnumerator WaitAndLoadData()
    {
        // Tunggu hingga frame berikutnya untuk memastikan semua objek diinisialisasi
        yield return null;

        // Setelah semua objek diinisialisasi, jalankan LoadPlayerData
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        GameData data = DataManager2.Instance.LoadGame();
        if (data != null)
        {
            Debug.Log("Health: " + data.playerHealth);
            Debug.Log("Pistol Ammo: " + data.pistolAmmo);
            Debug.Log("Riffle Ammo: " + data.riffleAmmo);
            Debug.Log("Pistol Total: " + data.pistolTotalAmmo);
            Debug.Log("Riffle Total: " + data.riffleTotalAmmo);
            Debug.Log("Mission: " + data.soldierMission);

            // Update player dan senjata berdasarkan data yang di-load
            player.currentHealth = data.playerHealth;
            player.maxHealth = data.playerHealth;
            pistol.ammoCount = data.pistolAmmo;
            riffle.ammoCount = data.riffleAmmo;
            pistol.totalAmmo = data.pistolTotalAmmo;
            riffle.totalAmmo = data.riffleTotalAmmo;
            string mission = data.soldierMission;
            healthBar.UpdateHealth(data.playerHealth);
            //hud.UpdateHUD(mission, data.playerHealth);
            hud.SetMission(mission);
            hud.SetHealth(data.playerHealth);
        }
        else
        {
            Debug.Log("No saved data found!");
        }
    }
}
