using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speen : MonoBehaviour
{
    [SerializeField] bool ammo = true;
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
            if (ammo)
            {
                other.GetComponent<Player>().GetAmmo(6);
            }
            else
            {
                other.GetComponent<Player>().GetGold(1);
            }
            Destroy(gameObject);
        }    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().GetAmmo(6);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            //Enemies have unlimited ammo
            Destroy(gameObject);
        }
    }
}
