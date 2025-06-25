using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speen : MonoBehaviour
{
    [Header("Don't touch these 2")]
    [SerializeField] bool ammo = true;
    [SerializeField] bool powerup = false;
    Vector3 startPos;
    // Start is called before the first frame update

    [Header("Ignore if not looking at Egg Carton")]
    [SerializeField] AudioClip ammoRefill;
    [Header("Ignore if not looking at PowerUp")]
    [SerializeField] AudioClip getArmor;
    [Header("Ignore if not looking at Medal")]
    [SerializeField] AudioClip getMedal;


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
            GameController.GC.player.SFX.PlayOneShot(ammoRefill);
        }
        else if(powerup)
        {
            other.GetComponent<Player>().GetArmored();
            GameController.GC.player.SFX.PlayOneShot(getArmor);
        }
        else
        {
            other.GetComponent<Player>().GetGold(1);
            GameController.GC.player.SFX.PlayOneShot(getMedal);
        }
        Destroy(gameObject);
    }
}
