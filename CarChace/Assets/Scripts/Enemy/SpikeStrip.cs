using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeStrip : MonoBehaviour
{
    public float duration;

    private void Update()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collided With {other.gameObject.name}");
        if (other.transform.GetComponent<WheelCollider>())
        {
            Spike(other.transform.GetComponent<WheelCollider>());
        }
    }

    public void Spike(WheelCollider wheel)
    {
        WheelFrictionCurve curve = wheel.sidewaysFriction;
        curve.extremumSlip = 2f;
        wheel.sidewaysFriction = curve;
    }
}
