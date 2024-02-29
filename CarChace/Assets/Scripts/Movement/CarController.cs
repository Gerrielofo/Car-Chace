using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor;
using System;
public class CarController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] WheelCollider _leftFront;
    [SerializeField] WheelCollider _rightFront;
    [SerializeField] WheelCollider _leftBack;
    [SerializeField] WheelCollider _rightBack;

    [SerializeField] bool FrontWheelDrive;

    [SerializeField] float _isMove;
    [SerializeField] float _isTurn;

    [SerializeField] float _minAngle;
    [SerializeField] float _maxAngle;

    [SerializeField] float _steerSensitivity;
    [SerializeField] float _steerAngle;
    [SerializeField] Transform _steeringWheelRotation;

    [SerializeField] float _brakeForce = 1000;
    [SerializeField] float _acceleration;
    [SerializeField] float _speed;

    [SerializeField] float _velocity;
    [SerializeField] TMP_Text _speedTxt;

    [SerializeField] Gear[] _gears;
    [SerializeField] int _currentGear;
    [SerializeField] TMP_Text _gearTxt;


    [SerializeField] GameObject _steeringWheelHolder;

    [SerializeField] float speed;

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

        if (_isMove > 0.5f)
        {
            _isMove = 1;
        }
        if (_isMove < -0.5f)
        {
            _isMove = -1;
        }
    }

    private void OnTurn(InputAction.CallbackContext context)
    {
        _isTurn = context.ReadValue<Vector2>().x;
    }

    private void Update()
    {
        if (_isMove >= 0)
        {
            _leftBack.brakeTorque = 0;
            _rightBack.brakeTorque = 0;

            if (FrontWheelDrive)
            {
                _leftFront.brakeTorque = 0;
                _rightFront.brakeTorque = 0;
            }

            _leftBack.motorTorque = speed;
            _rightBack.motorTorque = speed;

            if (FrontWheelDrive)
            {
                _leftFront.motorTorque = speed;
                _rightFront.motorTorque = speed;
            }
        }
        else
        {
            _leftBack.brakeTorque = _brakeForce;
            _rightBack.brakeTorque = _brakeForce;

            if (FrontWheelDrive)
            {
                _leftFront.brakeTorque = _brakeForce;
                _rightFront.brakeTorque = _brakeForce;
            }

            _leftBack.motorTorque = 0;
            _rightBack.motorTorque = 0;

            if (FrontWheelDrive)
            {
                _leftFront.motorTorque = 0;
                _rightFront.motorTorque = 0;
            }
        }


        // steerAngle = _steerAngle * _isTurn;

        _steerAngle = _steeringWheelRotation.eulerAngles.z / _steerSensitivity;
        Mathf.Clamp(_steerAngle, _minAngle, _maxAngle);

        _leftFront.steerAngle = -_steerAngle;
        _rightFront.steerAngle = -_steerAngle;

        _velocity = GetComponent<Rigidbody>().velocity.magnitude;
        _speedTxt.text = (_velocity * 10).ToString("0##");

        if (_gearTxt != null)
            _gearTxt.text = (_currentGear + 1).ToString();

        HandleSpeed();
    }

    void HandleSpeed()
    {
        if (_currentGear + 1 == _gears.Length)
        {
            if (_velocity < _gears[_currentGear].minimumSpeed)
            {
                _currentGear--;
            }
            speed = _gears[_currentGear].speed;
            _acceleration = _gears[_currentGear].acceleration;
            return;
        }
        if (_currentGear != 0 && _velocity < _gears[_currentGear].minimumSpeed)
        {
            _currentGear--;
        }
        else if (_velocity >= _gears[_currentGear + 1].minimumSpeed)
        {
            _currentGear++;
        }

        speed = _gears[_currentGear].speed * _isMove;
        _acceleration = _gears[_currentGear].acceleration;
        Mathf.Clamp(speed, _speed, -_speed);
    }
}
