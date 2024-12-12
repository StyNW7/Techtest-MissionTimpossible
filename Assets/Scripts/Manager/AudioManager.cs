using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource audioSource;

    [System.Serializable]
    public struct SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    public List<SceneMusic> sceneMusicMappings;

    private string currentSceneName = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        float volume = PlayerPrefs.GetFloat("Volume", 1.0f);
        audioSource.volume = volume;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = scene.name;

        if (newSceneName != currentSceneName)
        {
            currentSceneName = newSceneName;
            PlaySceneMusic(newSceneName);
        }
    }

    private void PlaySceneMusic(string sceneName)
    {
        audioSource.Stop();

        foreach (SceneMusic mapping in sceneMusicMappings)
        {
            if (mapping.sceneName == sceneName)
            {
                if (mapping.musicClip != null)
                {
                    audioSource.clip = mapping.musicClip;
                    audioSource.Play();
                }
                return;
            }
        }

        // Default audio = no audio
        audioSource.clip = null;

    }

    public void UpdateVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("Volume", newVolume);
        audioSource.volume = newVolume;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (newClip != null && newClip != audioSource.clip)
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}
