using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool lockCursor;

    public float mouseSensitivity = 25; // Mouse Sensitivity of Camera
    public Transform target; // Camera's target object
    public float distFromTarget = 2; // Camera Distance from Target
    public Vector2 pitchMinMax = new Vector2(-50,85); // The min and max values at which the vertical angles of the camera will be clamped at

    public float camRotateSmoothTIme; // Time needed for smooth camera angle movement to occur
    public Vector3 camRotateSmoothVelocity;
    public Vector3 camCurrRotation;


    float yaw; // Horizontal camera angle movement
    float pitch; // Vertical camera angle movement


    private void Start()
    {
        if (lockCursor) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X"); // Updates Horizontal camera movement
        pitch -= Input.GetAxis("Mouse Y"); // Updates Vertical camera movement
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y); // Clamps Vertical Camera Angles at the angles set in pitchMinMax

        camCurrRotation = Vector3.SmoothDamp(camCurrRotation, new Vector3(pitch, yaw), ref camRotateSmoothVelocity, camRotateSmoothTIme); // Setting where we want to look (with smoothing)

        //Vector3 targetRotation = new Vector3(pitch, yaw); // Setting where we want to look (without smoothing)
        //transform.eulerAngles = targetRotation; // Moving/transforming the camera angle to look there (without smoothing)

        transform.eulerAngles = camCurrRotation; // Moving/transforming the camera angle to look there (With smoothing)

        transform.position = target.position - transform.forward * distFromTarget; // Moving the camera to keep up with player/target if target moves

    }
}
