using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SUBTRACTFLOATS : MonoBehaviour
{
    public float float1;
    public float float2;
    public float outcome;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        outcome = float1 - float2;
    }
}
