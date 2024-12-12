using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public float volume = 1.0f;
    public Slider volumeSlider;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat("Volume", 1.0f);
        volumeSlider.value = volume;
        audioSource.volume = volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float newVolume)
    {
        audioSource.volume = newVolume;
        volume = newVolume;

        PlayerPrefs.SetFloat("Volume", volume);
        AudioManager.Instance.UpdateVolume(volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}
