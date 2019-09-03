using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public Vector3 localPos = new Vector3(0.0f,5.0f,0.0f);
    public float sunSpeed = 1.0f;
    public float timeCount = 5.0f;

    public float xSunDistance = 20.0f;
    public float ySunDistance = 20.0f;

    // Update is called once per frame
    void Update()
    {
        float x,y,z;

        // The sun rises and falls continously
        timeCount += Time.deltaTime * sunSpeed;
        x = Mathf.Cos (timeCount) * xSunDistance;
        y = Mathf.Sin (timeCount) * ySunDistance;

        transform.position = new Vector3 (x, y, 0);

    }
}
