using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] int _wayPointPart;
    public int WayPointPart
    {
        get { return _wayPointPart; }
        set { _wayPointPart = value; }
    }

    // [SerializeField] WaypointType _waypointType;

    // private enum WaypointType
    // {
    //     IDKYET,
    //     BEGIN,
    //     MIDDLE,
    //     END,
    // }

    public Transform _wayPoint;
    [SerializeField] LayerMask _wayPointMask;
    [SerializeField] float _wayPointRadius;

    private void Start()
    {
        _wayPoint = this.transform;
    }

    public List<Transform> GetNextWayPoints()
    {
        List<Transform> transforms = new();
        Collider[] colliders = Physics.OverlapSphere(_wayPoint.position, _wayPointRadius, _wayPointMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform != this.transform)
            {
                transforms.Add(colliders[i].transform);
            }
        }
        return transforms;
    }

    public List<Transform> GetWayPointConnections(int wayPointPart)
    {
        List<Transform> transforms = new();
        Collider[] colliders = Physics.OverlapSphere(_wayPoint.position, _wayPointRadius, _wayPointMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Waypoint>()._wayPointPart == wayPointPart && colliders[i].transform != this.transform)
            {
                transforms.Add(colliders[i].transform);
            }
        }
        return transforms;
    }

    private void OnDrawGizmos()
    {
        if (_wayPointPart == 1)
        {
            Gizmos.color = Color.green;
        }
        else if (_wayPointPart == 2)
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(_wayPoint.position, _wayPointRadius);
    }
}
