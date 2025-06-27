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

    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] AudioClip[] missSounds;
    [SerializeField] AudioClip[] throwSounds;

    [SerializeField] AudioSource eggNoise;

    void OnEnable()
    {
        currentLifeSpan = 0;
        popped = false;
        if (sr)
            sr.enabled = true;
        if (cc)
            cc.enabled = true;
        if(rb)
            rb.isKinematic = false;
        transform.SetAsLastSibling();
        if (trajectory == Vector3.zero)
            trajectory = Vector3.right;

        MaterialPropertyBlock test = new MaterialPropertyBlock();

        if (!thrownByPlayer)
        {
            sr.GetPropertyBlock(test);
            test.SetTexture("_DecorTex", textures[Random.Range(0, textures.Length - 1)]);
            sr.SetPropertyBlock(test);
        }

        if(sr.isVisible)
        {
            eggNoise.PlayOneShot(throwSounds[Random.Range(0, throwSounds.Length - 1)]);
        }

    }

    void Start()
    {
        cc = GetComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += trajectory * Time.deltaTime * shotSpeed;

        transform.GetChild(0).localPosition = (Vector3.up * Mathf.Sin((currentLifeSpan / lifeSpan) * Mathf.PI) * 3) + Vector3.up;
        transform.GetChild(0).Rotate(Vector3.back * Time.deltaTime * 550);
        currentLifeSpan += Time.deltaTime;

        if(currentLifeSpan >= lifeSpan && !popped)
        {
            DisableSelf();
        }
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
                    popped = true;
                    GameController.GC.confettiPop.EmitFromCascarone(transform.position, trajectory);
                    if (thrownBy == GameController.GC.player.gameObject)
                    {
                        GameController.GC.player.hitEnemy.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length - 1)]);
                    }
                    return;
                }
                else if (contact.otherCollider.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<Player>().Hurt(contact.point);
                    sr.enabled = false;
                    cc.enabled = false;
                    rb.isKinematic = true;
                    popped = true;
                    GameController.GC.confettiPop.EmitFromCascarone(transform.position, trajectory);
                    if (sr.isVisible)
                    {
                        GameController.GC.player.hitEnemy.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length - 1)]);
                    }

                    return;
                }
                else
                {
                    if (sr.isVisible && !popped)
                    {
                        eggNoise.PlayOneShot(missSounds[Random.Range(0, missSounds.Length - 1)]);
                    }
                    GameController.GC.confettiPop.EmitFromCascarone(transform.position, trajectory);
                    popped = true;
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
        if (!popped)
        {            
            GameController.GC.confettiPop.EmitFromCascarone(transform.position, trajectory);
            if (sr.isVisible)
            {
                GameController.GC.player.SFX.PlayOneShot(missSounds[Random.Range(0, missSounds.Length - 1)]);
            }
        }
        popped = true;

        gameObject.SetActive(false);
    }
    
}
