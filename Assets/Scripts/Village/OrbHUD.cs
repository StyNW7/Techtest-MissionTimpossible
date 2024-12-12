using System.Collections;
using UnityEngine;

public class OrbHUD : MonoBehaviour
{

    [SerializeField] private GameObject missionHUD;
    [SerializeField] private GameObject missionDoneHUD;
    private bool isPaused = false;

    private void Start()
    {
        missionHUD.SetActive(false);
        missionDoneHUD.SetActive(false);
    }

    public void ShowMissionHUD(float duration, System.Action onComplete)
    {
        StartCoroutine(ShowHUDAndHide(duration, onComplete));
    }

    public void ShowMissionHUD2(float duration)
    {
        StartCoroutine(ShowHUDAndHide2(duration));
    }

    private IEnumerator ShowHUDAndHide(float duration, System.Action onComplete)
    {
        missionHUD.SetActive(true);
        PauseGame();

        yield return new WaitForSecondsRealtime(duration); // Tunggu waktu dengan pause aktif

        missionHUD.SetActive(false);
        ResumeGame();

        onComplete?.Invoke(); // Panggil callback setelah HUD selesai
    }

    private IEnumerator ShowHUDAndHide2(float duration)
    {
        missionDoneHUD.SetActive(true);
        PauseGame();

        yield return new WaitForSecondsRealtime(duration);

        missionDoneHUD.SetActive(false);
        ResumeGame();
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

}
