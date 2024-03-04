using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider soundSlider;
    [SerializeField] TMP_Dropdown dropdown1;
    [SerializeField] TMP_Dropdown dropdown2;
    [SerializeField] Toggle toggle;

    float AudioVolume;
    int fullSave;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        dropdown2.ClearOptions();
        List<string> options = new List<string>();


        for (int i = 0; i < resolutions.Length; i++) 
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
        }
        dropdown2.AddOptions(options);
        dropdown2.value = PlayerPrefs.GetInt("ResInt");
        dropdown2.RefreshShownValue();

        Resolution resolution = resolutions[PlayerPrefs.GetInt("ResInt")];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        soundSlider.value = PlayerPrefs.GetFloat("musicVolume");
        dropdown1.value = PlayerPrefs.GetInt("qualityInt");
        AudioVolume = PlayerPrefs.GetFloat("musicVolume");
        fullSave = PlayerPrefs.GetInt("fullScreen");
        

        audioMixer.SetFloat("volume", AudioVolume);
        if (fullSave == 1)
        {
            Screen.fullScreen = true;
            toggle.isOn = true;
        }
        else
        {
            Screen.fullScreen = false;
            toggle.isOn = false;
        }
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityInt", qualityIndex);
    }

    public void setFullscreen(bool isFullscreen)
    {
        if (isFullscreen == true) 
        {
            fullSave = 1;
        }
        else
        {
            fullSave = 0;
        }
        PlayerPrefs.SetInt("fullScreen", fullSave);
        Screen.fullScreen = isFullscreen;
        
    }

    public void SetResolution(int resolutionIndex)
    {
        PlayerPrefs.SetInt("ResInt", resolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
