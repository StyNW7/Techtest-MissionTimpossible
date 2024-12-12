using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWidget : MonoBehaviour
{

    [SerializeField] private PlayerHUD hud;

    [SerializeField] private TextMeshProUGUI pistolBulletText;
    [SerializeField] private TextMeshProUGUI riffleBulletText;

    [SerializeField] private TextMeshProUGUI pistolTotalAmmo;
    [SerializeField] private TextMeshProUGUI riffleTotalAmmo;

    [SerializeField] private Image pistolImage;
    [SerializeField] private Image riffleImage;

    [SerializeField] private Pistol pistol;
    [SerializeField] private Rifle riffle;

    private PlayerManager playerManager;

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager == null) Debug.LogError("PlayerManager not found in the scene!");
    }

    void Update()
    {
        UpdateWeaponHUD();
    }

    private void UpdateWeaponHUD()
    {

        // Update bullets

        if (pistol != null)
        {
            pistolBulletText.text = $"Pistol: {pistol.GetBullet()} / 7";
            pistolTotalAmmo.text = $"[{pistol.GetTotalAmmo()}]";
        }
        if (riffle != null)
        {
            riffleBulletText.text = $"Rifle: {riffle.GetBullet()} / 30";
            riffleTotalAmmo.text = $"[{riffle.GetTotalAmmo()}]";
        }

        // Update weapon image opacity based on active weapon
        if (playerManager != null && playerManager.activeWeapon != null)
        {
            UpdateWeaponImageOpacity(playerManager.GetActiveWeapon());
        }

    }

    public void UpdateWeaponImageOpacity(string activeWeapon)
    {

        if (activeWeapon == "Pistol")
        {
            SetImageOpacity(pistolImage, 1f);
            SetImageOpacity(riffleImage, 0.5f);
        }
        else if (activeWeapon == "Riffle")
        {
            SetImageOpacity(pistolImage, 0.5f);
            SetImageOpacity(riffleImage, 1f);
        }
        else
        {
            // Default case if no weapon is active
            SetImageOpacity(pistolImage, 0.5f);
            SetImageOpacity(riffleImage, 0.5f);
        }

    }

    private void SetImageOpacity(Image image, float opacity)
    {

        if (image != null)
        {
            Color color = image.color;
            color.a = opacity;
            image.color = color;
        }

    }

}
