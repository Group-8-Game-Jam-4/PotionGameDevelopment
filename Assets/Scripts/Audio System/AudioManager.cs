using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public Sound[] music;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup soundMixer;
    public float fadeDuration = 1.0f; // Duration for fading out in seconds
    Scene currentScene;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        //makes it a singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        


        //add sounds to audio sources with their varying info
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.dopplerLevel = s.dopplerLevel;
            s.source.spatialBlend = s.spatialBlend;
            s.source.outputAudioMixerGroup = s.outputAudioMixerGroup;
        }

        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
            m.source.dopplerLevel = m.dopplerLevel;
            m.source.spatialBlend = m.spatialBlend;
            m.source.outputAudioMixerGroup = m.outputAudioMixerGroup;
        }
        //this could be used to play music when a scene loads
        PlayMusic("titleMusic"); //this will play the music sound with the name BGM
        currentScene = SceneManager.GetActiveScene();
        //SceneMusic(); alter this function to play different background music depending on scene
    }


    void Update()
    {
        if(SceneManager.GetActiveScene() != currentScene)
        {
            SceneMusic();
        }
    }

    //DANGER THIS WILL STOP ALL BUT ONESHOT SOUNDS BECAUSE OF STOPALL
    public void SceneMusic()
    {
        Scene scene = SceneManager.GetActiveScene();
        currentScene = scene;

        if(scene.name == "MainMenu Test")
        {
            CrossFadeMusic("titleMusic");
        }
        if(scene.name == "MorganScene")
        {
            CrossFadeMusic("inGameMusic");
        }
    }

    //this could be used to play music when a scene loads

    //plays oneshot sound. Use for overlapping audio such as gunshots
    public void PlayOneShotSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //custom error message when cant find clip name
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.PlayOneShot(s.clip, s.volume);
    }

    //plays sound
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //custom error message when cant find clip name
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        Debug.Log("Playing sound");
        s.source.Play();
    }

    // stop all sounds and play a specific one
    public void ForcePlaySound(string name)
    {
        foreach (Sound so in sounds)
        {
            if(so != null)
            {
                so.source.Stop();
            }
        }

        Sound s = Array.Find(sounds, sound => sound.name == name);
        //custom error message when cant find clip name
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        Debug.Log("Playing sound");
        s.source.Play();
    }

    // play music
    public void PlayMusic(string name)
    {
        Sound m = Array.Find(music, sound => sound.name == name);
        //custom error message when cant find clip name
        if (m == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        Debug.Log("Playing music");
        m.source.Play();
    }

    // stop all sounds and music (NOT ONESHOT SOUNDS)
    public void StopAll()
    {
        Component[] sources;
        sources = GetComponentsInChildren<AudioSource>();
        foreach(AudioSource tSource in sources){
            tSource.Stop();
        }
    }

    // stop a specific sound or piece of music
    public void StopSpecific(string name)
    {
        Sound m = Array.Find(music, sound => sound.name == name);
        //custom error message when cant find clip name
        if (m == null)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            //custom error message when cant find clip name
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            else
            {
                Debug.Log("Stopping sound");
                s.source.Stop();
            }
        }
        else
        {
            Debug.Log("Stopping music");
            m.source.Stop();
        }
    }



    void CrossFadeMusic(string musicName)
    {
        FadeOutAllMusic(musicName);
    }

    // Function to gradually fade out and stop all music audio sources
    public void FadeOutAllMusic(string musicName)
    {
        foreach (Sound m in music)
        {
            if (m != null && m.source != null)
            {
                StartCoroutine(FadeOutAndStop(m.source, musicName));
            }
        }
    }

    public void FadeOutAllMusic()
    {
        foreach (Sound m in music)
        {
            if (m != null && m.source != null)
            {
                StartCoroutine(FadeOutAndStop(m.source));
            }
        }
    }

    // Coroutine to gradually fade out the volume of an audio source and then stop it and play a new piece of music
    private IEnumerator FadeOutAndStop(AudioSource audioSource, string musicName)
    {
        float startVolume = audioSource.volume;

        // Gradually decrease volume over fadeDuration
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        // Ensure volume is 0 before stopping
        audioSource.volume = 0;

        // Stop the audio
        audioSource.Stop();

        // Reset volume to original
        audioSource.volume = startVolume;

        Sound m = Array.Find(music, sound => sound.name == musicName);
        startVolume = m.source.volume;
        m.source.volume = 0;

        PlayMusic(musicName);

        // Gradually increase volume over fadeDuration
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
    }

    // overload that doesent play a new song
    private IEnumerator FadeOutAndStop(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;

        // Gradually decrease volume over fadeDuration
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        // Ensure volume is 0 before stopping
        audioSource.volume = 0;

        // Stop the audio
        audioSource.Stop();

        // Reset volume to original
        audioSource.volume = startVolume;
    }


    //to play from anywhere do
    //FindObjectOfType<AudioManager>().PlaySound("SoundName")

    //to edit volume from another script do
    //AudioManager manager = FindObjectOfType<AudioManager>();
    //audioMixer.SetFloat("BgmVolume", volume);
}