using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MyBall : MonoBehaviour
{
    //public Vector3 velocity;

    public Vector3 speed;
    public Vector3 velocity;
    public float vel_comp;
    public Vector3 v_temp;

    public float velparametro;
    public Vector3 ultimavel;
    public Vector3 nuovavel;

    public Vector3 p_test;
    public Vector3 p_temp;

    public Vector3 direzione = new Vector3(1.0f, 1.0f, 1.0f);

    public float colorHue;
    public float colorValue;

    public float gravval = 0;

    public int c = 0;
    public int z = 0;

    public float tstart = 0;
    public float drag = 0.02f;

    private void Update()
    {
        if(ultimavel.y != 0 & gravval != 0 & c == 1)
        {
            tstart += Time.deltaTime;

            velocity = nuovavel;

            velocity = new Vector3 (Mathf.Abs(velocity.x - gravval * 9.8f * tstart), Mathf.Abs(velocity.y - gravval * 9.8f * tstart), Mathf.Abs(velocity.z - gravval * 9.8f * tstart));
            velocity = new Vector3 (Mathf.Abs(velocity.x * (1 - (drag * tstart))), Mathf.Abs(velocity.y * (1 - (drag * tstart))), Mathf.Abs(velocity.z * (1 - (drag * tstart))));

            ultimavel = new Vector3(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y), Mathf.Abs(velocity.z));
        }
        if (Mathf.RoundToInt(ultimavel.y) == 0 & gravval != 0 & c == 1 & z == 0)
        {
            direzione.Scale(new Vector3(1.0f, -1.0f, 1.0f));

            tstart = 0;
            tstart += Time.deltaTime;

            velocity = ultimavel;

            velocity = new Vector3(Mathf.Abs(velocity.x - gravval * 9.8f * tstart), Mathf.Abs(velocity.y - gravval * 9.8f * tstart), Mathf.Abs(velocity.z - gravval * 9.8f * tstart));
            velocity = new Vector3(Mathf.Abs(velocity.x * (1 - (drag * tstart))), Mathf.Abs(velocity.y * (1 - (drag * tstart))), Mathf.Abs(velocity.z * (1 - (drag * tstart))));

            nuovavel = new Vector3(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y), Mathf.Abs(velocity.z));

            z = 1;
        }
        if (c == 2)
        {
            if (Mathf.RoundToInt(ultimavel.y) < 0)
                velocity = new Vector3(0, 0, 0);

            velocity = new Vector3 (Mathf.Abs(velocity.x - 0.5f), 0, Mathf.Abs(velocity.z - 0.5f));
        }
    }
}
