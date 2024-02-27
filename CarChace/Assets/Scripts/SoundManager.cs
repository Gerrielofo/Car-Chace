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

    [Header("Music")]
    [SerializeField] AudioClip _mainMenuMusic;
    [SerializeField] AudioClip _gameMusic;
    [SerializeField] AudioClip _endGameMusic;

    [Header("Menu Sounds")]
    [SerializeField] AudioClip _clickSound;
    [SerializeField] AudioClip _scrollSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Found Multiple SoundManagers In Scene. Destroyed The One On " + gameObject.name);
            Destroy(this);
        }

        SceneManager.activeSceneChanged += PlayMusic;
    }



    #region Play Sounds

    public void PlayMusic(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "Game")
        {
            if (!GameManager.Instance.isPlaying)
            {
                musicAudioSource.clip = _endGameMusic;
                musicAudioSource.Play();
                return;
            }
            musicAudioSource.clip = _gameMusic;
            musicAudioSource.Play();
        }
        else if (newScene.name == "Main Menu")
        {
            musicAudioSource.clip = _mainMenuMusic;
            musicAudioSource.Play();
        }
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
