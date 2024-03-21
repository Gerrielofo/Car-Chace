using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class CarAgent : MonoBehaviour
{
    NavMeshAgent _carAgent;

    [Header("Info")]
    [SerializeField] Transform _currentWaypoint;
    [SerializeField] Transform _policeTarget;
    [SerializeField] int _currentRoadIndex;

    [SerializeField] Collider[] _colliders;
    [SerializeField] List<Transform> _passedWaypoints = new();

    public Transform carTransform;
    [Header("Settings")]
    public bool isPoliceAgent;

    [SerializeField] float _fov = 80;
    [SerializeField] float _minWaypointDistance = 1.8f;
    [SerializeField] float _baseSpeed = 10f;

    [SerializeField] int _maxWaypoints = 5;
    [SerializeField] float _waypointCheckRange = 10;
    [SerializeField] float _carRange = 8;
    [SerializeField] LayerMask _wayPointMask;
    [SerializeField] LayerMask _carMask;
    [SerializeField] LayerMask _enemyMask;

    [SerializeField] float _currentLifeSpan;
    public float maxLifeSpan;

    public float CarRange { get { return _carRange; } }

    bool _onIntersection;

    float _distanceFromWaypoint;

    private void Start()
    {
        _carAgent = GetComponent<NavMeshAgent>();
        if (isPoliceAgent)
        {
            Debug.Log($"Closest Enemy: {GetClosestEnemy()}");
            _carAgent.destination = GetClosestEnemy().position;
            _policeTarget = GetClosestEnemy();
        }
        else
        {
            GetNextWaypoint();
        }
        _carAgent.speed = _baseSpeed;

    }

    Transform GetClosestEnemy()
    {
        Transform closestEnemy = null;
        CarAgent[] carAgents = FindObjectsOfType<CarAgent>();

        float minRange = Mathf.Infinity;

        for (int i = 0; i < carAgents.Length; i++)
        {
            if (carAgents[i].isPoliceAgent)
            {
                continue;
            }
            float dist = Vector3.Distance(transform.position, carAgents[i].transform.position);
            if (dist < minRange)
            {
                minRange = dist;
                closestEnemy = carAgents[i].transform;
            }
        }
        return closestEnemy;
    }

    void GetNextWaypoint()
    {
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
        _carAgent.destination = _currentWaypoint.position;

    }

    void GetNewRANDOMWaypoints()
    {
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
        if (_currentWaypoint != null)
        {
            float distanceFromCar = Vector3.Distance(transform.position, carTransform.position);

            float angleBetweenWayPoints = _currentWaypoint.GetComponent<Waypoint>().angleToNextPoint;

            float newDesiredSpeed = _baseSpeed - ((360 - angleBetweenWayPoints) / 100);
            _carAgent.speed = newDesiredSpeed;

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
                if (_passedWaypoints.Count >= _maxWaypoints)
                {
                    _passedWaypoints.RemoveAt(_passedWaypoints.Count - 1);
                }
                _passedWaypoints.Add(_currentWaypoint);

                GetNextWaypoint();
            }
        }
        else if (isPoliceAgent)
        {
            _currentLifeSpan += Time.deltaTime;
            if (_currentLifeSpan > maxLifeSpan)
            {
                Destroy(carTransform.gameObject);
                Destroy(this);
            }
            float distanceFromCar = Vector3.Distance(transform.position, carTransform.position);

            if (distanceFromCar > _carRange)
            {
                _carAgent.isStopped = true;
            }
            else
            {
                _carAgent.isStopped = false;
            }
            _carAgent.destination = _policeTarget.position;
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
