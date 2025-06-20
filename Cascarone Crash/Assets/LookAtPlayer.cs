using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] float leftLookingMax = -30;
    [SerializeField] float rightLookingMax = 30;

    Transform player;
    
    Quaternion initialRotation;
    Vector3 initialPosition;
    [SerializeField] float rotationSpeed = 10;

    
    Vector3 playerLastPosition;

    float someDistanceScale = 10;
    float maxDistanceFactor = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.GC.player.transform;
        initialRotation = transform.rotation;
        initialPosition = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 lookDir = transform.position - player.position;
        float distance = lookDir.magnitude;

        float radians = Mathf.Atan2(lookDir.x, lookDir.z);
        float degrees = -Mathf.Clamp(radians * Mathf.Rad2Deg, leftLookingMax, rightLookingMax);

        float minDistanceFactor = 0.3f;
        float maxDistanceFactor = 1f;
        float someDistanceScale = 10f;

        float distanceFactor = Mathf.Clamp(distance / someDistanceScale, 0f, 1f);
        distanceFactor = Mathf.Lerp(minDistanceFactor, maxDistanceFactor, distanceFactor);

        float str = rotationSpeed * Time.deltaTime * distanceFactor;

        Quaternion targetRotation = Quaternion.Euler(0, degrees, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, str);

        /*
         * Works the best so far
        Vector3 lookDir = transform.position -  player.position;        
        float radians = Mathf.Atan2(lookDir.x, lookDir.z);
        float degrees = -Mathf.Clamp(radians * Mathf.Rad2Deg, leftLookingMax, rightLookingMax);

        float str = rotationSpeed * Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler(0, degrees, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, str);
        */
    }
}
