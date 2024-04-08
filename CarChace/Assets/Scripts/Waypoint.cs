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

    public List<Transform> _possibleNextWaypoints;

    public Transform interSection;

    public float angleToNextPoint;

    public bool isBridge;

    [SerializeField] LayerMask _wayPointMask;
    [SerializeField] float _wayPointRadius;

    [SerializeField] List<Transform> transforms1 = new();
    [SerializeField] List<Transform> transforms2 = new();

    private void Start()
    {
        if (WayPointIndex == 0 && _possibleNextWaypoints.Count == 0)
        {
            Intersection();
        }
        else if (_possibleNextWaypoints.Count == 0)
        {
            _possibleNextWaypoints = GetNextWayPoints();
            if (_possibleNextWaypoints.Count != 0)
            {
                angleToNextPoint = Vector3.Angle(transform.position, _possibleNextWaypoints[0].position);
            }
        }
        else
        {
            Vector3 targetDir = _possibleNextWaypoints[0].position - transform.position;

            angleToNextPoint = Vector3.Angle(targetDir, transform.forward);
        }

    }

    [SerializeField] List<Transform> validOptions = new();
    void Intersection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _wayPointRadius, _wayPointMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Waypoint waypoint = colliders[i].GetComponent<Waypoint>();
            if (waypoint != this && waypoint._possibleNextWaypoints.Count == 0)
            {
                waypoint._wayPointIndex = 3;
            }
            else
            {
                if (waypoint != this)
                {
                    validOptions.Add(waypoint.transform);
                    Debug.Log("Added A Valid Option");
                }
            }

        }

        for (int i = 0; i < colliders.Length; i++)
        {
            Waypoint waypoint = colliders[i].GetComponent<Waypoint>();
            if (waypoint.WayPointIndex == 3)
            {
                Debug.Log("Index Was 3");
            }

            Debug.Log("Checklist Length == " + validOptions.Count);
            float closestRange = Mathf.Infinity;
            Transform closestTransform = null;
            for (int w = 0; w < validOptions.Count; w++)
            {
                if (closestTransform == null)
                {
                    closestTransform = validOptions[w];
                    closestRange = Vector3.Distance(validOptions[w].transform.position, closestTransform.position);
                }
                else if (Vector3.Distance(waypoint.transform.position, closestTransform.position) < closestRange)
                {
                    closestRange = Vector3.Distance(validOptions[w].transform.position, closestTransform.position);
                    closestTransform = validOptions[w];
                }
            }
            if (closestTransform != null && validOptions.Contains(closestTransform))
            {
                Debug.Log($"Removing {closestTransform.gameObject.name} from valid points for {waypoint.gameObject.name}");
                waypoint._possibleNextWaypoints = validOptions;
                waypoint._possibleNextWaypoints.Remove(closestTransform);
            }
            else
            {
                continue;
            }
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
        else if (_wayPointIndex == 3)
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawWireSphere(transform.position, _wayPointRadius);
    }
}
