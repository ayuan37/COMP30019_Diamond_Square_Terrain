using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public float cameraSpeed;
    public float rotationSpeed = 2.0f;

    public float pitchRotation = 0.0f;
    public float yawRotation = 0.0f;


    public float lastMouse;



    // move camera wasd
    // pitch and yaw

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(); // create (0,0,0)
        // move camera
        float dx = 0.0f, dz = 0.0f;

        // Forward and Backward relative to where you are looking
        if (Input.GetKey(KeyCode.W))
            dz += 1.0f;
        if (Input.GetKey(KeyCode.S))
            dz -= 1.0f;

        //Left and right
        if (Input.GetKey(KeyCode.D))
            dx += 1.0f;
        if (Input.GetKey(KeyCode.A))
            dx -= 1.0f;

        this.transform.localPosition += new Vector3(dx, 0.0f, dz) * cameraSpeed * Time.deltaTime;

        // Control relative pitch and yaw

        // pitch = rotate in x-axis -> look up and down
        pitchRotation -= rotationSpeed * Input.GetAxis("Mouse Y");

        // yaw = rotate in y-axis -> look left and right
        yawRotation += rotationSpeed * Input.GetAxis("Mouse X");

        this.transform.eulerAngles = new Vector3(pitchRotation, yawRotation, 0.0f);


    }
}
