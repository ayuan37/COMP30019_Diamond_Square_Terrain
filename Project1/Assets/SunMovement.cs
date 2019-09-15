using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public Vector3 localPos = new Vector3(0.0f,5.0f,0.0f);
    public float sunSpeed = 0.5f;
    public float timeCount = 5.0f;

    private float xSunDistance = 30.0f;
    private float ySunDistance = 30.0f;
    
    void Update()
    {
        float x, y;

        // The sun rises and falls continously
        timeCount += Time.deltaTime * sunSpeed;
        x = Mathf.Cos (timeCount) * xSunDistance;
        y = Mathf.Sin (timeCount) * ySunDistance;

        transform.position = new Vector3 (x, y, 0);

    }
}
