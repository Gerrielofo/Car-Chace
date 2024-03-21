using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetCamPos : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [SerializeField] Transform _camera;
    [SerializeField] Transform _camOffset;
    [SerializeField] Transform _desiredCamPos;

    [SerializeField] bool _doReset;


    private void OnEnable()
    {
        playerInput.actions.FindAction("Recenter").performed += Recenter;
    }

    private void OnDisable()
    {
        playerInput.actions.FindAction("Recenter").performed -= Recenter;
    }

    private void Start()
    {
        ChangeCamPos();
    }

    private void Update()
    {
        if (!_doReset)
        {
            ChangeCamPos();
            _doReset = true;
        }
    }

    void Recenter(InputAction.CallbackContext ctx)
    {
        ChangeCamPos();
    }

    void ChangeCamPos()
    {
        Debug.Log("Recentered");
        XROrigin xROrigin = GetComponent<XROrigin>();
        xROrigin.MoveCameraToWorldLocation(_desiredCamPos.position);
        xROrigin.MatchOriginUpCameraForward(_desiredCamPos.up, _desiredCamPos.forward);
    }
}
