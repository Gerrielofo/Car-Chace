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

    [SerializeField] float _maxSteerAngle;
    [SerializeField] float _maxWheelTorque;

    [Header("Info")]
    [SerializeField] Vector3 localTarget;
    [SerializeField] float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        _carAgent = Instantiate(_carAgentPrefab, transform.position + Vector3.forward * 2, transform.rotation).GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
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
        _wheelColliders[2].motorTorque = _maxWheelTorque;
        _wheelColliders[3].motorTorque = _maxWheelTorque;
    }
}
