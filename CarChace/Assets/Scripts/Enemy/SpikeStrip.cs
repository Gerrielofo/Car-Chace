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
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponentInParent<CarAgentFollow>())
        {
            other.transform.GetComponentInParent<CarAgentFollow>().Spike();
            Destroy(gameObject);
        }
        else if (other.transform.GetComponent<CarAgentFollow>())
        {
            other.transform.GetComponent<CarAgentFollow>().Spike();
        }
    }
}
