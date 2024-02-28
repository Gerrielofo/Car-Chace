using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
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

    [SerializeField] float _minAngle;
    [SerializeField] float _maxAngle;

    [SerializeField] float _steerSensitivity;
    [SerializeField] float _steerAngle;
    [SerializeField] float _steeringWheelRotation;

    [SerializeField] float _acceleration;
    [SerializeField] float _speed;

    [SerializeField] float _velocity;
    [SerializeField] TMP_Text _speedTxt;


    [SerializeField] GameObject _steeringWheelHolder;



    private void OnEnable()
    {
        playerInput.actions.FindAction("GasBrake").started += OnGas;
        playerInput.actions.FindAction("GasBrake").performed += OnGas;
        playerInput.actions.FindAction("GasBrake").canceled += OnGas;

        playerInput.actions.FindAction("Turn").started += OnTurn;
        playerInput.actions.FindAction("Turn").performed += OnTurn;
        playerInput.actions.FindAction("Turn").canceled += OnTurn;
    }

    private void OnDisable()
    {
        playerInput.actions.FindAction("GasBrake").started -= OnGas;
        playerInput.actions.FindAction("GasBrake").performed -= OnGas;
        playerInput.actions.FindAction("GasBrake").canceled -= OnGas;

        playerInput.actions.FindAction("Turn").started -= OnTurn;
        playerInput.actions.FindAction("Turn").performed -= OnTurn;
        playerInput.actions.FindAction("Turn").canceled -= OnTurn;

    }

    private void OnGas(InputAction.CallbackContext context)
    {
        _isMove = context.ReadValue<float>();
    }

    private void OnTurn(InputAction.CallbackContext context)
    {
        _isTurn = context.ReadValue<Vector2>().x;
    }

    private void Update()
    {
        float speed = _isMove * (_acceleration * 1000) * Time.deltaTime;

        Mathf.Clamp(speed, _speed, -_speed);

        _leftBack.motorTorque = speed;
        _rightBack.motorTorque = speed;

        // steerAngle = _steerAngle * _isTurn;
        // _steeringWheelRotation = _steeringWheelHolder.GetComponent<SteeringWheelInteractable>().CurrentAngle;
        _steerAngle = _steeringWheelRotation / _steerSensitivity;
        Mathf.Clamp(_steerAngle, _minAngle, _maxAngle);

        _leftFront.steerAngle = -_steerAngle;
        _rightFront.steerAngle = -_steerAngle;

        _velocity = this.GetComponent<Rigidbody>().velocity.magnitude;
        _speedTxt.text = (_velocity * 10).ToString("#");
    }
}
