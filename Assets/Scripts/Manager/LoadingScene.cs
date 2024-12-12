using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;  // Reference untuk Progress Bar
    [SerializeField] private Text loadingText;    // Reference untuk teks Loading

    public void StartLoading(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private void Start()
    {
        string targetScene = PlayerPrefs.GetString("TargetScene");
        StartLoading(targetScene);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Memulai pemuatan scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        // Update progress bar dan teks selama proses loading
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressBar != null)
            {
                progressBar.value = progress; // Update progress bar
            }
            if (loadingText != null)
            {
                loadingText.text = "Loading... " + (progress * 100).ToString("F0") + "%";
            }

            // Jika loading selesai, aktifkan scene baru
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
