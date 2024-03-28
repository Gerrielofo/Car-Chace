using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class CarManager : MonoBehaviour
{
    [SerializeField] CarController _carController;

    [Header("Upgrades")]
    [SerializeField] ShopUpgrade[] _speedUpgrades;
    [SerializeField] ShopUpgrade[] _damageUpgrades;
    [SerializeField] ShopUpgrade[] _carModUpgrades;
    int _speedIndex;
    int _damageIndex;
    int _carModIndex;

    [Header("PowerUps")]
    int _speedPowerUpAmount;
    int _spikePowerUpAmount;
    int _helicopterPowerUpAmount;
    int _reinforcementPowerUpAmount;

    [Header("UI")]
    [SerializeField] Transform _powerUpParent;
    [SerializeField] GameObject _powerUpBtnPrefab;

    public PowerUp[] _powerUps;
    public List<int> _powerUpAmounts = new();

    // Start is called before the first frame update
    void Start()
    {
        _carController = FindObjectOfType<CarController>();

        _speedIndex = PlayerPrefs.GetInt("speedIndex");
        _damageIndex = PlayerPrefs.GetInt("damageIndex");
        _carModIndex = PlayerPrefs.GetInt("carModIndex");

        if (GameManager.Instance)
        {
            _speedPowerUpAmount = PlayerPrefs.GetInt(GameManager.Instance.speedPower);
            _spikePowerUpAmount = PlayerPrefs.GetInt(GameManager.Instance.spikePower);
            _helicopterPowerUpAmount = PlayerPrefs.GetInt(GameManager.Instance.helicopterPower);
            _reinforcementPowerUpAmount = PlayerPrefs.GetInt(GameManager.Instance.reinforcementPower);
        }

        AddPowerUps();

        if (_speedIndex > 0)
        {
            _carController.SetSpeedMultiplier(_speedUpgrades[_speedIndex - 1].upgradePrc);
        }
        if (_damageIndex > 0)
        {
            _carController.SetDamageMutliplier(_damageUpgrades[_damageIndex - 1].upgradePrc);
        }
        RefreshPowerUpUI();
    }

    public void RefreshPowerUpUI()
    {
        if (_powerUpParent.childCount > 0)
        {
            for (int i = 0; i < _powerUpParent.childCount; i++)
            {
                Destroy(_powerUpParent.GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < _powerUps.Length; i++)
        {
            if (_powerUpAmounts[i] > 0)
            {
                GameObject tempBtn = Instantiate(_powerUpBtnPrefab, _powerUpParent);
                tempBtn.GetComponent<PowerUpBtn>().SetPowerUpButton(_powerUps[i], _powerUpAmounts[i]);
            }
        }
    }

    void AddPowerUps()
    {
        _powerUpAmounts.Add(_speedPowerUpAmount);
        _powerUpAmounts.Add(_spikePowerUpAmount);
        _powerUpAmounts.Add(_helicopterPowerUpAmount);
        _powerUpAmounts.Add(_reinforcementPowerUpAmount);
    }

    public void MainMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }
}
