using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CarController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] float _isMove;
    // [SerializeField] float _isTurn;
    [SerializeField] float _isTurn;

    [SerializeField] float _wheelRotationSpeed;

    [SerializeField] Vector3 _frontWheelsRotation;

    private void OnEnable()
    {
        playerInput.actions.FindAction("GasBrake").started += OnGriddy;
        playerInput.actions.FindAction("GasBrake").performed += OnGriddy;
        playerInput.actions.FindAction("GasBrake").canceled += OnGriddy;

        playerInput.actions.FindAction("Turn").started += OnStickky;
        playerInput.actions.FindAction("Turn").performed += OnStickky;
        playerInput.actions.FindAction("Turn").canceled += OnStickky;
    }

    private void OnDisable()
    {
        playerInput.actions.FindAction("GasBrake").started -= OnGriddy;
        playerInput.actions.FindAction("GasBrake").performed -= OnGriddy;
        playerInput.actions.FindAction("GasBrake").canceled -= OnGriddy;

        playerInput.actions.FindAction("Turn").started -= OnStickky;
        playerInput.actions.FindAction("Turn").performed -= OnStickky;
        playerInput.actions.FindAction("Turn").canceled -= OnStickky;

    }

    private void OnGriddy(InputAction.CallbackContext context)
    {
        _isMove = context.ReadValue<float>();
    }

    private void OnStickky(InputAction.CallbackContext context)
    {
        _isTurn = context.ReadValue<Vector2>().x;
    }

    private void Update()
    {

    }
}
