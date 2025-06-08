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

    public int enemyID;
    [SerializeField] GameObject target;
    [SerializeField] int enemiesKilled = 0;

    GameObject killedBy;

    [SerializeField] Rigidbody rb;
    bool dead;

    [SerializeField] TrailRenderer tr;
    void Start()
    {
        anim.speed = wobbleSpeed;

        Player = GameObject.Find("Player Controller");
        Debug.Log(Player);
        cascaroneParent = GameObject.Find("Cascarones").transform;
        Debug.Log(cascaroneParent);
        rb = GetComponent<Rigidbody>();

        ChangeTarget();
        

        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (!target)
                ChangeTarget();
            if (target.GetComponent<Enemy>())
            {
                if (target.GetComponent<Enemy>().dead)
                {
                    ChangeTarget();
                }
            }
            if (cooldown < 0)
            {
                SetDestination();
            }
            if (!target)
                ChangeTarget();
            if (Vector3.Distance(transform.position, target.transform.position) < 10 && cooldown < 0)
            {
                Shoot();
            }

            cooldown -= Time.deltaTime;
        }
    }

    public void Hurt(GameObject thrownBy, Vector3 collisionPoint)
    {        
        killedBy = thrownBy;
        HP--;
        if (HP == 0 && !dead)
        {
            Die(collisionPoint);
        }
    }

    void Die(Vector3 collisionPoint)
    {
        tr.enabled = true;
        gameObject.layer = 11;
        agent.enabled = false;
        dead = true;
        transform.parent = GameController.GC.deadEnemiesParent;
        rb.AddForce((transform.position - collisionPoint) * 50 + Vector3.up* 4, ForceMode.Impulse);

        if(killedBy)
            if (killedBy.GetComponent<Enemy>())
                killedBy.GetComponent<Enemy>().enemiesKilled++;

        Invoke("DisableSelf", 3f);
    }

    void SetDestination()
    {
        if (traitor && enemiesKilled < 3)
        {
            if (target)
            {
                agent.SetDestination(target.transform.position + (Vector3.right * Random.Range(-4, 4) + Vector3.forward * Random.Range(-2, 2)));
            }
            else
            {
                ChangeTarget();
            }
        }
        else
            agent.SetDestination(Player.transform.position + Vector3.left * 4);
    }

    void Shoot()
    {
        cooldown = 5f;
        Cascarone cascarone = FindCascarone();
        cascarone.thrownBy = gameObject;
        cascarone.transform.position = transform.position;
        cascarone.trajectory = target.transform.position - transform.position;
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

    void ChangeTarget()
    {
        int newTarget = Random.Range(0, GameController.GC.enemyParent.childCount-1);
        if (GameController.GC.enemyParent.GetChild(newTarget).gameObject == gameObject)
        {
            //Everyone else is dead
            if (GameController.GC.enemyParent.childCount == 1)
            {
                target = Player;
                return;
            }
            //The current enemy is actually the last one in the list
            if (newTarget == GameController.GC.enemyParent.childCount - 1)
            {
                newTarget--;
                target = GameController.GC.enemyParent.GetChild(newTarget).gameObject;
                return;
            }
            //The current enemy is actually the first one in the list
            if (newTarget == 0)
            {
                newTarget++;
                target = GameController.GC.enemyParent.GetChild(newTarget).gameObject;
                return;
            }
        }
        else
        {
            target = GameController.GC.enemyParent.GetChild(newTarget).gameObject;
        }        
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
