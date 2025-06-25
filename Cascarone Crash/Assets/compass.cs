using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass : MonoBehaviour
{
    [SerializeField] public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;
        transform.LookAt(targetPos);
    }
}
