using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cascarone : MonoBehaviour
{

    public GameObject thrownBy;
    public Vector3 trajectory;

    [SerializeField] MeshRenderer mr;
    [SerializeField] public SpriteRenderer sr;
    [SerializeField] public Rigidbody rb;

    CapsuleCollider cc;

    [SerializeField] float lifeSpan = 3.5f;
    float currentLifeSpan;

    [SerializeField] float shotSpeed = 2.5f;

    [SerializeField] public Texture2D[] textures;

    public bool thrownByPlayer;
    bool popped = false;
    void OnEnable()
    {
        popped = false;
        if (mr)
            mr.enabled = true;
        if (cc)
            cc.enabled = true;
        if(rb)
            rb.isKinematic = false;
        transform.SetAsLastSibling();
        Invoke("DisableSelf", lifeSpan);
        if (trajectory == Vector3.zero)
            trajectory = Vector3.right;

        MaterialPropertyBlock test = new MaterialPropertyBlock();

        if (!thrownByPlayer)
        {
            sr.GetPropertyBlock(test);
            test.SetTexture("_DecorTex", textures[Random.Range(0, textures.Length - 1)]);
            sr.SetPropertyBlock(test);
        }
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
        transform.GetChild(0).Rotate(Vector3.back * Time.deltaTime * 550);
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
                    sr.enabled = false;
                    cc.enabled = false;
                    rb.isKinematic = true;
                    GameController.GC.confettiPop.EmitFromCascarone(transform.position, trajectory);
                }
                else if (contact.otherCollider.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<Player>().Hurt(contact.point);
                    sr.enabled = false;
                    cc.enabled = false;
                    rb.isKinematic = true;
                    GameController.GC.confettiPop.EmitFromCascarone(transform.position, trajectory);

                }
                else
                {
                    GameController.GC.confettiPop.EmitFromCascarone(transform.position, trajectory);
                }
            }
            sr.enabled = false;
            cc.enabled = false;
            rb.isKinematic = true;
            popped = true;
        }
    }

    void DisableSelf()
    {
        if(!popped)
            GameController.GC.confettiPop.EmitFromCascarone(transform.position, trajectory);

        gameObject.SetActive(false);
    }
    
}
