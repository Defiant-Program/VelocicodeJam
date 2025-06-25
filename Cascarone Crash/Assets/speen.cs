using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speen : MonoBehaviour
{
    [SerializeField] bool ammo = true;
    [SerializeField] bool powerup = false;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 35);
        transform.localPosition = startPos + Mathf.Sin(Time.time * 2) * Vector3.up;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PickUpStuff(other);
        }    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PickUpStuff(collision.collider);
        }
    }

    void PickUpStuff(Collider other)
    {
        if (ammo)
        {
            other.GetComponent<Player>().GetAmmo(6);
        }
        else if(powerup)
        {
            other.GetComponent<Player>().GetArmored();
        }
        else
        {
            other.GetComponent<Player>().GetGold(1);
        }
        Destroy(gameObject);
    }
}
