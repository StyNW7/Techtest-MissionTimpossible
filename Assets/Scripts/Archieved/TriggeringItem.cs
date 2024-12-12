//using UnityEngine;
//using TMPro;

//public class CollectibleItem : MonoBehaviour
//{

//    // Player

//    private bool isPlayerNearby = false;
//    public GameObject player;
//    public PlayerHUD hud;
//    public TextMeshProUGUI dialogText;
//    public GameObject interactHUD;

//    // Item


//    [SerializeField] public string itemName;
//    [SerializeField] private bool isCollected = false;


//    public NPCInteract npc;


//    void Start()
//    {

//        if (npc == null)
//        {
//            //Debug.LogError("NPCInteract component not found in the scene.");
//            npc = FindObjectOfType<NPCInteract>();
//        }

//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player") && itemName == "Pistol" && npc.canICollectPistol())
//        {
//            isPlayerNearby = true;
//            interactHUD.SetActive(true);
//            dialogText.text = "Press 'F' to pickup " + itemName;
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            isPlayerNearby = false;
//            interactHUD.SetActive(false);
//        }
//    }

//    void Update()
//    {
//        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
//        {
//            CollectItem();
//        }
//    }

//    void CollectItem()
//    {

//        Debug.Log("You have picked up: " + itemName);

//        isCollected = true;
//        Destroy(gameObject);  // Delete Object because it's already taken

//    }

//    void OnDestroy()
//    {

//        if (itemName == "Pistol")
//        {
//            Mission currentMission = MissionManager.instance.GetCurrentMission();

//            if (currentMission != null && currentMission.missionID == 2)
//            {
//                hud.ChangeTextColor();
//                MissionManager.instance.CompleteCurrentMission();
//                Debug.Log("Mission 2 completed: Pick Up The Pistol");
//                interactHUD.SetActive(false);
//            }
//        }

//        else if (itemName == "Riffle")
//        {
//            Mission currentMission = MissionManager.instance.GetCurrentMission();

//            if (currentMission != null && currentMission.missionID == 4)
//            {
//                hud.ChangeTextColor();
//                MissionManager.instance.CompleteCurrentMission();
//                Debug.Log("Mission 4 completed: Pick Up The Riffle");
//            }
//        }

//    }

//}