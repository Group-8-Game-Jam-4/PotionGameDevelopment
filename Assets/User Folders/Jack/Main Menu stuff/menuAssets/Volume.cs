using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeMenu : MonoBehaviour {

    public AudioMixer masterMixer;

    public Slider master, music, effects;

    void Start ()
    {
        //Loads all player prefs
        LoadPrefs();
    }

    public void SetEffectsVolume (float volume)
    {
        masterMixer.SetFloat("SfxVolume", volume);
        PlayerPrefs.SetFloat("SfxVolume", volume);
        Debug.Log(volume);
    }

    public void SetMusicVolume (float volume)
    {
        masterMixer.SetFloat("BgmVolume", volume);
        PlayerPrefs.SetFloat("BgmVolume", volume);
        Debug.Log(volume);
    }

    public void SetMasterVolume (float volume)
    {
        masterMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        Debug.Log(volume);
    }

    void LoadPrefs()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume");
        music.value = PlayerPrefs.GetFloat("BgmVolume");
        effects.value = PlayerPrefs.GetFloat("SfxVolume");
    }

}
