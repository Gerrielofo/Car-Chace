using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MainMenuManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject _playerObj;
    [SerializeField] ActionBasedContinuousTurnProvider _actionBasedContinuousTurnProvider;
    [SerializeField] ActionBasedSnapTurnProvider _actionBasedSnapTurnProvider;
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

        _actionBasedContinuousTurnProvider = _playerObj.GetComponent<ActionBasedContinuousTurnProvider>();
        _actionBasedSnapTurnProvider = _playerObj.GetComponent<ActionBasedSnapTurnProvider>();
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

    public void SwitchTurnType(int value)
    {
        GameOptionsManager.Instance.turnType = value;
        if (value == 0)
        {
            _actionBasedSnapTurnProvider.enabled = true;
            _actionBasedContinuousTurnProvider.enabled = false;
        }
        else
        {
            _actionBasedSnapTurnProvider.enabled = false;
            _actionBasedContinuousTurnProvider.enabled = true;
        }
    }

    public void StartGame()
    {
        GameManager.Instance.isPlaying = true;
        SceneManager.LoadScene(GameManager.Instance.gameScene);
    }

    public void AddPoints(int amount)
    {
        if (amount > 0)
            GameManager.Instance.AddPoints(amount);
        else
        {
            Debug.LogError("Point Amount Not Set!");
        }
        FindObjectOfType<ShopManager>().UpdatePointCount();
    }


    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
