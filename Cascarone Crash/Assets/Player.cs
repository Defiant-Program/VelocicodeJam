using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    [SerializeField] public Animator anim;

    [SerializeField] float wobbleSpeed;

    [SerializeField] int _ammo;
    [SerializeField]
    int ammo
    {
        get { return _ammo; }
        set
        {
            _ammo = value;
            GameController.GC.UpdateAmmo(_ammo);
        }
    }

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

    Vector3 prevMousePos;
    [SerializeField] Transform reticalSprite;

    [SerializeField] public MeshRenderer playerMat;

    void Start()
    {
        anim.speed = wobbleSpeed;
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();

        ChangeAim();

    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameController.GC.OpenSettings();
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Move();
            }
            else
            {
                anim.speed = wobbleSpeed;
                rb.velocity = Vector3.zero;
            }
            switch (PlayerPrefs.GetInt("MouseAim"))
            {
                case 0:
                    if ((Input.mousePosition - prevMousePos).sqrMagnitude > 0.00001f)
                    {
                        MouseAim(Input.mousePosition - prevMousePos);
                    }
                    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
                    {
                        if (ammo > 0)
                        {
                            Shoot();
                        }
                    }
                    break;
                case 1:
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
                    break;
                
                case 2: //Dual analog sticks
                    Debug.Log("IMPLEMENT MEEEEE");
                    break;
            }
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 10, Time.deltaTime * 2);
        }
        prevMousePos = Input.mousePosition;
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

    void MouseAim(Vector3 newReticalPos)
    {

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y = 0 plane
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - transform.position;

            // Optional: flatten the direction if necessary
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                retical.transform.rotation = targetRotation;
            }
        }

    }

    public void ChangeAim()
    {
        Debug.Log(PlayerPrefs.GetInt("MouseAim"));
        retical.transform.localPosition = Vector3.zero;
        retical.transform.rotation = Quaternion.Euler(Vector3.zero);
        reticalSprite.transform.localPosition = Vector3.zero;
        reticalSprite.rotation = Quaternion.Euler(Vector3.zero);

        switch (PlayerPrefs.GetInt("MouseAim"))
        {
            case 0: // mouse
                reticalSprite.localPosition = Vector3.forward * 5;
                break;
            case 1: // keyboard
                retical.transform.localPosition = Vector3.right * 5;
                break;
            case 2: //Dual analog sticks
                Debug.Log("IMPLEMENT MEEEE");
                break;
        }
    }

    void Shoot()
    {
        if (Time.timeScale >= 1)
        {
            Cascarone cascarone = FindCascarone();
            cascarone.thrownByPlayer = true;
            MaterialPropertyBlock test = new MaterialPropertyBlock();
            cascarone.sr.GetPropertyBlock(test);
            test.SetTexture("_DecorTex", GameController.GC.GetEggTexture(ammo));
            cascarone.sr.SetPropertyBlock(test);
            cascarone.thrownBy = gameObject;
            switch (GameController.GC.aimType)
            {
                case 0: // Mouse
                case 2: // Dual Analog Sticks (functions the same here)
                    cascarone.trajectory = (reticalSprite.transform.position - retical.transform.position).normalized * 5;
                    break;
                case 1: // Keyboard
                    cascarone.trajectory = (retical.transform.position - transform.position).normalized * 5;
                    break;
            }
            if (cascarone.trajectory == Vector3.zero)
                cascarone.trajectory = Vector3.right * 5;
            cascarone.transform.position = transform.position + Vector3.up * 1.33f + (cascarone.trajectory.normalized * 3);
            cascarone.gameObject.SetActive(true);
            GetAmmo(-1);
        }
    }


    Cascarone FindCascarone()
    {
        foreach (Transform t in cascaroneParent)
        {
            if (!t.gameObject.activeSelf)
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

