﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VideoMenu : MonoBehaviour {

    public TMPro.TMP_Dropdown resolutionDropdown;

    public Toggle fullscreenToggle;

    public int currentResolutionIndex;

    Resolution[] resolutions;

    void Start ()
    {
        //Loads all player prefs except resolution
        LoadPrefs();

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if(isFullscreen)
        {
            PlayerPrefs.SetInt("FullscreenBool", 1);
            fullscreenToggle.isOn = true;
        }
        else
        {
            PlayerPrefs.SetInt("FullscreenBool", 0);
            fullscreenToggle.isOn = false;
        }
    }

    void CheckFullscreen(int num)
    {
        if(num == 1)
        {
            SetFullscreen(true);
            PlayerPrefs.SetInt("FullscreenBool", num);
            fullscreenToggle.isOn = true;
        }
        else
        {
            SetFullscreen(false);
            PlayerPrefs.SetInt("FullscreenBool", num);
            fullscreenToggle.isOn = false;
        }
    }

    void LoadPrefs()
    {
        CheckFullscreen(PlayerPrefs.GetInt("FullscreenBool"));
    }

}
