using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Upgrades")]
    public int speedIndex;
    public int damageIndex;
    public int carModIndex;
    public ShopUpgrade[] speedUpgrades;
    public ShopUpgrade[] damageUpgrades;
    public ShopUpgrade[] carModUpgrades;

    [Header("UI")]
    [SerializeField] TMP_Text _pointsTxt;
    [SerializeField] Sprite _activeImage;
    [SerializeField] Sprite _inactiveImage;

    [SerializeField] Image[] _speedImages;
    [SerializeField] Image[] _damageImages;
    [SerializeField] Image[] _carModImages;

    [SerializeField] TMP_Text _speedCostTxt;
    [SerializeField] TMP_Text _damageCostTxt;
    [SerializeField] TMP_Text _carModCostTxt;

    private void Start()
    {
        speedIndex = PlayerPrefs.GetInt("speedIndex");
        damageIndex = PlayerPrefs.GetInt("damageIndex");
        carModIndex = PlayerPrefs.GetInt("carModIndex");


        UpdatePointCount();
        UpdateImages();
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
