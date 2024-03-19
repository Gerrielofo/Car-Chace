using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] int _maxEnemyCount;

    Waypoint[] _allWaypoints;

    private void Start()
    {
        _allWaypoints = FindObjectsOfType<Waypoint>();

        for (int i = 0; i < _maxEnemyCount; i++)
        {

        }
    }
}