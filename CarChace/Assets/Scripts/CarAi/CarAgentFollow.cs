using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAgentFollow : MonoBehaviour
{
    [SerializeField] NavMeshAgent _carAgent;

    [Header("settings")]
    [SerializeField] GameObject _carAgentPrefab;

    [SerializeField] WheelCollider[] _wheelColliders;

    [SerializeField] Transform[] _wheelTransforms;

    [SerializeField] float _minSpeedForTurn = 2f;
    [SerializeField] float _maxSpeedDif = 2;
    [SerializeField] float _desiredSpeed = 5;
    [SerializeField] float _maxSteerAngle;
    [SerializeField] float _maxWheelTorque;
    [SerializeField] float _brakeTorque;

    [SerializeField] float _spikedFriction = 10f;

    [Header("Info")]
    [SerializeField] float _currentSpeed;
    [SerializeField] Vector3 localTarget;
    [SerializeField] float targetAngle;
    public bool isAlive;


    [SerializeField] float _preferredDistanceFromAgent;
    float _distanceFromAgent;

    void Start()
    {
        _carAgent = Instantiate(_carAgentPrefab, transform.position + Vector3.forward * 2, transform.rotation).GetComponent<NavMeshAgent>();
        _carAgent.GetComponent<CarAgent>().carTransform = transform;
        _preferredDistanceFromAgent = _carAgent.GetComponent<CarAgent>().CarRange / 2.5f;
    }

    void Update()
    {
        if (!isAlive)
            return;
        _distanceFromAgent = Vector3.Distance(transform.position, _carAgent.transform.position);
        _currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;


        localTarget = transform.InverseTransformPoint(_carAgent.transform.position);
        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        CalculateSteerAngle();
        HandleAcceleration();
    }

    void CalculateSteerAngle()
    {
        _wheelColliders[0].steerAngle = Mathf.Clamp(targetAngle, -_maxSteerAngle, _maxSteerAngle);
        _wheelColliders[1].steerAngle = Mathf.Clamp(targetAngle, -_maxSteerAngle, _maxSteerAngle);

        for (int i = 0; i < _wheelTransforms.Length; i++)
        {
            Vector3 pos;
            Quaternion rot;
            _wheelColliders[i].GetWorldPose(out pos, out rot);
            _wheelTransforms[i].position = pos;
            _wheelTransforms[i].rotation = rot;
        }
    }

    void HandleAcceleration()
    {
        if (targetAngle > _maxSteerAngle && _currentSpeed > _minSpeedForTurn)
        {
            Brake();
        }
        else if (_distanceFromAgent < _preferredDistanceFromAgent)
        {
            Brake();
        }
        else
        {
            if (_currentSpeed < _carAgent.speed)
            {
                UnBrake();
                _wheelColliders[2].motorTorque = _maxWheelTorque;
                _wheelColliders[3].motorTorque = _maxWheelTorque;
            }
            else
            {
                Idle(_currentSpeed / _carAgent.speed);
            }

        }
    }

    void Brake()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].brakeTorque = _brakeTorque * _currentSpeed;
            _wheelColliders[i].motorTorque = 0;
        }
    }

    void UnBrake()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].brakeTorque = 0;
        }
    }

    void Idle(float currentSpeed)
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].brakeTorque = 0;
            _wheelColliders[i].motorTorque = currentSpeed;
        }
    }

    public void Die()
    {
        isAlive = false;
        Destroy(_carAgent.gameObject);
        Destroy(gameObject, 5f);
    }
}
