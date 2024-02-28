using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheelInteractable : XRBaseInteractable
{
    [SerializeField] Transform _wheelHolderTransform;
    [SerializeField] Transform _wheelTransform;


    [SerializeField] GameObject _hand1;
    [SerializeField] GameObject _hand2;

    [SerializeField] Vector3 zRotation;
    [SerializeField] Vector3 _rotation1;
    [SerializeField] Vector3 _rotation2;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        foreach (IXRSelectInteractor interactor in interactorsSelecting)
        {
            if (interactorsSelecting.Count == 1)
            {
                _rotation1 = interactor.transform.eulerAngles - zRotation;
            }
            else
            {
                _rotation2 = interactor.transform.eulerAngles - zRotation;
            }
            // if (interactor.transform.gameObject == args.interactableObject.transform.gameObject && interactor.transform.gameObject == _hand1)
            // {
            //     _rotation1 = interactor.transform.eulerAngles - zRotation;
            // }
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // _currentAngle = FindWheelAngle();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                // float sharedRotation = 0;
                foreach (IXRSelectInteractor interactor in interactorsSelecting)
                {
                    if (interactorsSelecting.Count == 1)
                    {
                        zRotation = interactor.transform.eulerAngles - _rotation1;
                    }
                    else
                    {
                        Vector3 tempVec = Vector3.zero;
                        foreach (IXRSelectInteractor interactor2 in interactorsSelecting)
                        {
                            tempVec += interactor2.transform.eulerAngles;
                        }
                        tempVec /= 2;
                        zRotation = tempVec - ((_rotation1 + _rotation2) / 2);
                    }
                }
                _wheelTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, zRotation.z));

            }
        }
    }

    // private void RotateWheel()
    // {
    //     float totalAngle = FindWheelAngle();

    //     float angleDifference = _currentAngle - totalAngle;

    //     _currentAngle = totalAngle;
    //     _wheelTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, totalAngle));
    //     onTurning?.Invoke(angleDifference);
    // }

    // private float FindWheelAngle()
    // {
    //     float totalAngle = 0;

    //     foreach (IXRSelectInteractor interactor in interactorsSelecting)
    //     {
    //         Vector2 direction = FindLocalPoint(interactor.transform.position);
    //         totalAngle += ConvertToAngle(direction) * FindRotationSensitivity();
    //     }

    //     return totalAngle;
    // }

    // private Vector2 FindLocalPoint(Vector3 position)
    // {
    //     return transform.InverseTransformPoint(position).normalized;
    // }

    // private float ConvertToAngle(Vector2 direction)
    // {
    //     return Vector2.SignedAngle(transform.up, direction);
    // }

    private float FindRotationSensitivity()
    {
        return 1.0f / interactorsSelecting.Count;
    }
}
