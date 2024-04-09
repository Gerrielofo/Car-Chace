using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{

    [SerializeField] Transform _waypointHolder;
    [SerializeField] List<Transform> _waypoints = new();
    [SerializeField] int _waypointIndex;
    [Header("Settings")]
    [SerializeField] float _desiredSpeed;
    [SerializeField] float _minDistanceFromWaypoint;
    [SerializeField] bool loop_respawn = true;

    [Header("Info")]
    [SerializeField] bool _hasOpened;



    void Start()
    {
        for (int i = 0; i < _waypointHolder.childCount; i++)
        {
            _waypoints.Add(_waypointHolder.GetChild(i).transform);
        }
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_waypointIndex].position, _desiredSpeed * Time.deltaTime);
        transform.LookAt(_waypoints[_waypointIndex]);

        if (Vector3.Distance(transform.position, _waypoints[_waypointIndex].position) < _minDistanceFromWaypoint)
        {
            if (_waypointIndex == _waypoints.Count - 1)
            {
                if (loop_respawn)
                {
                    _waypointIndex = 0;
                    transform.position = _waypoints[_waypointIndex].position;
                }
                else
                {
                    _waypointIndex = 0;
                }
            }
            else
            {
                _waypointIndex++;
            }
        }
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
}
