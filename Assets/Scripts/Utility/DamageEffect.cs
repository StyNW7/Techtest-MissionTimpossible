using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{

    public Image damageImage;
    public float flashDuration = 0.2f;
    private Color originalColor;

    void Start()
    {
        if (damageImage != null)
        {
            originalColor = damageImage.color;
            damageImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
    }

    // Fungsi untuk memicu flash merah
    public void FlashRed()
    {
        StartCoroutine(FlashRedCoroutine());
    }

    private IEnumerator FlashRedCoroutine()
    {
        Debug.Log("Damage effect");
        damageImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
        yield return new WaitForSeconds(flashDuration);
        damageImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

}
