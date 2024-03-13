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


    [Header("Settings")]
    [SerializeField] float _fov;
    [SerializeField] float _minWaypointDistance = 0.5f;

    [SerializeField] int _maxWaypoints = 5;
    [SerializeField] float _range;
    [SerializeField] LayerMask _wayPointMask;


    private void Start()
    {
        if (!runCode)
            return;
        _carAgent = GetComponent<NavMeshAgent>();
        GetNewRANDOMWaypoints();
    }

    void GetNextWaypoint()
    {
        if (!runCode)
            return;
        if (_currentWaypoint == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 30f);
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
            if (_currentWaypoint.GetComponent<Waypoint>()._possibleNextWaypoints.Length != 0)
                _currentWaypoint = _currentWaypoint.GetComponent<Waypoint>()._possibleNextWaypoints[Random.Range(0, _currentWaypoint.GetComponent<Waypoint>()._possibleNextWaypoints.Length - 1)];
        }
    }

    void GetNewRANDOMWaypoints()
    {
        if (!runCode)
            return;
        Debug.LogWarning("Getting New Waypoints");

        _colliders = Physics.OverlapSphere(transform.position, _range, _wayPointMask);

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
            float distanceFromWP = Vector3.Distance(transform.position, _currentWaypoint.position);
            if (distanceFromWP <= _minWaypointDistance)
            {
                if (_passedWaypoints.Count >= _maxWaypoints)
                {
                    _passedWaypoints.RemoveAt(_passedWaypoints.Count - 1);
                }
                _passedWaypoints.Add(_currentWaypoint);

                GetNewRANDOMWaypoints();
            }
        }
        if (_currentWaypoint == null)
        {
            GetNewRANDOMWaypoints();
        }
        else
        {
            _carAgent.destination = _currentWaypoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
