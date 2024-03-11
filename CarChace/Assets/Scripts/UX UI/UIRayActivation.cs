using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRRayInteractor))]
public class UIRayActivation : MonoBehaviour
{
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
