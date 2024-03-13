using UnityEngine;
using UnityEngine.AI;

public class AIController2 : MonoBehaviour
{
    public Circuit circuit;
    private Cart cart;
    public float steeringSensitivity = 0.01f;
    public float breakingSensitivity = 1.0f;
    public float accelerationSensitivity = 0.3f;

    [SerializeField] float _carSearchRadius;
    [SerializeField] LayerMask _wayPointMask;

    [SerializeField] Transform _wayPointToGoTo;

    public GameObject trackerPrefab;
    NavMeshAgent agent;

    int currentTrackerWP;
    float lookAhead = 10;

    float lastTimeMoving = 0;


    [SerializeField] Vector3 _agentSpawnOffset = new Vector3(0, 0, 2);
    // Start is called before the first frame update
    void Start()
    {
        cart = GetComponent<Cart>();
        GameObject tracker = Instantiate(trackerPrefab, cart.transform.position + _agentSpawnOffset, cart.transform.rotation) as GameObject;
        agent = tracker.GetComponent<NavMeshAgent>();
    }

    private Transform GetNewWayPoint()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _carSearchRadius, _wayPointMask);

        // Collider[] colliders = Physics.OverlapSphere(this.transform.position, _carSearchRadius, _wayPointMask);

        Debug.Log($"{colliders.Length}");
        _wayPointToGoTo = colliders[Random.Range(0, colliders.Length)].transform;

        return _wayPointToGoTo;
    }

    void ProgressTracker()
    {
        if (Vector3.Distance(agent.transform.position, cart.transform.position) > lookAhead)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        agent.SetDestination(GetNewWayPoint().position);


        if (Vector3.Distance(agent.transform.position, circuit.waypoints[currentTrackerWP].position) < 4 && circuit != null)
        {
            currentTrackerWP++;
            if (currentTrackerWP >= circuit.waypoints.Count)
                currentTrackerWP = 0;
        }
    }

    void ResetLayer()
    {
        cart.gameObject.layer = 10;
    }

    // Update is called once per frame
    void Update()
    {
        ProgressTracker();

        Vector3 localTarget;
        float targetAngle;

        if (cart._rigidbody.velocity.magnitude > 1)
        {
            lastTimeMoving = Time.time;
        }

        if (Time.time > lastTimeMoving + 4)
        {
            cart.transform.position = circuit.waypoints[currentTrackerWP].transform.position + Vector3.up * 2;
            agent.transform.position = cart.transform.position;
            cart.gameObject.layer = 8;

            Invoke("ResetLayer", 3);
        }

        localTarget = cart.transform.InverseTransformPoint(agent.transform.position);
        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;


        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(cart.current_speed);
        float speedFactor = cart.current_speed / 30;
        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerFactor = corner / 90f;

        float brake = 0;
        if (corner > 10 && speedFactor > 0.1f)
            brake = Mathf.Lerp(0, 1 + speedFactor * breakingSensitivity, cornerFactor);

        float accel = 1f;
        if (corner > 20 && speedFactor > 0.1f && speedFactor > 0.2f)
            accel = Mathf.Lerp(0, 1 * accelerationSensitivity, 1 - cornerFactor);

        cart.AccelerateCart(accel, steer, brake);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _carSearchRadius);
    }
}
