using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    [SerializeField] Animator anim;

    [SerializeField] float wobbleSpeed;

    [SerializeField] int _ammo;
    [SerializeField] int ammo { 
        get { return _ammo; } 
        set {
            _ammo = value;
            GameController.GC.UpdateAmmo(_ammo);
        } }

    [SerializeField] Transform cascaroneParent;

    [SerializeField] int HP = 1;

    [SerializeField] GameObject retical;
    [SerializeField] float reticalSpeed;
    [SerializeField] float reticalDistance;

    [SerializeField] Rigidbody rb;

    public TrailRenderer tr;
    bool dead = false;

    [SerializeField] int _gold;
    [SerializeField]
    int gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
            GameController.GC.UpdateGold(_gold);
        }
    }

    void Start()
    {
        anim.speed = wobbleSpeed;
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Move();
            }
            else
            {
                anim.speed = wobbleSpeed;
                rb.velocity = Vector3.zero;
            }
            if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))
            {
                Aim();
            }
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
            {
                if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.L))
                {
                    Aim();
                }
                if (ammo > 0)
                {
                    Shoot();
                }
                else
                {
                    //empty gun hammer click sfx
                    //"No ammo" particle effect
                }
            }

        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 10, Time.deltaTime * 2);
        }
    }

    public void GetAmmo(int ammount)
    {
        ammo += ammount;
    }

    public void GetGold(int ammount)
    {
        gold += ammount;
    }

    void Move()
    {
        
        Vector3 moveMe = Input.GetKey(KeyCode.W) ? Vector3.forward : Vector3.zero;
        moveMe += Input.GetKey(KeyCode.A) ? Vector3.left : Vector3.zero;
        moveMe += Input.GetKey(KeyCode.S) ? Vector3.back : Vector3.zero;
        moveMe += Input.GetKey(KeyCode.D) ? Vector3.right : Vector3.zero;
        moveMe.Normalize();
        anim.speed = 0.5f + Mathf.Abs(moveMe.x * wobbleSpeed) + Mathf.Abs(moveMe.z * wobbleSpeed);
        //transform.position += moveMe * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveMe * moveSpeed * Time.deltaTime);
    }
    void Aim()
    {
        Vector3 moveRetical = Input.GetKey(KeyCode.I) ? Vector3.forward : Vector3.zero;
        moveRetical += Input.GetKey(KeyCode.J) ? Vector3.left : Vector3.zero;
        moveRetical += Input.GetKey(KeyCode.K) ? Vector3.back : Vector3.zero;
        moveRetical += Input.GetKey(KeyCode.L) ? Vector3.right : Vector3.zero;

        moveRetical.Normalize();
        
        //retical.transform.localPosition = Vector3.MoveTowards(retical.transform.localPosition, moveRetical * reticalDistance, reticalSpeed * Time.deltaTime);
        
        retical.transform.localPosition = moveRetical * reticalDistance;

    }

    void Shoot()
    {
        if (Time.timeScale >= 1)
        {
            Cascarone cascarone = FindCascarone();
            cascarone.thrownBy = gameObject;
            cascarone.trajectory = (retical.transform.position - transform.position).normalized * 5;
            if (cascarone.trajectory == Vector3.zero)
                cascarone.trajectory = Vector3.right * 5;
            cascarone.transform.position = transform.position + Vector3.up * 1.33f + (cascarone.trajectory.normalized * 3);
            cascarone.gameObject.SetActive(true);
            GetAmmo(-1);
        }
    }

    Cascarone FindCascarone()
    {
        foreach(Transform t in cascaroneParent)
        {
            if(!t.gameObject.activeSelf)
            {
                return t.GetComponent<Cascarone>();
            }
        }
        return null;
    }

    public void Hurt(Vector3 collisionPoint)
    {
        HP--;
        if (HP == 0)
        {
            dead = true;
            tr.enabled = true;
            gameObject.layer = 11;
            GetComponent<CapsuleCollider>().enabled = false;
            rb.AddForce((transform.position - collisionPoint) * 50 + Vector3.up * 4, ForceMode.Impulse);

            Invoke("Lose", 0.5f);
        }
    }

    void Lose()
    {
        GameController.GC.Lose();
    }
}
