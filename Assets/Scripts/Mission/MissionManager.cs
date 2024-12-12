using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class MissionManager : MonoBehaviour
{

    // Class that controls everything about mission and get the mission blueprint form Mission Class

    public static MissionManager instance;
    public List<Mission> missions;
    private int currentMissionIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Getter and Setter for the MissionIndex

    public int GetMissionIndex()
    {
        return currentMissionIndex;
    }
    public void SetMissionIndex()
    {
        currentMissionIndex += 1;
    }

    void Start()
    {
        InitializeMissions();
    }

    void Update()
    {
        
    }

    void InitializeMissions()
    {
        missions = new List<Mission> {
            new Mission(1, "Find ‘Asuna’ and Talk to Her!"),
            new Mission(2, "Pick Up The Pistol"),
            new Mission(3, "Shoot 10 Rounds at the shooting target!(0/10)!"),
            new Mission(4, "Shoot 50 Bullets with the rifle! (0/50)"),
            new Mission(5, "Eliminate the soldiers that are attacking the village! (0/16)"),
            new Mission(6, "Head to the secret teleport room and defeat the boss")
        };
    }

    public Mission GetCurrentMission()
    {
        if (currentMissionIndex < missions.Count)
        {
            return missions[currentMissionIndex];
        }
        return null;
    }

    public void CompleteCurrentMission()
    {
        if (currentMissionIndex < missions.Count)
        {
            missions[currentMissionIndex].CompleteMission();
            currentMissionIndex++;
        }
    }

    // Winning Condition

    public bool IsAllMissionsCompleted()
    {
        return currentMissionIndex >= missions.Count;
    }

}
