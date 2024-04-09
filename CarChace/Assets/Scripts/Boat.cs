using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Boat : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;

    [SerializeField] Transform _boatFront;

    [Header("Settings")]
    [SerializeField] float _desiredSpeed;
    [SerializeField] float _fov;
    [SerializeField] float _randomDestinationRadius;

    [SerializeField] LayerMask _waterMask;

    [Header("Debug")]
    public bool ShowGizmos;

    [SerializeField] float _minDistanceFromPoint;
    [SerializeField] float _distanceFromPoint;

    [Header("Info")]
    [SerializeField] bool _hasOpened;


    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.speed = _desiredSpeed;
    }

    void Update()
    {
        _distanceFromPoint = Vector3.Distance(transform.position, _navMeshAgent.destination);

        if (_distanceFromPoint < _minDistanceFromPoint)
        {
            _navMeshAgent.destination = GetRandomDestination();
        }
    }

    Vector3 GetRandomDestination()
    {
        Vector3 direction = new Vector3(0, Random.Range(transform.localRotation.y - _fov, transform.localRotation.y + _fov), 0);
        // Debug.Log($"Random Direction: {direction}");
        // Debug.Log($"y rotation {_navMeshAgent.transform.localRotation.y} + -fov {-_fov} = {transform.rotation.y - _fov}");
        // Vector3 _randomDirection = Random.insideUnitSphere * _randomDestinationRadius;
        NavMesh.SamplePosition(transform.position, out NavMeshHit hit, _randomDestinationRadius, _waterMask);
        // Debug.Log($"random pos :{hit.position}");
        return hit.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<BridgeTrigger>(out BridgeTrigger bridgeTrigger);

        if (bridgeTrigger != null)
        {
            bridgeTrigger.ToggleBridge(!_hasOpened);
            _hasOpened = !_hasOpened;
        }
    }

    private void OnDrawGizmos() // shows a Gizmos representing the waypoints and AI FOV
    {
        if (ShowGizmos == true)
        {
            CalculateFOV();
        }

        void CalculateFOV()
        {
            Gizmos.color = Color.white;
            float rayRange = 10.0f;
            float halfFOV = _fov / 2.0f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.DrawRay(_boatFront.position, leftRayDirection * rayRange);
            Gizmos.DrawRay(_boatFront.position, rightRayDirection * rayRange);
        }
    }
}
