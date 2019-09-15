using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
  // add RigidBody to angle the direction of the forwards and backwards movement
  // of the camera
  public Rigidbody myRigidBody;
  public float cameraSpeed = 0.1f;
  public float rotationSpeed = 1.0f;

  public float pitchRotation = 0.0f;
  public float yawRotation = 0.0f;


  private void Start()
  {
    // Initialize RigidBody
    myRigidBody = GetComponent<Rigidbody>();
  }

  // Update camera movmenets for each frame  
  void Update()
  {
    // Forward and Backward relative to where you are looking
    if (Input.GetKey(KeyCode.W))
      myRigidBody.AddForce(transform.forward * cameraSpeed);

    if (Input.GetKey(KeyCode.S))
      myRigidBody.AddForce((transform.forward * -1) * cameraSpeed);

    //Left and right
    if (Input.GetKey(KeyCode.D))
      myRigidBody.AddForce(transform.right * cameraSpeed);
    if (Input.GetKey(KeyCode.A))
      myRigidBody.AddForce(transform.right * -1 * cameraSpeed);

    // Control relative pitch and yaw
    // pitch = rotate in x-axis -> look up and down
    pitchRotation -= rotationSpeed * Input.GetAxis("Mouse Y");

    // yaw = rotate in y-axis -> look left and right
    yawRotation += rotationSpeed * Input.GetAxis("Mouse X");

    this.transform.eulerAngles = new Vector3(pitchRotation, yawRotation, 0.0f);





  }
}
