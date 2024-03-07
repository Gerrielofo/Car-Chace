using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TrafficAI : MonoBehaviour
{
    NavMeshAgent agent;
    [Header("Wheel Colliders")]
    [SerializeField] WheelCollider _frontLeft;
    [SerializeField] WheelCollider _frontRight;
    [SerializeField] WheelCollider _backLeft;
    [SerializeField] WheelCollider _backRight;

    [Header("Wheel Transforms")]
    [SerializeField] Transform _frontLeftTransform;
    [SerializeField] Transform _frontRightTransform;
    [SerializeField] Transform _backLeftTransform;
    [SerializeField] Transform _backRightTransform;

    [Header("Settings")]
    [SerializeField] float _maxRpm;
    [SerializeField] float _maxTurnAngle;
    [SerializeField] Transform _currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Vector3 speed = agent.desiredVelocity;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
