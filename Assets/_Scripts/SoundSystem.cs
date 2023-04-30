using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSystem : MonoBehaviour {

    public static SoundSystem INSTANCE {get; private set;}

    [Header("Sound Collection")]
    [SerializeField] private AudioClip musicLoop;
    [SerializeField] private AudioClip gameOverSFX;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip zombieGrowlSFX;
    [SerializeField] private AudioClip trapTriggerSFX;
    [SerializeField] private AudioClip bulletImpactSFX;
    [SerializeField] private AudioClip doorSFX;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    
    private const string masterVolume = "MasterVolume";
    private AudioSource audioSource;

    private void Awake() {

        audioSource = GetComponent<AudioSource>();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("SoundSystem instance created.");
        } else {
            Debug.LogError("More than one SoundSystem instance created.");
            Destroy(this);
            return;
        }

    }

    //Convert float from slider to a logarithemic scale for the master volume of the AudioMixer.
    public void SetVolume(float sliderValue) {
        audioMixer.SetFloat(masterVolume, Mathf.Log10(sliderValue) * 20f);
    }

    //Play a audio clip from source.
    public void PlaySFX(AudioClip audio, AudioSource source) {
        source.clip = audio;
        source.loop = false;
        source.Play();
    }

    //Loop a audio clip as BGM from source.
    public void PlayMusic(AudioClip audio) {
        audioSource.clip = audio;
        audioSource.loop = true;
        audioSource.Play();
    }

    //Getters.
    public AudioClip GetMusicLoop() {
        return musicLoop;
    }

    public AudioClip GetGameOverSFX() {
        return gameOverSFX;
    }

    public AudioClip GetShootSFX() {
        return shootSFX;
    }

    public AudioClip GetZombieGrowlSFX() {
        return zombieGrowlSFX;
    }

    public AudioClip GetTrapTriggerSFX() {
        return trapTriggerSFX;
    }

    public AudioClip GetBulletImpactSFX() {
        return bulletImpactSFX;
    }

    public AudioClip GetDoorSFX() {
        return doorSFX;
    }

    public AudioSource GetAudioSourceSFX() {
        return audioSource;
    }

}
