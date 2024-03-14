using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    CarCrash car;
    Vector3 force;

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponentInParent<CarCrash>();
        force = new Vector3(Random.Range(car.partSpeed.y, car.partSpeed.x), Random.Range(car.partSpeed.y, car.partSpeed.x), Random.Range(car.partSpeed.y, car.partSpeed.x));
    }

    public void Crash()
    {
        gameObject.AddComponent<Rigidbody>();
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        GetComponent<Rigidbody>().useGravity = true;
    }
}
