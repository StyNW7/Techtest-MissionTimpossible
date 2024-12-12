using UnityEngine;

[System.Serializable]
public class GameData
{

    // Player Data

    public float playerHealth;

    // Weapon Data

    public int pistolAmmo;
    public int riffleAmmo;

    public int pistolTotalAmmo;
    public int riffleTotalAmmo;

    // Mission

    public string soldierMission = "Eliminate the soldiers that are attacking the village! (0/16)";

}
