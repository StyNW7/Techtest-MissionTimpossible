using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        // Memuat scene LoadingScreen terlebih dahulu
        SceneManager.LoadScene("LoadingScreen");

        // Menyimpan nama scene tujuan di PlayerPrefs agar bisa diakses oleh LoadingScreen
        PlayerPrefs.SetString("TargetScene", sceneName);
    }
}
