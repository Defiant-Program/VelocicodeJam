using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cascarone : MonoBehaviour
{

    public GameObject thrownBy;

    void OnEnable()
    {
        Invoke("DisableSelf", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * 15;
    }

    /*
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != thrownBy)
        {
            if(other.tag == "Enemy")
            {
                other.GetComponent<Enemy>().Hurt(thrownBy);
                gameObject.SetActive(false);
            }
            if(other.tag == "Player")
            {
                other.GetComponent<Player>().Hurt();
                gameObject.SetActive(false);
            }
        }    
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != thrownBy)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.Log("Collided");
                if (contact.otherCollider.gameObject.tag == "Enemy")
                {
                    collision.gameObject.GetComponent<Enemy>().Hurt(thrownBy, contact.point);
                    gameObject.SetActive(false);
                }
                if (contact.otherCollider.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<Player>().Hurt(contact.point);
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    
}
