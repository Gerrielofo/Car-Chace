using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCarManager : MonoBehaviour
{
    [SerializeField] Transform _rotationPoint;
    [SerializeField] float _rotationSpeed;

    [SerializeField] GameObject[] _carMods1;
    [SerializeField] GameObject[] _carMods2;
    [SerializeField] GameObject[] _carMods3;

    void Update()
    {
        UpdateCarMods();
        _rotationPoint.Rotate(new Vector3(0, _rotationSpeed * Time.deltaTime, 0));
    }

    void UpdateCarMods()
    {
        for (int one = 0; one < _carMods1.Length; one++)
        {
            _carMods1[one].SetActive(false);
        }

        for (int two = 0; two < _carMods2.Length; two++)
        {
            _carMods2[two].SetActive(false);
        }

        for (int three = 0; three < _carMods3.Length; three++)
        {
            _carMods3[three].SetActive(false);
        }
        for (int i = 0; i < PlayerPrefs.GetInt("carModIndex"); i++)
        {
            if (i == 2)
            {
                Debug.Log("was three");
                for (int three = 0; three < _carMods3.Length; three++)
                {
                    _carMods3[three].SetActive(true);
                }
            }

            if (i == 1)
            {
                Debug.Log("was two");
                for (int two = 0; two < _carMods2.Length; two++)
                {
                    _carMods2[two].SetActive(true);
                }
            }

            if (i == 0)
            {
                Debug.Log("was one");
                for (int one = 0; one < _carMods1.Length; one++)
                {
                    _carMods1[one].SetActive(true);
                    Debug.Log($"Set {one} to active");
                }
            }
        }
    }
}
