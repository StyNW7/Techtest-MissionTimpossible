using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;

public class CutSceneTrigger : MonoBehaviour
{

    public GameObject cam1;
    public GameObject player;
    public GameObject canvas;
    public GameObject damageCanvas;

    public GameObject mission5cam;
    public GameObject interactText;
    public RiggingController rig;
    public SliderHealthBar healthBar;
    void Start()
    {

        mission5cam.SetActive(false);
        interactText.SetActive(false);

        // Reset other + Play the first Scene

        damageCanvas.SetActive(false);
        canvas.SetActive(false);
        player.SetActive(false);
        StartCoroutine(TheSequence());

    }

    IEnumerator TheSequence()
    {
        PlayerCanvasOff();
        yield return new WaitForSeconds(5);
        PlayerCanvasOn();
        rig.NoWeapon();
        cam1.SetActive(false);
        healthBar.UpdateHealthBarColor();
    }

    public IEnumerator Mission5()
    {
        mission5cam.SetActive(true);
        PlayerCanvasOff();

        yield return new WaitForSeconds(5);

        PlayerCanvasOn();
        mission5cam.SetActive(false);
    }

    private void PlayerCanvasOff()
    {
        player.SetActive(false);
        canvas.SetActive(false);
        interactText.SetActive(false);
    }

    private void PlayerCanvasOn()
    {
        player.SetActive(true);
        canvas.SetActive(true);
        damageCanvas.SetActive(true);
    }

}
