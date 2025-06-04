using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float moveSpeed;

    [SerializeField] Animator anim;

    [SerializeField] float wobbleSpeed;

    int _ammo;
    [SerializeField] int ammo { 
        get { return _ammo; } 
        set {
            _ammo = value;
            GameController.GC.UpdateAmmo(_ammo);
        } }

    [SerializeField] Transform cascaroneParent;

    [SerializeField] int HP = 1;

    // Update is called once per frame
    void Update()
    {
        anim.speed = Mathf.Min(0.5f + Mathf.Abs(Input.GetAxisRaw("Horizontal") * wobbleSpeed) + Mathf.Abs(Input.GetAxisRaw("Vertical") * wobbleSpeed), wobbleSpeed);

        transform.position += (Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward) * moveSpeed * Time.deltaTime;

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

    void Shoot()
    {
        if (Time.timeScale >= 1)
        {
            Cascarone cascarone = FindCascarone();
            cascarone.thrownBy = gameObject;
            cascarone.transform.position = transform.position;
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

    public void Hurt()
    {
        HP--;
        if (HP == 0)
        {
            GameController.GC.lose = true;
        }
    }
}
