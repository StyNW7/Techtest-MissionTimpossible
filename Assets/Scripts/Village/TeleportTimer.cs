using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeleportTimer : MonoBehaviour
{

    public float countdownTime = 60f;
    public TMP_Text timerText;
    public GameObject timerPanel;
    public GameObject teleportOrb;
    public GameObject[] enemies;

    private float currentTime;
    private bool timerActive = false;
    private bool teleportActivated = false;

    // Player

    public GameObject player;
    private bool isPlayerNearby = false;
    public TextMeshProUGUI dialogText;
    public GameObject interactHUD;

    [SerializeField] private OrbHUD orbHUD;

    [SerializeField] public PlayerManager playerManager;

    public Rifle riffle;
    public Pistol pistol;

    void Start()
    {

        timerText.gameObject.SetActive(false);
        teleportOrb.SetActive(false);
        timerPanel.SetActive(false);
        currentTime = countdownTime;

    }

    void Update()
    {

        if (timerActive)
        {
            //timerText.gameObject.SetActive(true);
            currentTime -= Time.deltaTime;

            timerText.text = Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0f)
            {
                EndTimer(false);
            }

            if (AreAllEnemiesDefeated() && !teleportActivated)
            {
                EndTimer(true);
            }
        }

        if (teleportActivated)
        {

            float distanceToPlayer = Vector3.Distance(teleportOrb.transform.position, player.transform.position);
            //Debug.Log("Lokasi player ke Orb Portal: " + distanceToPlayer);

            if (distanceToPlayer <= 30f)
            {

                isPlayerNearby = true;

                //Debug.Log("Near Portal");

                if (isPlayerNearby)
                {
                    isPlayerNearby = true;
                    interactHUD.SetActive(true);
                    dialogText.text = $"Press 'F' to go to Boss Area";
                }

                if (Input.GetKeyDown(KeyCode.F))
                {

                    // Save Data First
                    if (DataManager.Instance != null)
                    {
                        DataManager.Instance.SaveGame(100, pistol.ammoCount, riffle.ammoCount, pistol.totalAmmo, riffle.totalAmmo);
                    }

                    // Go to Maze

                    Debug.Log("Go to Boss Area");
                    SceneManager.LoadScene("Maze");
                }

                else if (!isPlayerNearby)
                {
                    isPlayerNearby = false;
                    interactHUD.SetActive(false);
                }

            }

            else
            {
                isPlayerNearby = false;
                interactHUD.SetActive(false);
            }

        }

    }

    // Start timer
    public void StartTimer()
    {
        timerActive = true;
        timerPanel.SetActive(true);
        timerText.gameObject.SetActive(true);
        currentTime = countdownTime;
    }

    // Stop timer
    private void EndTimer(bool isVictory)
    {

        timerActive = false;

        if (isVictory)
        {

            orbHUD.ShowMissionHUD2(3f);
            teleportActivated = true;
            teleportOrb.SetActive(true);
            timerText.text = "Portal Enemy Cleared";
            timerText.color = Color.green;
            Debug.Log("Teleport Orb Activated!");

        }

        else
        {
            Debug.Log("Player Lost!");
            SceneManager.LoadScene("DeathScene");
        }

    }

    // Checking portal soldier
    private bool AreAllEnemiesDefeated()
    {

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                return false;
            }
        }

        return true;

    }

    // Trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !timerActive)
        {
            orbHUD.ShowMissionHUD(3f, StartTimer);
            //StartTimer();
        }
    }

}
