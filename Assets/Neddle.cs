using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neddle : MonoBehaviour
{
    static float minAngle = 580.0f;
    static float maxAngle = 321.0f;
    static Neddle thisNeddle;

    void Start()
    {
        thisNeddle = this;
    }

    public static void ShowGas(float gas, float min, float max)
    {
        float ang = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(min, max, gas));
        thisNeddle.transform.eulerAngles = new Vector3(0, 0, ang);
    }
}