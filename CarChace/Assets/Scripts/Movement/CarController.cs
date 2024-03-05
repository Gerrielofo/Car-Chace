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

    [SerializeField] bool _frontWheelDrive;

    [SerializeField] float _isMove;
    [SerializeField] float _isExtraMove;
    [SerializeField] float _isTurn;

    [SerializeField] float _minAngle;
    [SerializeField] float _maxAngle;

    [SerializeField] float _steerSensitivity;
    [SerializeField] float _steerAngle;
    [SerializeField] Transform _steeringWheelRotation;

    [SerializeField] float _reverseDelay = 1f;
    [SerializeField] bool _canReverse;
    [SerializeField] float _brakeForce = 1000;
    [SerializeField] float _acceleration;
    [SerializeField] float _speed;

    float _reverseTimer;

    [SerializeField] float _velocity;
    [SerializeField] TMP_Text _speedTxt;

    [SerializeField] Gear[] _gears;
    [SerializeField] int _currentGear;
    [SerializeField] TMP_Text _gearTxt;

    [SerializeField] Transform _camPos;
    [SerializeField] Transform _camOffset;

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

        playerInput.actions.FindAction("ExtraGasBrake").started += OnExtraMove;
        playerInput.actions.FindAction("ExtraGasBrake").performed += OnExtraMove;
        playerInput.actions.FindAction("ExtraGasBrake").canceled += OnExtraMove;
    }

    private void OnDisable()
    {
        playerInput.actions.FindAction("GasBrake").started -= OnGas;
        playerInput.actions.FindAction("GasBrake").performed -= OnGas;
        playerInput.actions.FindAction("GasBrake").canceled -= OnGas;

        playerInput.actions.FindAction("Turn").started -= OnTurn;
        playerInput.actions.FindAction("Turn").performed -= OnTurn;
        playerInput.actions.FindAction("Turn").canceled -= OnTurn;

        playerInput.actions.FindAction("ExtraGasBrake").started -= OnExtraMove;
        playerInput.actions.FindAction("ExtraGasBrake").performed -= OnExtraMove;
        playerInput.actions.FindAction("ExtraGasBrake").canceled -= OnExtraMove;
    }

    private void Start()
    {
        _camOffset.position = _camPos.position;
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

    private void OnExtraMove(InputAction.CallbackContext context)
    {
        _isExtraMove = context.ReadValue<Vector2>().y;
        _isExtraMove /= 2;
    }

    private void Update()
    {
        if (_isMove >= 0)
        {
            // _canReverse = false;
            if (_canReverse)
            {
                Debug.Log("Breaking Cuz Reverse");
                _leftBack.brakeTorque = _brakeForce;
                _rightBack.brakeTorque = _brakeForce;

                if (_frontWheelDrive)
                {
                    _leftFront.brakeTorque = _brakeForce;
                    _rightFront.brakeTorque = _brakeForce;
                }
                if (myApproximation(_velocity, 0, 0.5f))
                {
                    _canReverse = false;

                }
            }
            else
            {
                _leftBack.brakeTorque = 0;
                _rightBack.brakeTorque = 0;

                if (_frontWheelDrive)
                {
                    _leftFront.brakeTorque = 0;
                    _rightFront.brakeTorque = 0;
                }

                _leftBack.motorTorque = speed;
                _rightBack.motorTorque = speed;

                if (_frontWheelDrive)
                {
                    _leftFront.motorTorque = speed;
                    _rightFront.motorTorque = speed;
                }
            }
        }
        else
        {
            if (myApproximation(_velocity, 0, 0.5f) && _reverseTimer <= 0f && !_canReverse)
            {
                _reverseTimer = _reverseDelay;
            }
            else if (_reverseTimer > 0)
            {
                _reverseTimer -= Time.deltaTime;
                if (_reverseTimer <= 0)
                {
                    _canReverse = true;
                }
            }

            if (_canReverse)
            {
                Debug.Log("Put it in reverse terry");
                _leftBack.motorTorque = speed;
                _rightBack.motorTorque = speed;

                if (_frontWheelDrive)
                {
                    _leftFront.motorTorque = speed;
                    _rightFront.motorTorque = speed;
                }

                _leftBack.brakeTorque = 0;
                _rightBack.brakeTorque = 0;

                if (_frontWheelDrive)
                {
                    _leftFront.brakeTorque = 0;
                    _rightFront.brakeTorque = 0;
                }
            }
            else
            {
                _leftBack.brakeTorque = _brakeForce;
                _rightBack.brakeTorque = _brakeForce;

                if (_frontWheelDrive)
                {
                    _leftFront.brakeTorque = _brakeForce;
                    _rightFront.brakeTorque = _brakeForce;
                }

                _leftBack.motorTorque = 0;
                _rightBack.motorTorque = 0;

                if (_frontWheelDrive)
                {
                    _leftFront.motorTorque = 0;
                    _rightFront.motorTorque = 0;
                }
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

        speed = _gears[_currentGear].speed * _isMove * (1 + _isExtraMove);
        _acceleration = _gears[_currentGear].acceleration;
        Mathf.Clamp(speed, _speed, -_speed);
    }

    private bool myApproximation(float a, float b, float tolerance)
    {
        return (Mathf.Abs(a - b) < tolerance);
    }
}
