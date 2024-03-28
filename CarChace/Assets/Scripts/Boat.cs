using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class Boat : MonoBehaviour
{
    NavMeshAgent _navMeshAgent;

    [Header("Settings")]
    [SerializeField] float _desiredSpeed;
    [SerializeField] float _fov;

    [Header("Info")]
    [SerializeField] bool _hasOpened;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.speed = _desiredSpeed;
    }

    void Update()
    {

    }

    void GetRandomDestination()
    {

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
