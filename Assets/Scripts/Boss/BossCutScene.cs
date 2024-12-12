using UnityEngine;
using System.Collections;

public class BossCutScene : MonoBehaviour
{

    public GameObject cam1;
    public GameObject player;
    public GameObject canvas;

    public GameObject firstCam;

    void Start()
    {

        cam1.SetActive(false);
        StartCoroutine(MazeAreaScene());

    }

    public IEnumerator MazeAreaScene()
    {
        firstCam.SetActive(true);
        canvas.SetActive(false);
        //player.SetActive(false);
        yield return new WaitForSeconds(5);
        player.SetActive(true);
        firstCam.SetActive(false);
        canvas.SetActive(true);
    }

    public IEnumerator TheSequence()
    {
        cam1.SetActive(true);
        canvas.SetActive(false);
        player.SetActive(false);
        yield return new WaitForSeconds(5);
        player.SetActive(true);
        cam1.SetActive(false);
        canvas.SetActive(true);
    }

}
