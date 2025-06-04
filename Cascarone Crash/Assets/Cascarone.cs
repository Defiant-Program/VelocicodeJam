using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cascarone : MonoBehaviour
{

    public GameObject thrownBy;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * 15;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != thrownBy)
        {
            if(other.tag == "Enemy")
            {
                other.GetComponent<Enemy>().Hurt();
                gameObject.SetActive(false);
            }
            if(other.tag == "Player")
            {
                other.GetComponent<Player>().Hurt();
                gameObject.SetActive(false);
            }
        }    
    }
}
