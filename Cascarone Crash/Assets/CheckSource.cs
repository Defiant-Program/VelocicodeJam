using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSource : MonoBehaviour
{
    RuntimePlatform platform;

    void Start()
    {
        if(platform != RuntimePlatform.WindowsPlayer || platform != RuntimePlatform.OSXPlayer)
        {
            gameObject.SetActive(false);
        }
    }

}
