using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] int _wayPointIndex;
    public int WayPointIndex
    {
        get { return _wayPointIndex; }
        set { _wayPointIndex = value; }
    }

    public Transform[] _possibleNextWaypoints;

    public Transform interSection;

    [SerializeField] LayerMask _wayPointMask;
    [SerializeField] float _wayPointRadius;

    [SerializeField] List<Transform> transforms1 = new();
    [SerializeField] List<Transform> transforms2 = new();

    private void Start()
    {
        if (WayPointIndex == 0 && _possibleNextWaypoints.Length == 0)
        {
            Intersection();
        }
        else if (_possibleNextWaypoints.Length == 0)
        {
            _possibleNextWaypoints = GetNextWayPoints().ToArray();
        }
    }

    void Intersection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _wayPointRadius, _wayPointMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Waypoint>()._wayPointIndex == 1 && colliders[i].transform != this.transform)
            {
                transforms1.Add(colliders[i].transform);
            }
            else if (colliders[i].GetComponent<Waypoint>()._wayPointIndex == 2 && colliders[i].transform != this.transform)
            {
                transforms2.Add(colliders[i].transform);
            }
        }


        for (int i = 0; i < transforms1.Count; i++)
        {
            transforms1[i].GetComponent<Waypoint>().interSection = this.transform;
        }

        for (int i = 0; i < transforms2.Count; i++)
        {
            transforms2[i].GetComponent<Waypoint>().interSection = this.transform;
        }
    }

    public List<Transform> GetNextWayPoints()
    {
        List<Transform> transforms = new();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _wayPointRadius, _wayPointMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform != this.transform)
            {
                transforms.Add(colliders[i].transform);
            }
        }
        return transforms;
    }

    public List<Transform> GetWayPointConnections(int wayPointIndex, Transform excludedTransform = null)
    {
        List<Transform> transforms = new();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _wayPointRadius, _wayPointMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Waypoint>()._wayPointIndex == wayPointIndex && colliders[i].transform != this.transform)
            {
                if (excludedTransform != null)
                {
                    if (colliders[i].transform == excludedTransform)
                    {
                        continue;
                    }
                }

                transforms.Add(colliders[i].transform);

            }
        }
        return transforms;
    }

    private void OnDrawGizmos()
    {
        if (_wayPointIndex == 1)
        {
            Gizmos.color = Color.green;
        }
        else if (_wayPointIndex == 2)
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(transform.position, _wayPointRadius);
    }
}
