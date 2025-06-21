using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cascarone : MonoBehaviour
{

    public GameObject thrownBy;
    public Vector3 trajectory;

    [SerializeField] MeshRenderer mr;
    CapsuleCollider cc;

    [SerializeField] float lifeSpan = 3.5f;
    float currentLifeSpan;

    [SerializeField] float shotSpeed = 2.5f;

    void OnEnable()
    {
        if (mr)
            mr.enabled = true;
        if (cc)
            cc.enabled = true;
        transform.SetAsLastSibling();
        Invoke("DisableSelf", lifeSpan);
        if (trajectory == Vector3.zero)
            trajectory = Vector3.right;
    }

    void Start()
    {
        mr = transform.GetChild(0).GetComponent<MeshRenderer>();
        cc = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += trajectory * Time.deltaTime * shotSpeed;

        transform.GetChild(0).localPosition = (Vector3.up * Mathf.Sin((currentLifeSpan / lifeSpan) * Mathf.PI) * 3) + Vector3.up;

        currentLifeSpan += Time.deltaTime;
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
                if (contact.otherCollider.gameObject.tag == "Enemy")
                {
                    collision.gameObject.GetComponent<Enemy>().Hurt(thrownBy, contact.point);
                    mr.enabled = false;
                    cc.enabled = false;
                }
                else if (contact.otherCollider.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<Player>().Hurt(contact.point);
                    mr.enabled = false;
                    cc.enabled = false;
                }
                else
                {
                    Debug.Log(contact.otherCollider.name);
                }
            }
            mr.enabled = false;
            cc.enabled = false;
        }
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    
}
