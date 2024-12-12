using UnityEngine;

public class Mission
{

    // This is the blueprint for every mission

    public int missionID;
    public string missionDescription;
    public bool isCompleted;
    public bool isCutScene;

    public Mission(int id, string description)
    {
        missionID = id;
        missionDescription = description;
        isCompleted = false;
        isCutScene = false;
    }

    public void CompleteMission()
    {
        isCompleted = true;
        Debug.Log("Mission " + missionID + " completed!");
    }

}
