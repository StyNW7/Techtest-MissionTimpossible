using UnityEngine;
using System.Collections;

public class CutSceneNPC : MonoBehaviour
{

    public GameObject cam1;
    public GameObject player;
    public GameObject canvas;

    // Collection of Cut Scene Cam

    public GameObject mission1Cam;
    public GameObject mission2Cam;
    public GameObject mission3Cam;
    public GameObject mission4Cam;
    void Start()
    {

        // Reset all camera

        mission1Cam.SetActive(false);
        mission2Cam.SetActive(false);
        mission3Cam.SetActive(false);
        mission4Cam.SetActive(false);

        // Reset other + Play the first Scene

        canvas.SetActive(false);
        player.SetActive(false);
        StartCoroutine(TheSequence());

    }

    public IEnumerator Mission1()
    {
        mission1Cam.SetActive(true);
        PlayerCanvasOff();

        yield return new WaitForSeconds(5);

        PlayerCanvasOn();
        mission1Cam.SetActive(false);
    }

    public IEnumerator Mission2()
    {
        mission2Cam.SetActive(true);
        PlayerCanvasOff();

        yield return new WaitForSeconds(5);

        PlayerCanvasOn();
        mission2Cam.SetActive(false);
    }

    public IEnumerator Mission3()
    {
        mission3Cam.SetActive(true);
        PlayerCanvasOff();

        yield return new WaitForSeconds(5);

        PlayerCanvasOn();
        mission3Cam.SetActive(false);
    }

    public IEnumerator Mission4()
    {
        mission4Cam.SetActive(true);
        PlayerCanvasOff();

        yield return new WaitForSeconds(5);

        PlayerCanvasOn();
        mission4Cam.SetActive(false);
    }

    IEnumerator TheSequence()
    {
        yield return new WaitForSeconds(5);
        PlayerCanvasOn();
        cam1.SetActive(false);
    }

    private void PlayerCanvasOff()
    {
        player.SetActive(false);
        canvas.SetActive(false);
    }

    private void PlayerCanvasOn()
    {
        player.SetActive(true);
        canvas.SetActive(true);
    }

}
