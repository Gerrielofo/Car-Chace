using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBtn : MonoBehaviour
{
    [SerializeField] int _powerUpAmount;
    [SerializeField] TMP_Text _powerUpAmountTxt;

    [SerializeField] Button _button;
    [SerializeField] CarController _carController;
    [SerializeField] PowerUp _powerUp;

    public void SetPowerUpButton(PowerUp powerUp, int powerUpAmount)
    {
        _powerUp = powerUp;
        _powerUpAmount = powerUpAmount;
        UpdateText();
        GetComponent<Image>().sprite = _powerUp.powerUpImage;

        switch (_powerUp.powerUpType)
        {
            case PowerUp.PowerUpType.SPEED:
                _carController = FindObjectOfType<CarController>();
                _button.onClick.AddListener(delegate { UseSpeedPowerup(); });
                break;
            case PowerUp.PowerUpType.SPIKE:
                _button.onClick.AddListener(delegate { UseSpikePowerup(); });
                break;
            case PowerUp.PowerUpType.HELICOPTER:
                _button.onClick.AddListener(delegate { UseHelicopterPowerup(); });
                break;
            case PowerUp.PowerUpType.REINFORCEMENTS:
                _button.onClick.AddListener(delegate { UseReinforcementsPowerup(); });
                break;
            default:
                Debug.LogError("Could Not Use Powerup Because Powerup Type Does Not Exist");
                break;
        }
    }

    void UseSpeedPowerup()
    {
        Debug.Log(" USE|ING SPEED");
        _powerUpAmount--;
        PlayerPrefs.SetInt(GameManager.Instance.speedPower, _powerUpAmount);
        _carController.SpeedBoost(_powerUp.powerUpAmount, _powerUp.powerUpDuration);
        if (_powerUpAmount < 1)
        {
            Destroy(gameObject);
        }
        UpdateText();
    }

    void UseSpikePowerup()
    {
        _powerUpAmount--;
        PlayerPrefs.SetInt(GameManager.Instance.spikePower, _powerUpAmount);

        // _carController.SpeedBoost(_powerUp.powerUpAmount, _powerUp.powerUpDuration);
        if (_powerUpAmount < 1)
        {
            Destroy(gameObject);
        }
        UpdateText();
    }

    void UseHelicopterPowerup()
    {
        _powerUpAmount--;
        PlayerPrefs.SetInt(GameManager.Instance.helicopterPower, _powerUpAmount);
        // _carController.SpeedBoost(_powerUp.powerUpAmount, _powerUp.powerUpDuration);
        if (_powerUpAmount < 1)
        {
            Destroy(gameObject);
        }
        UpdateText();
    }

    void UseReinforcementsPowerup()
    {
        _powerUpAmount--;
        PlayerPrefs.SetInt(GameManager.Instance.reinforcementPower, _powerUpAmount);

        // _carController.SpeedBoost(_powerUp.powerUpAmount, _powerUp.powerUpDuration);
        if (_powerUpAmount < 1)
        {
            Destroy(gameObject);
        }
        UpdateText();
    }

    void UpdateText()
    {
        _powerUpAmountTxt.text = _powerUpAmount.ToString();
    }


}
