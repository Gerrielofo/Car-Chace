using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpButton : MonoBehaviour
{
    [SerializeField] PowerUp _powerUp;

    [SerializeField] Button _powerUpBtn;
    public PowerUp PowerUp
    {
        set { _powerUp = value; }
    }

    private void Awake()
    {

    }
}
