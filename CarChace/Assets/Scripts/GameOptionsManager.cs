using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsManager : MonoBehaviour
{
    public static GameOptionsManager Instance;

    public int timeLimitMinutes;
    public int enemyHealthPrc;

    public int turnType;
    [SerializeField] Transform _powerUpParent;
    [SerializeField] GameObject _powerUpBtnPrefab;
    [SerializeField] List<GameObject> _powerUpBtns = new();

    [SerializeField] TMP_Text[] _powerUpAmountTxt;

    public PowerUp[] _powerUps;
    public int[] _powerUpAmounts;

    [SerializeField] CarController _carController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"Found A Second Instance! Destroyed The One On {gameObject.name}");
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void RefreshPowerUpUI()
    {
        for (int i = 0; i < _powerUps.Length; i++)
        {
            _powerUpBtns.Add(Instantiate(_powerUpBtnPrefab, _powerUpParent));

            _powerUpBtns[i].GetComponent<Button>().onClick.AddListener(delegate { UsePowerUp(i); });
            _powerUpBtns[i].GetComponent<Button>().image.sprite = _powerUps[i].powerUpImage;
        }
    }

    private void UsePowerUp(int powerUpIndex)
    {
        switch (_powerUps[powerUpIndex].powerUpType)
        {
            case PowerUp.PowerUpType.SPEED:
                _carController.SpeedBoost(_powerUps[powerUpIndex].powerUpAmount, _powerUps[powerUpIndex].powerUpDuration);
                _powerUpAmounts[powerUpIndex]--;
                _powerUpAmountTxt[powerUpIndex].text = _powerUpAmounts[powerUpIndex].ToString();
                break;
            case PowerUp.PowerUpType.SPIKE:
                _powerUpAmounts[powerUpIndex]--;
                _powerUpAmountTxt[powerUpIndex].text = _powerUpAmounts[powerUpIndex].ToString();
                break;
            case PowerUp.PowerUpType.HELICOPTER:
                _powerUpAmounts[powerUpIndex]--;
                _powerUpAmountTxt[powerUpIndex].text = _powerUpAmounts[powerUpIndex].ToString();
                break;
            case PowerUp.PowerUpType.REINFORCEMENTS:
                _powerUpAmounts[powerUpIndex]--;
                _powerUpAmountTxt[powerUpIndex].text = _powerUpAmounts[powerUpIndex].ToString();
                break;
            default:
                Debug.LogError("Could Not Use Powerup Because Powerup Type Does Not Exist");
                break;
        }
    }
}
