using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField] AudioClip[] ambience;
    [SerializeField] AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        soundSource.clip = (ambience[Random.Range(0, ambience.Length - 1)]);
        soundSource.Play();
        Debug.Log(soundSource.isPlaying);
        Debug.Log(soundSource.clip.name);
    }
}
