using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Gear")]
public class Gear : ScriptableObject
{
    public float minimumSpeed;
    public float acceleration;
    public float speed;
}
