using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CarCrash : MonoBehaviour
{
    public GameObject[] carParts;
    public Vector2 partSpeed;
    public bool crash;

    void Update()
    {
        if (crash)
        {
            for (int i = 0; i < carParts.Length; i++)
            {
                carParts[i].GetComponent<CarPart>().Crash();
                crash = false;
                this.enabled = false;
            }
        }
    }
}
