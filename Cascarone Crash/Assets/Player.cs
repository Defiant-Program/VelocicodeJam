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



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Move();
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            Aim();
        }
        if (Input.GetMouseButtonDown(0))
        {
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

    public void GetAmmo(int ammount)
    {
        ammo += ammount;
    }

    void Move()
    {
        
        Vector3 moveMe = Input.GetKey(KeyCode.W) ? Vector3.forward : Vector3.zero;
        moveMe += Input.GetKey(KeyCode.A) ? Vector3.left : Vector3.zero;
        moveMe += Input.GetKey(KeyCode.S) ? Vector3.back : Vector3.zero;
        moveMe += Input.GetKey(KeyCode.D) ? Vector3.right : Vector3.zero;
        anim.speed = Mathf.Min(0.5f + Mathf.Abs(moveMe.x * wobbleSpeed) + Mathf.Abs(moveMe.z * wobbleSpeed), wobbleSpeed);
        transform.position += moveMe * moveSpeed * Time.deltaTime;
    }
    void Aim()
    {
        Vector3 moveRetical = Input.GetKey(KeyCode.UpArrow) ? Vector3.forward : Vector3.zero;
        moveRetical += Input.GetKey(KeyCode.LeftArrow) ? Vector3.left : Vector3.zero;
        moveRetical += Input.GetKey(KeyCode.DownArrow) ? Vector3.back : Vector3.zero;
        moveRetical += Input.GetKey(KeyCode.RightArrow) ? Vector3.right : Vector3.zero;

        moveRetical.Normalize();

        retical.transform.localPosition = Vector3.MoveTowards(retical.transform.localPosition, moveRetical * reticalDistance, reticalSpeed * Time.deltaTime);
        
    }

    void Shoot()
    {
        if (Time.timeScale >= 1)
        {
            Cascarone cascarone = FindCascarone();
            cascarone.thrownBy = gameObject;
            cascarone.transform.position = transform.position;
            cascarone.trajectory = retical.transform.position - transform.position;
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
            Invoke("Lose", 0.5f);
        }
    }

    void Lose()
    {
        GameController.GC.lose = true;
    }
}
