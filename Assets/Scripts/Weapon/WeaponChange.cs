using System.Collections;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    private PlayerManager playerManager;
    private CollectibleItem item;
    public WeaponWidget weaponWidget;
    public RiggingController rig;

    // Weapon
    [SerializeField] private GameObject rifflePrefab;
    [SerializeField] private GameObject pistolPrefab;

    // State Management
    private bool isSwitchingWeapon = false;

    private void Start()
    {
        rig.NoWeapon();
        ResetWeapon();
        playerManager = GetComponent<PlayerManager>();
        playerManager.SetActiveWeapon("None");
    }

    private void Update()
    {
        
        if (isSwitchingWeapon) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (playerManager.GetActiveWeapon() == "Riffle") return;
            if (playerManager.CheckRiffle())
            {
                StartCoroutine(SwitchWeapon("Riffle", rifflePrefab, rig.EquipRifle));
            }
            else
            {
                StartCoroutine(SwitchWeapon("None", null, rig.NoWeapon));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (playerManager.GetActiveWeapon() == "Pistol") return;
            if (playerManager.CheckPistol())
            {
                StartCoroutine(SwitchWeapon("Pistol", pistolPrefab, rig.EquipPistol));
            }
            else
            {
                StartCoroutine(SwitchWeapon("None", null, rig.NoWeapon));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (playerManager.GetActiveWeapon() == "None") return;
            StartCoroutine(SwitchWeapon("None", null, rig.NoWeapon));
        }
    }

    public void ResetWeapon()
    {
        rifflePrefab.SetActive(false);
        pistolPrefab.SetActive(false);
    }


    public void SetPistol()
    {
        StartCoroutine(SwitchWeapon("Pistol", pistolPrefab, rig.EquipPistol));
    }

    public void SetRiffle()
    {
        StartCoroutine(SwitchWeapon("Riffle", rifflePrefab, rig.EquipRifle));
    }

    private IEnumerator SwitchWeapon(string weaponType, GameObject weaponPrefab, System.Action equipAction)
    {
        isSwitchingWeapon = true;

        rig.NoWeapon();
        playerManager.SetActiveWeapon("None");
        ResetWeapon();

        yield return new WaitForSeconds(0.5f);

        if (weaponType != "None")
        {
            playerManager.SetActiveWeapon(weaponType);
            equipAction.Invoke();
            if (weaponPrefab != null) weaponPrefab.SetActive(true);
        }

        isSwitchingWeapon = false;
    }

}
