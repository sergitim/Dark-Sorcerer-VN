using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SettingsUI : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Button returnToTitleScreenButton;
    public Image backgroundScreen;
    public SaveLoadSystem sls;
    public Slider musicVolumeSlider;
    public AudioSource AS;

    private Resolution[] resolutions;

    private void Start()
    {


        sls.LoadSettings();

        RectTransform backgroundRectTransform = backgroundScreen.rectTransform;

        backgroundRectTransform.anchorMin = Vector2.zero;
        backgroundRectTransform.anchorMax = Vector2.one;
        backgroundRectTransform.offsetMin = Vector2.zero;
        backgroundRectTransform.offsetMax = Vector2.zero;




        // Populate the resolution dropdown list
        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 10; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Set the fullscreen toggle to the current fullscreen state
        fullscreenToggle.isOn = Screen.fullScreen;

        // Add an event listener to the fullscreen toggle
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // Add listener to the resolution dropdown
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // Add an event listener to the return to title screen button
        returnToTitleScreenButton.onClick.AddListener(OnReturnToTitleScreenButtonClicked);



        //Music slider
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        float aspectRatio = (float)resolution.width / resolution.height;
        aspectRatio = Mathf.Clamp(aspectRatio, 4f / 3f, 16f / 9f);
        resolution.width = Mathf.RoundToInt(resolution.height * aspectRatio);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (isFullscreen)
        {
            resolutionDropdown.interactable = false;
        }
        else
        {
            resolutionDropdown.interactable = true;
            SetResolution(resolutionDropdown.value);
            Screen.fullScreen = false; // Add this line to disable full screen when toggle is turned off
        }
    }

    public void SetMusicVolume(float volume)
    {
        // Set the volume of the music using the provided volume value
        // You can use this value to control the volume of the music in your audio system
        // For example:
        AS.volume = musicVolumeSlider.value;
        
    }

    public void ReturnToTitleScreen()
    {
        sls.SaveSettings();
        SceneManager.LoadScene("TitleScreen");

    }

    public void OnReturnToTitleScreenButtonClicked()
    {
        ReturnToTitleScreen();
    }
}