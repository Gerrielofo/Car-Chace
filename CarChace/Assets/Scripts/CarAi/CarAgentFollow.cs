using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] float _maxSteerAngle;
    [SerializeField] float _maxWheelTorque;
    [SerializeField] float _brakeTorque;

    [SerializeField] int _maxReroutes = 5;
    [SerializeField] float _timeToDespawn = 10f;
    [SerializeField] float[] _slopeAngles;

    [SerializeField] float _distanceTolarance = 0.65f;
    [SerializeField] float _addativeAgentSpeed = 1.5f;
    [SerializeField] float _addativeCarSpeed = 1.5f;

    [Header("Arrest Setting")]
    [SerializeField] float _maxArrestSpeed = 1f;
    [SerializeField] float _timeToArrest = 3f;
    [SerializeField] float _arrestTimer;
    [SerializeField] float _arrestRange = 4f;
    [SerializeField] LayerMask _policeMask;
    [SerializeField] MeshRenderer[] _colorChangeParts;


    [Header("Info")]
    [SerializeField] float _addativeSpeed = 0;
    [SerializeField] float _slopeMultiplier = 1f;
    [SerializeField] float _currentAngle;
    [SerializeField] float _currentSpeed;
    [SerializeField] Vector3 localTarget;
    [SerializeField] float targetAngle;
    public bool isAlive;
    [SerializeField] bool _idling;
    [SerializeField] float _idleTime;
    [SerializeField] float _idleThreshold = 3;

    [SerializeField] float _preferredDistanceFromAgent;
    public float PreferredDistanceFromAgent
    {
        get { return _preferredDistanceFromAgent; }
    }
    [SerializeField] float _distanceFromAgent;

    float _despawnTimer;
    int _rerouteAttempts;

    RaycastHit _slopeHit;

    bool _reversing;
    [SerializeField] float _reverseTime = 2f;


    void Start()
    {
        _carAgent = Instantiate(_carAgentPrefab, transform.position + transform.forward * 2, transform.rotation).GetComponent<NavMeshAgent>();
        _carAgent.GetComponent<CarAgent>().carTransform = transform;
        _preferredDistanceFromAgent = _carAgent.GetComponent<CarAgent>().CarRange / 2.5f;
    }

    public void ChangeColor(Color newColor)
    {
        for (int i = 0; i < _colorChangeParts.Length; i++)
        {
            _colorChangeParts[i].material.color = newColor;
        }
    }

    void Update()
    {
        if (!isAlive)
            return;

        if (_reversing)
        {
            DoReverse();
        }

        if (_idling && _idleThreshold > _idleTime)
        {
            _idleTime += Time.deltaTime;
        }
        else if (_idling && _idleTime > _idleThreshold)
        {
            if (_addativeSpeed < 7)
            {
                _carAgent.GetComponent<CarAgent>().BaseSpeed += _addativeAgentSpeed;
                _addativeSpeed += _addativeCarSpeed;
            }
            _idleTime = 0;
        }
        else
        {
            _idleTime = 0;
        }

        _currentAngle = GetSlopAngle();
        _slopeMultiplier = GetSlopAngle() / 3 + 1;

        if (_currentSpeed < _maxArrestSpeed)
        {
            CheckArrest();
        }
        else
        {
            _arrestTimer = 0f;
        }

        if (_currentSpeed < 0.1f)
        {
            _despawnTimer += Time.deltaTime;
            if (_rerouteAttempts > _maxReroutes)
            {
                Destroy(_carAgent.gameObject);
                Destroy(gameObject);
            }
            if (_despawnTimer > _timeToDespawn && !_reversing)
            {
                _carAgent.GetComponent<CarAgent>().GetNewRandomWaypoint();
                _rerouteAttempts++;
                _despawnTimer = 0f;
                StartCoroutine(Reverse());
            }
        }
        else
        {
            _rerouteAttempts = 0;
            _despawnTimer = 0;
        }



        _distanceFromAgent = Vector3.Distance(transform.position, _carAgent.transform.position);
        _currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;


        localTarget = transform.InverseTransformPoint(_carAgent.transform.position);
        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        CalculateSteerAngle();
        HandleAcceleration();
    }

    void CheckArrest()
    {
        Debug.Log($"{gameObject.name} Can Be Arrested");
        int amountOfPolice;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _arrestRange, _policeMask);
        amountOfPolice = colliders.Length;

        if (_arrestTimer < _timeToArrest)
        {
            _arrestTimer += Time.deltaTime * amountOfPolice;
        }
        else
        {
            Debug.Log("Ay you arrested that fool yo");
            GetComponent<Enemy>().Die();
        }
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
            _idling = false;
            Brake();
        }
        else if (_distanceFromAgent < _preferredDistanceFromAgent)
        {
            _idling = false;
            Brake();
        }
        else
        {
            // THIS SHOULD BE BETTER IDK HOW maybe a different check
            if (MyApproximation(_distanceFromAgent, _preferredDistanceFromAgent, _distanceTolarance))
            {
                Idle(_currentSpeed / _carAgent.speed);
            }
            else
            {
                _idling = false;
                UnBrake();
                _wheelColliders[2].motorTorque = _maxWheelTorque * _slopeMultiplier + _addativeSpeed;
                _wheelColliders[3].motorTorque = _maxWheelTorque * _slopeMultiplier + _addativeSpeed;
            }

        }
    }

    float GetSlopAngle()
    {
        float angle = 0;

        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit))
        {
            angle = Vector3.Angle(_slopeHit.normal, Vector3.up);
        }

        return angle;
    }

    IEnumerator Reverse()
    {
        _reversing = true;
        yield return new WaitForSeconds(_reverseTime);
        _reversing = false;
    }

    void DoReverse()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].brakeTorque = 0;
            _wheelColliders[i].motorTorque = -_maxWheelTorque;
        }
    }

    void Brake()
    {
        Debug.Log("BRAKE");
        _addativeSpeed = 0;
        _carAgent.GetComponent<CarAgent>().BaseSpeed = 10;

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
        _idling = true;
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].brakeTorque = 0;
            _wheelColliders[i].motorTorque = currentSpeed;
        }
    }

    public void Die()
    {
        isAlive = false;
        GetComponent<CarCrash>().crash = true;
        Destroy(_carAgent.gameObject);
        Destroy(gameObject, 5f);
    }

    private bool MyApproximation(float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _arrestRange);

    }
}
