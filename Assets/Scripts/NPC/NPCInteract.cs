using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class NPCInteract : MonoBehaviour
{

    public string npcName = "Asuna";
    private bool isPlayerNearby = false;  
    public GameObject player;               
    public GameObject interactHUD;               
    public float interactDistance = 3f;       
    public TextMeshProUGUI dialogText;  
    public GameObject playerBody;

    [SerializeField] public PlayerHUD hud;


    // Flag untuk memastikan pemain dapat mengumpulkan item (misalnya pistol) saat misi dikeluarkan

    // Make public for NPC in the Village Area

    public bool isCollectPistol = false;
    public bool isCollectRiffle = false;


    public TMP_Text missionText;
    public Color resetMissionColor = Color.white;

    // Music

    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;

    // Tunnel

    [SerializeField] public GameObject tunnelDoor;
    [SerializeField] public GameObject tunnelCollider;

    // Village  Area

    [SerializeField] public bool villageArea;

    // Cut Scene

    public CutSceneNPC cutScene;
    public CutSceneTrigger cutSceneVillage;


    // Player

    [SerializeField] PlayerManager playerManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Masuk interaksi");
            isPlayerNearby = true;
            interactHUD.SetActive(true);
            dialogText.text = "Press 'F' to interact with " + npcName;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactHUD.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            playerBody.SetActive(false);
            StartCoroutine(Unfreeze());
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
            //StartCoroutine(Interact());
            Interact();
            MissionChecker();
            //playerBody.SetActive(true);
        }
    }

    private void Awake()
    {
        isPlayerNearby = false;
        interactHUD.SetActive(false);
    }

    private void Start()
    {
        isPlayerNearby = false;
        interactHUD.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found! Please add AudioSource component to the Pistol object.");
        }
    }
    void Interact()
    {

        Mission currentMission = MissionManager.instance.GetCurrentMission();

        Debug.Log(hud);
        Debug.Log(isCollectRiffle);

        //Debug.Log(currentMission.missionDescription);

        if (currentMission != null)
        {
            if (villageArea && playerManager.GetPatrolEnemyKill() == 16)
            {
                dialogText.text = npcName + ": Here is your mission: " + 
                    "Head to the secret teleport room and defeat the boss";
                return;
            }

            else if (villageArea)
            {
                dialogText.text = npcName + ": Baru bunuh " + playerManager.GetPatrolEnemyKill() + " Patrol";
                return;
            }

            if (currentMission.isCompleted)
            { 

                MissionManager.instance.CompleteCurrentMission();
                dialogText.text = npcName + ": Good job! You've completed the mission.";
                MissionManager.instance.SetMissionIndex();
                hud.SetMission(MissionManager.instance.GetCurrentMission().missionDescription);
                hud.ResetTextColor();

            }
            else
            {
                hud.ResetTextColor();
                hud.SetMission(currentMission.missionDescription);
                dialogText.text = npcName + ": Here is your mission: " + currentMission.missionDescription;
            }
        }
        else
        {
            dialogText.text = npcName + ": You've completed all missions! Well done!";
        }
    }

    void MissionChecker()
    {

        Mission currentMission = MissionManager.instance.GetCurrentMission();

        // Mission 6 -> Kill 16 soldier enemy

        if (villageArea && playerManager.GetPatrolEnemyKill() == 16)
        {

            // Already kill 16 soldier enemy
            if (true)
            {
                hud.SetMission("Head to the secret teleport room and defeat the boss");
                hud.ResetTextColor();
                Destroy(tunnelDoor);
            }
            return;

        }

        else
        {
            Debug.Log("Baru bunuh: " + playerManager.GetPatrolEnemyKill() + "Patrol");
        }

        // Logic untuk misi 1: Jika misi 1 belum selesai dan pemain berinteraksi, misi dianggap selesai
        if (currentMission != null && currentMission.missionID == 1 && !villageArea)
        {
            if (!currentMission.isCompleted)
            {
                MissionManager.instance.CompleteCurrentMission();
                dialogText.text = "Mission 1: Completed!";
                isCollectPistol = true;
            }
        }

        // Logic written in the CollectibleItem.cs for Mission 3

        //else if (currentMission != null && currentMission.missionID == 3)
        //{
        //    Pistol pistol = GetComponent<Pistol>();
        //    string updateMission = "Shoot 10 Rounds at the shooting target!(" + pistol.CountOnTarget() + "/10)!";
        //    hud.SetMission(updateMission);
        //}

        // Player can collect Riffle (Mission 4)

        else if (currentMission != null && currentMission.missionID == 4)
        {
            //Rifle riffle = GetComponent<Rifle>();
            //string updateMission = $"Shoot 50 Bullets with the rifle! (" +riffle.CountOnTarget()+"/50)";
            //hud.SetMission(updateMission);
            if (!currentMission.isCompleted)
            {
                isCollectRiffle = true;
            }
        }

        // Mission 5 -> Open the Tunnel

        else if (currentMission != null && currentMission.missionID == 5)
        {
            if (!currentMission.isCompleted)
            {
                Destroy(tunnelDoor);
                Destroy(tunnelCollider);
            }
        }

    }

    IEnumerator Unfreeze()
    {
        //yield return new WaitForSeconds(2);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        

        // Cut Scene

        Mission currentMission = MissionManager.instance.GetCurrentMission();

        Debug.Log("Mission: " + currentMission.missionID);

        if (currentMission.missionID == 1 && villageArea && !currentMission.isCutScene)
        {
            Debug.Log("Mission 2 Cut Scene");
            StartCoroutine(cutSceneVillage.Mission5());
            currentMission.isCutScene = true;
        }

        if (currentMission.missionID == 2 && !currentMission.isCompleted && !currentMission.isCutScene)
        {
            Debug.Log("Mission 2 Cut Scene");
            StartCoroutine(cutScene.Mission1());
            currentMission.isCutScene = true;
        }

        else if (currentMission.missionID == 3 && !currentMission.isCompleted && !currentMission.isCutScene)
        {
            Debug.Log("Mission 3 Cut Scene");
            StartCoroutine(cutScene.Mission2());
            currentMission.isCutScene = true;
        }

        else if (currentMission.missionID == 4 && !currentMission.isCompleted && !currentMission.isCutScene)
        {
            Debug.Log("Mission 4 Cut Scene");
            StartCoroutine(cutScene.Mission3());
            currentMission.isCutScene = true;
        }

        else if (currentMission.missionID == 5 && !currentMission.isCompleted && !currentMission.isCutScene)
        {
            Debug.Log("Mission 5 Cut Scene");
            StartCoroutine(cutScene.Mission4());
            currentMission.isCutScene = true;
        }

        // Munculin Player (setelah atau meskipun ga ada Cut Scene)

        playerBody.SetActive(true);

    }

    public void ChangeTextColor()
    {
        if (missionText != null)
        {
            missionText.color = resetMissionColor;
        }
    }

    public void setCollectPistol()
    {
        isCollectPistol = !isCollectPistol;
    }

    public void setCollectRiffle()
    {
        isCollectRiffle = !isCollectRiffle;
    }

    public bool canICollectPistol()
    {
        return isCollectPistol;
    }

    public bool canICollectRiffle()
    {
        return isCollectRiffle;
    }

    public bool GetVillageArea()
    {
        return villageArea;
    }

}
