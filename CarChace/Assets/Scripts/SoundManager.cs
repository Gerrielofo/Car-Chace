using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource masterAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    [SerializeField] bool _isMainMenuManager;

    [Header("Music")]
    [SerializeField] AudioClip _mainMenuMusic;
    [SerializeField] AudioClip _gameMusic;
    [SerializeField] AudioClip _endGameMusic;

    [Header("Menu Sounds")]
    [SerializeField] AudioClip _clickSound;
    [SerializeField] AudioClip _scrollSound;

    #region Play Sounds

    public void PlayMusic()
    {
        if (!GameManager.Instance.isPlaying)
        {
            musicAudioSource.clip = _endGameMusic;
            musicAudioSource.Play();
            return;
        }

        if (_isMainMenuManager)
        {
            musicAudioSource.clip = _mainMenuMusic;
        }
        else
        {
            musicAudioSource.clip = _gameMusic;
        }
        musicAudioSource.Play();

    }

    public void ClickSound()
    {
        sfxAudioSource.clip = _clickSound;
        sfxAudioSource.Play();
    }

    public void ScrollSound()
    {
        sfxAudioSource.clip = _scrollSound;
        sfxAudioSource.Play();
    }


    #endregion


}
