using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 35);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().GetAmmo(6);
            Destroy(gameObject);
        }    
        else if(other.tag == "Enemy")
        {
            //Enemies have unlimited ammo
            Destroy(gameObject);
        }
    }
}
