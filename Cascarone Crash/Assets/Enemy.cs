using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float wobbleSpeed;

    [SerializeField] float ratWobbleSpeed = 1;

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

    [SerializeField] public TrailRenderer tr;

    [SerializeField] MeshRenderer enemyRenderer;
    int enemyType;

    public string[] movements = { "Dog", "Rat", "Cat", "Bird", "Hedgehog", "Armadillo", "Hare" };

    bool chasing = false;
    bool shooting = false;
    bool shootingAtPlayerTemporarily;

    [SerializeField] float visionDistance = 15;

    [SerializeField] GameObject medal;

    float shootingTimer = 0;

    [Header("Death Related Variables")]
    [SerializeField] bool dontLaunch;
    [SerializeField] float deathSpinSpeed = 135;

    [SerializeField] Texture[] ouchies;

    Vector3 prevPos;

    [SerializeField] AudioSource deathZoomNoise;

    void Start()
    {
        enemyID = transform.GetSiblingIndex();

        enemyType = Random.Range(0, GameController.GC.mats.Length);

        MaterialPropertyBlock test = new MaterialPropertyBlock();
        enemyRenderer.GetPropertyBlock(test);
        test.SetTexture("_MainTex", GameController.GC.characters[enemyType].texture);

        enemyRenderer.SetPropertyBlock(test);

        anim.speed = wobbleSpeed;

        Player = GameObject.Find("Player Controller");
        cascaroneParent = GameObject.Find("Cascarones").transform;
        rb = GetComponent<Rigidbody>();

        ChangeTarget();
        agent.updateRotation = false;

        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;

        anim.Play(movements[enemyType] + "Movement");
        StartCoroutine(Wander());

        traitor = enemyID % 4 == 0;

        agent.avoidancePriority = Random.Range(30, 60);

        if(enemyType == 2)
        {
            anim.speed = ratWobbleSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!dead)
            enemyRenderer.material.color = Color.white + Color.red * shootingTimer;

        if (!dead && !shooting)
        {
            if (!target || cooldown < -10 )
                ChangeTarget();
            if (target.GetComponent<Enemy>())
            {
                if (target.GetComponent<Enemy>().dead)
                {
                    ChangeTarget();
                }
            }
            if (cooldown < 0 && chasing)
            {
                SetDestination();
            }
            chasing = Vector3.Distance(transform.position, target.transform.position) > visionDistance;
            if ((Vector3.Distance(transform.position, target.transform.position) < visionDistance || Vector3.Distance(transform.position, Player.transform.position) < visionDistance) && cooldown < 0)
            {
                if (Vector3.Distance(transform.position, Player.transform.position) < visionDistance)
                    shootingAtPlayerTemporarily = true;
                shooting = true;
                agent.isStopped = true;
            }

            cooldown -= Time.deltaTime;
        }
        else if(shooting && !dead)
        {
            shootingTimer += Time.deltaTime;
            if(shootingTimer >= 1)
            {
                Shoot();
            }
        }
        if(dead)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * deathSpinSpeed);
        }
    }

    void FixedUpdate()
    {
        if (agent.velocity.x < 0) //moving left
        {
            anim.transform.localScale = Vector3.right + Vector3.up + Vector3.forward;
        }
        else
        {
            anim.transform.localScale = Vector3.left + Vector3.up + Vector3.forward;
        }
    }

    public void Hurt(GameObject thrownBy, Vector3 collisionPoint)
    {        
        killedBy = thrownBy;
        HP--;
        if (HP == 0 && !dead)
        {
            deathZoomNoise.Play();

            MaterialPropertyBlock test = new MaterialPropertyBlock();
            enemyRenderer.GetPropertyBlock(test);
            test.SetTexture("_MainTex", ouchies[enemyType]);
            enemyRenderer.SetPropertyBlock(test);
            Die(collisionPoint);
        }
    }

    void Die(Vector3 collisionPoint)
    {
        
        tr.enabled = true;
        gameObject.layer = 11;
        GetComponent<CapsuleCollider>().enabled = false;
        agent.enabled = false;
        dead = true;
        transform.parent = GameController.GC.deadEnemiesParent;

        if(!dontLaunch)
            rb.AddForce((transform.position - collisionPoint) * 25 + Vector3.up* 4, ForceMode.Impulse);

        if (killedBy)
        {
            if (killedBy.GetComponent<Enemy>())
                killedBy.GetComponent<Enemy>().AddKillCount();
            else
            {
                GameController.GC.player.GetGold(1);
                if (Random.Range(0, 5) == 3)
                {
                    int randomMedals = Random.Range(0, 4);
                    for (int i = 0; i < randomMedals; i++)
                    {
                        Instantiate(medal, transform.position, Quaternion.identity);
                    }
                }
            }
        }
        StopAllCoroutines();

        Invoke("DisableSelf", 3f);
    }

    void SetDestination()
    {
        if (traitor)
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
        if (!dead)
        {
            shootingTimer = 0;
            shooting = false;
            agent.isStopped = false;

            cooldown = Random.Range(1f, 6f);
            Cascarone cascarone = FindCascarone();
            cascarone.thrownBy = gameObject;
            if (shootingAtPlayerTemporarily)
            {
                cascarone.trajectory = new Vector3(Player.transform.position.x - transform.position.x, 0, Player.transform.position.z - transform.position.z).normalized * 5;
                shootingAtPlayerTemporarily = false;
            }
            else
                cascarone.trajectory = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z).normalized * 5;

            cascarone.transform.position = transform.position + cascarone.trajectory;
            cascarone.gameObject.SetActive(true);
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
            if (newTarget <= GameController.GC.enemyParent.childCount - 1 && newTarget != 0)
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
            if (traitor)
                target = GameController.GC.enemyParent.GetChild(newTarget).gameObject;
            else
                target = Player;
            return;
        }        

        if(!target)
        {
            Debug.Log("How. Enemy ID: " + enemyID + " newTarget#: " + newTarget + "Target? " + target);
        }
    }

    public void AddKillCount()
    {
        enemiesKilled++;
        if (enemiesKilled >= 3)
            traitor = false;
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    IEnumerator Wander()
    {
        while (!dead)
        {
            if (!chasing && !shooting)
            {
                float waitTime = Random.Range(1, 3);
                yield return new WaitForSeconds(waitTime);

                Vector3 randomDirection = Random.insideUnitSphere * 15;
                randomDirection += transform.position;

                if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 15, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
                if (!dead)
                {
                    while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                    {
                        yield return null;
                    }
                }
                else
                    yield return null;

            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void OnDisable()
    {
        tr.Clear();
    }
    private void OnDestroy()
    {
        tr.Clear();
    }
}
