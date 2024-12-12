using TMPro;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{

    public string npcName = "Asuna";
    private bool isPlayerNearby = false;

    public GameObject player;
    public GameObject interactHUD;
    public float interactDistance = 3f;
    public TextMeshProUGUI dialogText;

    // Protect the player from collect pistol before mission is issued

    private bool isCollectRightNow = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Press 'F' to interact with " + npcName);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void Update()
    {

        float distance = Vector3.Distance(player.transform.position, transform.position);

        //if ()

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
            MissionChecker();
        }
    }

    void Interact()
    {

        Mission currentMission = MissionManager.instance.GetCurrentMission();

        if (currentMission != null)
        {
            if (currentMission.isCompleted)
            {
                MissionManager.instance.CompleteCurrentMission();
                Debug.Log(npcName + ": Good job! You've completed the mission.");
                // Update for the new mission
                MissionManager.instance.SetMissionIndex();
                Debug.Log(npcName + ": Here is your next mission: " + currentMission.missionDescription);
            }
            else
            {
                Debug.Log(npcName + ": Here is your mission: " + currentMission.missionDescription);
            }
        }

        else
        {
            Debug.Log(npcName + ": You've completed all missions! Well done!");
        }

    }

    void MissionChecker()
    {

        Mission currentMission = MissionManager.instance.GetCurrentMission();

        // Logic Mission 1: As long as player interact with the NPC mission is completed

        if (currentMission != null && currentMission.missionID == 1)
        {
            if (!currentMission.isCompleted)
            {
                Debug.Log("Mission 1: Completed");
                MissionManager.instance.CompleteCurrentMission();
                isCollectRightNow = true;
            }
        }

    }

    // Setter to check if player can collect pistol or no for mission 2

    public bool canICollect()
    {
        return isCollectRightNow;
    }

}
