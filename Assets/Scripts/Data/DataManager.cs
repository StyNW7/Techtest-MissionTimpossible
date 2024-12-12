using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // There is always DataManager in every scene
        }

        saveFilePath = Application.persistentDataPath + "/GameData.json";
    }

    public void SaveGame(float health, int pistolAmmo, int riffleAmmo,
        int pistolTotalAmmo, int riffleTotalAmmo)
    {
        GameData data = new GameData
        {
            playerHealth = health,
            pistolAmmo = pistolAmmo,
            riffleAmmo = riffleAmmo,
            pistolTotalAmmo = pistolTotalAmmo,
            riffleTotalAmmo = riffleTotalAmmo,
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved: " + json);
    }

    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded: " + json);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }

}
