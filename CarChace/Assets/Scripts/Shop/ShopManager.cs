using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Upgrades")]
    #region  Upgrades

    public int speedIndex;
    public int damageIndex;
    public int carModIndex;

    public ShopUpgrade[] speedUpgrades;
    public ShopUpgrade[] damageUpgrades;
    public ShopUpgrade[] carModUpgrades;

    #endregion

    [Header("Power Ups")]
    #region PowerUps

    [SerializeField] int _maxPowerUps = 5;

    [SerializeField] PowerUp[] _powerUps;
    [SerializeField] int[] _powerUpAmounts;

    [SerializeField] int _speedPowerUpAmount;
    [SerializeField] int _spikePowerUpAmount;
    [SerializeField] int _helicopterPowerUpAmount;
    [SerializeField] int _reinforcementPowerUpAmount;

    #endregion
    [Header("UI")]
    #region UI

    [SerializeField] TMP_Text _pointsTxt;
    [SerializeField] Sprite _activeImage;
    [SerializeField] Sprite _inactiveImage;

    [SerializeField] Image[] _speedImages;
    [SerializeField] Image[] _damageImages;
    [SerializeField] Image[] _carModImages;

    [SerializeField] TMP_Text _speedCostTxt;
    [SerializeField] TMP_Text _damageCostTxt;
    [SerializeField] TMP_Text _carModCostTxt;

    [SerializeField] TMP_Text _maxPowerUpsTxt;

    [SerializeField] TMP_Text[] _powerUpAmountTxt;
    [SerializeField] TMP_Text[] _powerupCostTxt;

    #endregion

    private void Start()
    {
        speedIndex = PlayerPrefs.GetInt("speedIndex");
        damageIndex = PlayerPrefs.GetInt("damageIndex");
        carModIndex = PlayerPrefs.GetInt("carModIndex");

        _speedPowerUpAmount = PlayerPrefs.GetInt(GameManager.Instance.speedPower);
        _spikePowerUpAmount = PlayerPrefs.GetInt(GameManager.Instance.spikePower);
        _helicopterPowerUpAmount = PlayerPrefs.GetInt(GameManager.Instance.helicopterPower);
        _reinforcementPowerUpAmount = PlayerPrefs.GetInt(GameManager.Instance.reinforcementPower);

        UpdatePowerUpText();
        UpdatePointCount();
        UpdateImages();
        UpdateCostText();
    }

    public void BuyPowerUp(int index)
    {
        PowerUp powerUpToBuy = _powerUps[index];
        if (powerUpToBuy.powerUpPrice > GameManager.Instance.points)
        {
            Debug.Log("Could Not Afford PowerUp");
            return;
        }
        else if (GetTotalPowerUpAmount() >= _maxPowerUps)
        {
            Debug.Log("Reached Maximum PowerUps!");
            return;
        }

        switch (powerUpToBuy.powerUpType)
        {
            case PowerUp.PowerUpType.SPEED:
                _speedPowerUpAmount++;
                PlayerPrefs.SetInt(GameManager.Instance.speedPower, _speedPowerUpAmount);
                break;
            case PowerUp.PowerUpType.SPIKE:
                _spikePowerUpAmount++;
                PlayerPrefs.SetInt(GameManager.Instance.spikePower, _spikePowerUpAmount);
                break;
            case PowerUp.PowerUpType.HELICOPTER:
                _helicopterPowerUpAmount++;
                PlayerPrefs.SetInt(GameManager.Instance.helicopterPower, _helicopterPowerUpAmount);
                break;
            case PowerUp.PowerUpType.REINFORCEMENTS:
                _reinforcementPowerUpAmount++;
                PlayerPrefs.SetInt(GameManager.Instance.reinforcementPower, _reinforcementPowerUpAmount);
                break;
            default:
                Debug.LogError("PowerUpType Not Found");
                break;
        }
        GameManager.Instance.UsePoints(powerUpToBuy.powerUpPrice);
        UpdatePowerUpText();
    }

    public void ResetPowerUps()
    {
        PlayerPrefs.SetInt(GameManager.Instance.speedPower, 0);
        PlayerPrefs.SetInt(GameManager.Instance.spikePower, 0);
        PlayerPrefs.SetInt(GameManager.Instance.helicopterPower, 0);
        PlayerPrefs.SetInt(GameManager.Instance.reinforcementPower, 0);

        _speedPowerUpAmount = 0;
        _spikePowerUpAmount = 0;
        _helicopterPowerUpAmount = 0;
        _reinforcementPowerUpAmount = 0;

        UpdatePowerUpText();
    }

    void UpdateCostText()
    {
        for (int i = 0; i < _powerupCostTxt.Length; i++)
        {
            _powerupCostTxt[i].text = "COST :" + _powerUps[i].powerUpPrice.ToString();
        }
    }

    void UpdatePowerUpText()
    {
        _powerUpAmounts[0] = _speedPowerUpAmount;
        _powerUpAmounts[1] = _spikePowerUpAmount;
        _powerUpAmounts[2] = _helicopterPowerUpAmount;
        _powerUpAmounts[3] = _reinforcementPowerUpAmount;

        for (int i = 0; i < _powerUpAmountTxt.Length; i++)
        {
            _powerUpAmountTxt[i].text = "OWNED: " + _powerUpAmounts[i].ToString();
        }

        _maxPowerUpsTxt.text = "POWER-UPS: " + GetTotalPowerUpAmount() + "/" + _maxPowerUps.ToString();
        UpdatePointCount();
    }

    int GetTotalPowerUpAmount()
    {
        int total = 0;
        for (int i = 0; i < _powerUpAmounts.Length; i++)
        {
            total += _powerUpAmounts[i];
        }
        return total;
    }

    public void UpgradeSpeed(int id)
    {
        if (speedIndex == speedUpgrades.Length || speedIndex >= id || (id - speedIndex) > 1)
            return;
        if (GameManager.Instance.CanAfford(speedUpgrades[speedIndex].pointCost))
        {
            speedIndex = id;
            UpdateImages();
            GameManager.Instance.UsePoints(speedUpgrades[speedIndex - 1].pointCost);
            UpdatePointCount();
            PlayerPrefs.SetInt("speedIndex", speedIndex);
        }
        else
        {
            Debug.Log("Don't Have Enough Points For This Upgrade!");
        }
    }

    public void UpgradeDamage(int id)
    {
        if (damageIndex == damageUpgrades.Length || damageIndex >= id || (id - damageIndex) > 1)
            return;
        if (GameManager.Instance.CanAfford(damageUpgrades[damageIndex].pointCost))
        {
            damageIndex = id;
            UpdateImages();
            GameManager.Instance.UsePoints(damageUpgrades[damageIndex - 1].pointCost);
            UpdatePointCount();
            PlayerPrefs.SetInt("damageIndex", damageIndex);
        }
        else
        {
            Debug.Log("Don't Have Enough Points For This Upgrade!");
        }
    }

    public void UpgradeCarMod(int id)
    {
        if (carModIndex == carModUpgrades.Length || carModIndex >= id || (id - carModIndex) > 1)
            return;
        if (GameManager.Instance.CanAfford(carModUpgrades[carModIndex].pointCost))
        {
            carModIndex = id;
            UpdateImages();
            GameManager.Instance.UsePoints(carModUpgrades[carModIndex - 1].pointCost);
            UpdatePointCount();
            PlayerPrefs.SetInt("carModIndex", carModIndex);
        }
        else
        {
            Debug.Log("Don't Have Enough Points For This Upgrade!");
        }
    }

    public void ResetUpgrades()
    {
        PlayerPrefs.SetInt("speedIndex", 0);
        PlayerPrefs.SetInt("damageIndex", 0);
        PlayerPrefs.SetInt("carModIndex", 0);

        speedIndex = 0;
        damageIndex = 0;
        carModIndex = 0;

        UpdateImages();
        UpdatePointCount();
    }

    void UpdateImages()
    {
        for (int i = 0; i < _speedImages.Length; i++)
        {
            if (i < speedIndex)
                _speedImages[i].sprite = _activeImage;
            else
            {
                _speedImages[i].sprite = _inactiveImage;
            }
        }

        for (int i = 0; i < _damageImages.Length; i++)
        {
            if (i < damageIndex)
                _damageImages[i].sprite = _activeImage;
            else
            {
                _damageImages[i].sprite = _inactiveImage;
            }
        }

        for (int i = 0; i < _carModImages.Length; i++)
        {
            if (i < carModIndex)
                _carModImages[i].sprite = _activeImage;
            else
            {
                _carModImages[i].sprite = _inactiveImage;
            }
        }
    }

    void UpdatePointCount()
    {
        _pointsTxt.text = "Points: " + GameManager.Instance.points;

        if (speedIndex < 3)
        {
            _speedCostTxt.text = "Cost: " + speedUpgrades[speedIndex].pointCost;
        }
        else
        {
            _speedCostTxt.text = "Max Uprgade";
        }
        if (damageIndex < 3)
        {
            _damageCostTxt.text = "Cost: " + damageUpgrades[damageIndex].pointCost;

        }
        else
        {
            _damageCostTxt.text = "Max Uprgade";
        }
        if (carModIndex < 3)
        {
            _carModCostTxt.text = "Cost: " + carModUpgrades[carModIndex].pointCost;
        }
        else
        {
            _carModCostTxt.text = "Max Uprgade";
        }
    }
}
