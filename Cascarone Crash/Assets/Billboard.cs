using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Get the camera
        Camera cam = Camera.main;

        // Get the direction from this object to the camera, but ignore the vertical (Y) axis
        Vector3 camDirection = cam.transform.position - transform.position;
        camDirection.y = 0f; // Flatten to horizontal plane

        if (camDirection.sqrMagnitude > 0.001f)
        {
            // Compute the rotation to face the camera horizontally
            Quaternion targetRotation = Quaternion.LookRotation(camDirection);

            // Preserve the Z rotation (wobble)
            float z = transform.localEulerAngles.z;

            // Apply the camera-facing rotation
            transform.rotation = targetRotation;

            // Re-apply the Z-axis wobble
            transform.Rotate(0f, 0f, z);
        }
    }
}
