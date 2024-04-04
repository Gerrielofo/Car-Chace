using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Beep : MonoBehaviour
{

    [SerializeField] AudioSource _beep;

    private void OnTriggerEnter(Collider other)
    {


        _beep.Play();

    }
}
