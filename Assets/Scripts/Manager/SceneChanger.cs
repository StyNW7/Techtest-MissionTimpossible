using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    [SerializeField] public string sceneName;
    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log("Change Scene");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is not set. Please provide a valid scene name.");
        }
    }
}
