using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound Elements")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource effectSound;

    [Space]
    [Header("Sound Data")]
    [SerializeField] private AudioClip[] backgroundMusics;
    [SerializeField] private AudioClip[] clickSounds;
    [SerializeField] private AudioClip[] failedSounds;
    [SerializeField] private AudioClip[] winGameSounds;
    [SerializeField] private AudioClip[] gameOverSounds;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PlayBackgroundMusic();
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayClickSound()
    {
        int i = Random.Range(0, clickSounds.Length);
        effectSound.clip = clickSounds[i];
        effectSound.Play();
    }

    public void PlayFailedSound()
    {
        int i = Random.Range(0, failedSounds.Length);
        effectSound.clip = failedSounds[i];
        effectSound.Play();
    }

    public void PlayWinGameSound()
    {
        int i = Random.Range(0, winGameSounds.Length);
        effectSound.clip = winGameSounds[i];
        effectSound.Play();
    }

    public void PlayGameOverSound()
    {
        int i = Random.Range(0, gameOverSounds.Length);
        effectSound.clip = gameOverSounds[i];
        effectSound.Play();
    }

    public void PlayBackgroundMusic()
    {
        Debug.Log("Start");
        int i = Random.Range(0, backgroundMusics.Length);
        backgroundMusic.clip = backgroundMusics[0];
        backgroundMusic.Play();
    }

    public void ChangeBackgroundMusicVolume(float value)
    {
        backgroundMusic.volume = value;
    }

    public void ChangeEffectsSoundVolume(float value)
    {
        effectSound.volume = value;
    }
}
