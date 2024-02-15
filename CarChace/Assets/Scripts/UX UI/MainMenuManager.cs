using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("GameOptions")]
    [SerializeField] TMP_Dropdown _timeDropdown;
    [SerializeField] TMP_Dropdown _healthDropdown;

    // Start is called before the first frame update
    void Start()
    {
        #region Audio
        _mainMixer.SetFloat("MasterVol", Mathf.Log10(PlayerPrefs.GetFloat("MasterVol")) * 20);
        _masterSlider.value = PlayerPrefs.GetFloat("MasterVol");

        _mainMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol")) * 20);
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVol");

        _mainMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVol")) * 20);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
        #endregion

        OnChangeTime();
        OnChangeHealth();
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

    #region GameOptions

    public void OnChangeTime()
    {
        string dropdownOption = _timeDropdown.options[_timeDropdown.value].text;
        string amountString = dropdownOption.Replace("min", "");

        int value;
        int.TryParse(amountString, out value);

        GameOptionsManager.Instance.timeLimitMinutes = value;
    }

    public void OnChangeHealth()
    {
        string dropdownOption = _healthDropdown.options[_healthDropdown.value].text;
        string amountString = dropdownOption.Replace("%", "");

        int value;
        int.TryParse(amountString, out value);

        GameOptionsManager.Instance.enemyHealthPrc = value;
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
