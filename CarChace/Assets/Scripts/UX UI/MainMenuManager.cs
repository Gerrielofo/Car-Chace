using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioMixer _mainMixer;
    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        _mainMixer.SetFloat("MasterVol", Mathf.Log10(PlayerPrefs.GetFloat("MasterVol")) * 20);
        _masterSlider.value = PlayerPrefs.GetFloat("MasterVol");

        _mainMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol")) * 20);
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVol");

        _mainMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVol")) * 20);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Audio

    public void ChangeMasterVol(float prc)
    {
        _mainMixer.SetFloat("MasterVol", Mathf.Log10(prc) * 20);
        PlayerPrefs.SetFloat("MasterVol", prc);
    }

    public void ChangeMusicVol(float prc)
    {
        _mainMixer.SetFloat("MusicVol", Mathf.Log10(prc) * 20);
        PlayerPrefs.SetFloat("MusicVol", prc);
    }

    public void ChangeSFXVol(float prc)
    {
        _mainMixer.SetFloat("SFXVol", Mathf.Log10(prc) * 20);
        PlayerPrefs.SetFloat("SFXVol", prc);
    }

    #endregion




    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
