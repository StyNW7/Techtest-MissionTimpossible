using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Animations.Rigging;

public class CollectibleItem : MonoBehaviour
{

    private bool isPlayerNearby = false;
    public GameObject player;
    public PlayerHUD hud;
    public TextMeshProUGUI dialogText;
    public GameObject interactHUD;

    [SerializeField] public string itemName;
    [SerializeField] public string itemType;
    [SerializeField] private bool isCollected = false;

    public NPCInteract npc;
    [SerializeField] public PlayerManager playerManager;

    public float interactionDistance = 3f;

    public Rifle riffle;
    public Pistol pistol;
    public WeaponChange weaponChange;

    void Start()
    {
        if (npc == null)
        {
            npc = FindObjectOfType<NPCInteract>();
            if (npc == null) Debug.LogError("NPCInteract component not found!");
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) Debug.LogError("Player with tag 'Player' not found!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log($"Distance to player: {distanceToPlayer}");

        Ray ray = new Ray();

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        }

        //Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        RaycastHit hit;

        // Pistol and Riffle

        if (distanceToPlayer <= interactionDistance && ((itemName == "Pistol" && npc.canICollectPistol()) || (itemName == "Riffle" && npc.canICollectRiffle())))
        {
            if (Physics.Raycast(ray, out hit, interactionDistance) && hit.transform == transform)
            {
                if (!isPlayerNearby)
                {
                    isPlayerNearby = true;
                    interactHUD.SetActive(true);
                    dialogText.text = $"Press 'F' to pickup {itemName}";
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    CollectItem();
                }
            }
            else if (isPlayerNearby)
            {
                isPlayerNearby = false;
                interactHUD.SetActive(false);
            }
        }

        // Ammo

        else if (distanceToPlayer <= interactionDistance && ((itemName == "PistolAmmo" && npc.canICollectPistol()) || (itemName == "RiffleAmmo" && npc.canICollectRiffle())))
        {

            //Debug.Log("Near Ammo");
            
            if (isPlayerNearby)
            {
                isPlayerNearby = false;
                interactHUD.SetActive(false);
                dialogText.text = $"Press 'F' to pickup {itemName}";
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                CollectItem();
            }
            
            else if (!isPlayerNearby)
            {
                isPlayerNearby = true;
                interactHUD.SetActive(true);
            }
        }

        // Tunnel

        else if (distanceToPlayer <= interactionDistance && (itemName == "Tunnel"))
        {

            if (isPlayerNearby)
            {
                isPlayerNearby = false;
                interactHUD.SetActive(false);
                dialogText.text = $"Press 'F' to go to the Village Area";
            }

            if (Input.GetKeyDown(KeyCode.F))
            {

                // Save Data First
                DataManager.Instance.SaveGame(playerManager.currentHealth,
                    pistol.ammoCount, riffle.ammoCount, pistol.totalAmmo, riffle.totalAmmo);

                // Baru ganti scene (biar data health + ammo2nya masih sama)

                SceneManager.LoadScene("VillageArea");

            }

            else if (!isPlayerNearby)
            {
                isPlayerNearby = true;
                interactHUD.SetActive(true);
            }
        }

        else if (isPlayerNearby)
        {
            isPlayerNearby = false;
            interactHUD.SetActive(false);
        }

    }


    void CollectItem()
    {

        Debug.Log($"You have picked up: {itemName}");
        isCollected = true;
        interactHUD.SetActive(false);

        if (itemName == "Pistol")
        {
            weaponChange.SetPistol();
            playerManager.SetActiveWeapon("Pistol");
            playerManager.SetPistol();
        }

        else if (itemName == "Riffle")
        {
            weaponChange.SetRiffle();
            playerManager.SetActiveWeapon("Riffle");
            playerManager.SetRiffle();
        }

        else if (itemName == "PistolAmmo")
        {
            pistol.SetTotalAmmo();
        }

        else if (itemName == "RiffleAmmo")
        {
            riffle.SetTotalAmmo();
        }

        Destroy(gameObject);

    }

    void OnDestroy()
    {

        if (itemName == "Pistol")
        {
            Mission currentMission = MissionManager.instance.GetCurrentMission();

            if (currentMission != null && currentMission.missionID == 2)
            {
                hud.ChangeTextColor();
                MissionManager.instance.CompleteCurrentMission();
                Debug.Log("Mission 2 completed: Pick Up The Pistol");
            }
        }

        //else if (itemName == "Riffle")
        //{
        //    Mission currentMission = MissionManager.instance.GetCurrentMission();

        //    if (currentMission != null && currentMission.missionID == 4)
        //    {
        //        hud.ChangeTextColor();
        //        MissionManager.instance.CompleteCurrentMission();
        //        Debug.Log("Mission 4 completed: Pick Up The Riffle");
        //    }
        //}

        if (hud != null)
        {
            hud.gameObject.SetActive(true);
        }

    }

}
