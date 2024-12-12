using UnityEngine;
using System.Collections;

public class CutSceneManager : MonoBehaviour
{

    public GameObject cam1;
    public GameObject player;
    public GameObject canvas;
    public GameObject[] missionCams;

    void Start()
    {

        foreach (var cam in missionCams)
        {
            cam.SetActive(false);
        }

        cam1.SetActive(false);
        canvas.SetActive(false);
        player.SetActive(true);

    }

    public IEnumerator PlayMissionCutscene(int missionID)
    {

        if (missionID > 0 && missionID <= missionCams.Length)
        {
            GameObject missionCam = missionCams[missionID - 1];

            if (missionCam != null)
            {
                missionCam.SetActive(true);
                cam1.SetActive(false);
                canvas.SetActive(false);
                player.SetActive(false);

                // Tunggu durasi cutscene
                yield return new WaitForSeconds(3);

                missionCam.SetActive(false);
                cam1.SetActive(false);
                canvas.SetActive(true);
                player.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"Mission camera for ID {missionID} is not assigned!");
            }

        }

        else
        {
            Debug.LogWarning($"Invalid mission ID: {missionID}");
        }

    }
}
