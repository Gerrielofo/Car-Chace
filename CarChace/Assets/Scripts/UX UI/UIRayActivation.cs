using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class UIRayActivation : MonoBehaviour
{
    public LayerMask interactionLayer;
    XRRayInteractor xRRayInteractor;

    private void Start()
    {
        xRRayInteractor = GetComponent<XRRayInteractor>();
    }

    void Update()
    {
        if (xRRayInteractor.IsOverUIGameObject())
        {
            GetComponent<XRInteractorLineVisual>().enabled = true;
            GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            GetComponent<XRInteractorLineVisual>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
        }
    }
}
