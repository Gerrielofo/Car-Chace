using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CarController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] WheelCollider _leftFront;
    [SerializeField] WheelCollider _rightFront;
    [SerializeField] WheelCollider _leftBack;
    [SerializeField] WheelCollider _rightBack;

    [SerializeField] float _isMove;
    [SerializeField] float _isTurn;

    [SerializeField] float _steerAngle = 25f;
    [SerializeField] float steerAngle;


    [SerializeField] float _speed;
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
        _leftBack.motorTorque = _isMove * _speed;
        _rightBack.motorTorque = _isMove * _speed;

        steerAngle = _steerAngle * _isTurn;
        _leftFront.steerAngle = steerAngle;
        _rightFront.steerAngle = steerAngle;
    }
}
