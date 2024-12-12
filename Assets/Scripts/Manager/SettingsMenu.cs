using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    // Must testing in the build project

    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    void Start()
    {

        // Initialize Resolutions

        resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set up Quality Settings

        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string> { "Very Low", "Low", "Medium", "High", "Very High" };
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        // Set up Fullscreen Toggle

        fullscreenToggle.isOn = Screen.fullScreen;

        // Add listeners for UI changes

        resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
        qualityDropdown.onValueChanged.AddListener(delegate { SetQuality(qualityDropdown.value); });
        fullscreenToggle.onValueChanged.AddListener(delegate { SetFullscreen(fullscreenToggle.isOn); });

    }


    // change all of the settings

    public void SetResolution(int resolutionIndex)
    {
        Debug.Log("Setting resolution to: " + resolutions[resolutionIndex].width + "x" + resolutions[resolutionIndex].height);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        Debug.Log("Setting quality to level: " + qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Debug.Log("Setting fullscreen to: " + isFullscreen);
        Screen.fullScreen = isFullscreen;
    }

}
