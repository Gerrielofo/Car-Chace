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

    public void UpgradeSpeed()
    {
        if (speedIndex == speedUpgrades.Length)
            return;
        if (GameManager.Instance.CanAfford(speedUpgrades[speedIndex + 1].pointCost))
        {
            speedIndex++;
            UpdateImages();
            GameManager.Instance.UsePoints(speedUpgrades[speedIndex + 1].pointCost);
            UpdatePointCount();
            PlayerPrefs.SetInt("speedIndex", speedIndex);
        }
        else
        {
            Debug.Log("Don't Have Enough Points For This Upgrade!");
        }
    }

    public void UpgradeDamage()
    {
        if (damageIndex == damageUpgrades.Length)
            return;
        if (GameManager.Instance.CanAfford(damageUpgrades[damageIndex + 1].pointCost))
        {
            damageIndex++;
            UpdateImages();
            GameManager.Instance.UsePoints(damageUpgrades[damageIndex + 1].pointCost);
            UpdatePointCount();
            PlayerPrefs.SetInt("damageIndex", damageIndex);
        }
        else
        {
            Debug.Log("Don't Have Enough Points For This Upgrade!");
        }
    }

    public void UpgradeCarMod()
    {
        if (carModIndex == carModUpgrades.Length)
            return;
        if (GameManager.Instance.CanAfford(carModUpgrades[carModIndex + 1].pointCost))
        {
            carModIndex++;
            UpdateImages();
            GameManager.Instance.UsePoints(carModUpgrades[carModIndex + 1].pointCost);
            UpdatePointCount();
            PlayerPrefs.SetInt("carModIndex", carModIndex);
        }
        else
        {
            Debug.Log("Don't Have Enough Points For This Upgrade!");
        }
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt("speedIndex", 0);
        PlayerPrefs.SetInt("damageIndex", 0);
        PlayerPrefs.SetInt("carModIndex", 0);

        UpdateImages();
        UpdatePointCount();
    }

    void UpdateImages()
    {
        for (int i = 0; i < _speedImages.Length; i++)
        {
            if (i <= speedIndex)
                _speedImages[speedIndex].sprite = _activeImage;
            else
            {
                _speedImages[speedIndex].sprite = _inactiveImage;
            }
        }

        for (int i = 0; i < _damageImages.Length; i++)
        {
            if (i <= damageIndex)
                _damageImages[damageIndex].sprite = _activeImage;
            else
            {
                _damageImages[damageIndex].sprite = _inactiveImage;
            }
        }

        for (int i = 0; i < _carModImages.Length; i++)
        {
            if (i <= carModIndex)
                _carModImages[carModIndex].sprite = _activeImage;
            else
            {
                _carModImages[carModIndex].sprite = _inactiveImage;
            }
        }
    }

    void UpdatePointCount()
    {
        _pointsTxt.text = "Points: " + GameManager.Instance.points;

        _speedCostTxt.text = "Cost: " + speedUpgrades[speedIndex].pointCost;
        _damageCostTxt.text = "Cost: " + damageUpgrades[damageIndex].pointCost;
        _carModCostTxt.text = "Cost: " + carModUpgrades[carModIndex].pointCost;
    }
}
