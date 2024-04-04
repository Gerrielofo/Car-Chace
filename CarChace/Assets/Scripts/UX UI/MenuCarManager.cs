using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCarManager : MonoBehaviour
{
    [SerializeField] Transform _rotationPoint;
    [SerializeField] float _rotationSpeed;

    [SerializeField] GameObject[] _carMods;

    void Update()
    {
        UpdateCarMods();
        _rotationPoint.Rotate(new Vector3(0, _rotationSpeed * Time.deltaTime, 0));
    }

    void UpdateCarMods()
    {
        for (int i = 0; i < _carMods.Length; i++)
        {
            if (i < PlayerPrefs.GetInt("carModIndex"))
            {
                _carMods[i].SetActive(true);
            }
            else
            {
                _carMods[i].SetActive(false);
            }
        }
    }
}
