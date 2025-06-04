using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float wobbleSpeed;

    [SerializeField] int HP = 1;

    GameObject Player;
    Transform cascaroneParent;

    [SerializeField] float cooldown = 5f;

    // Start is called before the first frame update

    [SerializeField] NavMeshAgent agent;

    [SerializeField] bool traitor;

    void Start()
    {
        anim.speed = wobbleSpeed;

        Player = GameObject.Find("Player Controller");
        Debug.Log(Player);
        cascaroneParent = GameObject.Find("Cascarones").transform;
        Debug.Log(cascaroneParent);

    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown < 0)
        {
            if (traitor)
                agent.SetDestination(transform.position + Vector3.right * Random.Range(-4, 4) + Vector3.forward * Random.Range(-4, 4));
            else
                agent.SetDestination(Player.transform.position + Vector3.left * 4);
        }
        if (HP == 0)
        {
            Die();
        }

        if(Vector3.Distance(transform.position, Player.transform.position) < 10 && cooldown < 0)
        {
            Shoot();
        }
        else if(cooldown < 0) //shoot anyway lol
        {
            Shoot();
        }

        cooldown -= Time.deltaTime;
    }

    public void Hurt()
    {
        HP--;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Shoot()
    {
        cooldown = 5f;
        Cascarone cascarone = FindCascarone();
        cascarone.thrownBy = gameObject;
        cascarone.transform.position = transform.position;
        cascarone.gameObject.SetActive(true);
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
}
