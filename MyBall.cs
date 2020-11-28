using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBall : MonoBehaviour
{
    public float speed;
    public Vector3 velocity;

    public float vy_0;
    public float py_0;

    public float colorHue;
    public float colorValue;
    public float drag = 0.05f;

    private void Update()
    {
        speed = velocity.magnitude;
    }
}
