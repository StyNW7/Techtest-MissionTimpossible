using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        //Quit Build: Application.Quit();
    }
}
