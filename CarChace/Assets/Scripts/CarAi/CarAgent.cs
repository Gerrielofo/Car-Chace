using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class CarAgent : MonoBehaviour
{
    NavMeshAgent _carAgent;
    public bool runCode = true;

    [Header("Info")]
    [SerializeField] Transform _currentWaypoint;
    [SerializeField] int _currentRoadIndex;

    [SerializeField] Collider[] _colliders;
    [SerializeField] List<Transform> _passedWaypoints = new();

    public Transform carTransform;
    [Header("Settings")]
    [SerializeField] float _fov;
    [SerializeField] float _minWaypointDistance = 1.8f;
    [SerializeField] float _baseSpeed = 10f;

    [SerializeField] int _maxWaypoints = 5;
    [SerializeField] float _waypointCheckRange = 10;
    [SerializeField] float _carRange = 8;
    [SerializeField] LayerMask _wayPointMask;
    [SerializeField] LayerMask _carMask;

    public float CarRange { get { return _carRange; } }

    bool _onIntersection;

    float _distanceFromWaypoint;

    private void Start()
    {
        if (!runCode)
            return;
        _carAgent = GetComponent<NavMeshAgent>();
        GetNextWaypoint();
        _carAgent.speed = _baseSpeed;

    }

    void GetNextWaypoint()
    {
        if (!runCode)
            return;
        if (_currentWaypoint == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 30f, _wayPointMask);
            Transform closestTransform = null;
            float minRange = Mathf.Infinity;
            for (int i = 0; i < colliders.Length; i++)
            {
                float dist = Vector3.Distance(transform.position, colliders[i].transform.position);
                if (dist < minRange)
                {
                    minRange = dist;
                    closestTransform = colliders[i].transform;
                }
            }
            _currentWaypoint = closestTransform;
        }
        else
        {
            Waypoint waypoint = _currentWaypoint.GetComponent<Waypoint>();
            if (waypoint.interSection != null && !_onIntersection)
            {
                Transform[] availableWaypoints = waypoint.interSection.GetComponent<Waypoint>().GetWayPointConnections(waypoint.WayPointIndex, _currentWaypoint).ToArray();
                _onIntersection = true;
                if (availableWaypoints.Length == 0)
                {
                    Debug.LogError("Intersection Did Not Have Any Availlable Connections");
                    return;
                }
                _currentWaypoint = availableWaypoints[Random.Range(0, availableWaypoints.Length - 1)];
            }
            else
            {
                int rng = Random.Range(0, _currentWaypoint.GetComponent<Waypoint>()._possibleNextWaypoints.Length - 1);

                if (_currentWaypoint.GetComponent<Waypoint>()._possibleNextWaypoints.Length != 0)
                    _currentWaypoint = _currentWaypoint.GetComponent<Waypoint>()._possibleNextWaypoints[rng];
                if (_onIntersection)
                {
                    Debug.Log("Came Of Intersection");
                    _onIntersection = false;
                }
            }
        }
    }

    void GetNewRANDOMWaypoints()
    {
        if (!runCode)
            return;
        Debug.LogWarning("Getting New Waypoints");

        _colliders = Physics.OverlapSphere(transform.position, _waypointCheckRange, _wayPointMask);

        List<Transform> validWaypoints = new();
        for (int i = 0; i < _colliders.Length; i++)
        {
            if (_passedWaypoints.Contains(_colliders[i].transform))
            {
                continue;
            }
            Vector3 targetDir = _colliders[i].transform.position - transform.position;
            float angleToWaypoint = Vector3.Angle(targetDir, transform.forward);

            if (angleToWaypoint >= -_fov / 2 && angleToWaypoint <= _fov / 2)
            {
                if (_currentRoadIndex == 0)
                {
                    validWaypoints.Add(_colliders[i].transform);
                }
                else
                {
                    if (_colliders[i].transform.GetComponent<Waypoint>().WayPointIndex == _currentRoadIndex)
                    {
                        validWaypoints.Add(_colliders[i].transform);
                    }
                }
            }
        }

        _currentWaypoint = validWaypoints[Random.Range(0, validWaypoints.Count - 1)];
        _colliders = null;
    }

    private void Update()
    {
        if (!runCode)
            return;
        if (_currentWaypoint != null)
        {
            float distanceFromCar = Vector3.Distance(transform.position, carTransform.position);

            float angleBetweenWayPoints = _currentWaypoint.GetComponent<Waypoint>().angleToNextPoint;

            float newDesiredSpeed = _baseSpeed - ((360 - angleBetweenWayPoints) / 100);
            _carAgent.speed = newDesiredSpeed;

            // Debug.Log($"New Speed: {newDesiredSpeed}");

            if (distanceFromCar > _carRange)
            {
                _carAgent.isStopped = true;
            }
            else
            {
                _carAgent.isStopped = false;
            }
            _distanceFromWaypoint = Vector3.Distance(transform.position, _currentWaypoint.position);
            if (_distanceFromWaypoint <= _minWaypointDistance)
            {
                Debug.Log("IS CLOSE ENOUGH TO POINT");
                if (_passedWaypoints.Count >= _maxWaypoints)
                {
                    _passedWaypoints.RemoveAt(_passedWaypoints.Count - 1);
                }
                _passedWaypoints.Add(_currentWaypoint);

                GetNextWaypoint();
            }
        }
        if (_currentWaypoint == null)
        {
            GetNextWaypoint();
        }
        else
        {
            _carAgent.destination = _currentWaypoint.position;
        }
    }

    float DistanceSpeedModifier(float distance)
    {
        float distanceSpeedModifier = 0;

        distanceSpeedModifier = (100 + distance) / 100;

        return distanceSpeedModifier;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, _waypointCheckRange);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _carRange);
    }
}
