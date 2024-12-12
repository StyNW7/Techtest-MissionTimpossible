using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ShowNPC : MonoBehaviour
{

    [SerializeField] GameObject NPCBody;
    public PlayerManager playerManager;

    void Start()
    {
        NPCBody.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerManager.GetPatrolEnemyKill() == 16)
        {
            NPCBody.SetActive(true);
        }
    }
}
