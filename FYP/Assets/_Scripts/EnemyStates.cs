using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    private EnemyHandler stats;
    public enum state
    {
        idle,
        patrol,
        chase,
        dead,
        dummy
    }

    public state currentState;
    Transform[] waypoints;
    [Header("Agent")] [SerializeField] float stoppingDistance;
    [SerializeField] private float sightRange;
    [SerializeField] Transform target;
    [SerializeField] int targetWaypointIndex = 0;

    NavMeshAgent agent;
    Animator anim;
    SpriteRenderer characterSprite;
    bool facingRight = true;
    float previousXPos;
    Vector3Int rotationLock = new Vector3Int(0, 0, 0);
    [SerializeField] bool isDummy = false;

    [Header("Attack")] [SerializeField] private float cooldown;
    [SerializeField] private float attackRange;
    private float attackTimer = 3;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        stats = GetComponentInChildren<EnemyHandler>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        characterSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (currentState != state.dummy)
        {
            agent = GetComponent<NavMeshAgent>();
            previousXPos = transform.position.x;
            currentState = state.idle;

            yield return new WaitForSecondsRealtime(2);

            waypoints = new Transform[GameObject.FindGameObjectsWithTag("Waypoint").Length];
            int i = 0;
            foreach (GameObject x in GameObject.FindGameObjectsWithTag("Waypoint"))
            {
                waypoints[i] = x.transform;
                i++;
            }

            currentState = state.patrol;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != state.dead)
            currentState = isDummy ? state.dummy : currentState;
                        

        switch (currentState)
        {
            case state.idle:
                agent.enabled = false;
                break;

            case state.patrol:
                agent.enabled = true;

                targetWaypointIndex =
                    Vector3.Distance(transform.position, waypoints[targetWaypointIndex].position) > stoppingDistance
                        ? targetWaypointIndex
                        : Random.Range(0, waypoints.Length);


                target = waypoints[targetWaypointIndex];

                agent.SetDestination(target.position);
                break;

            case state.chase:
                agent.enabled = true;
                target = PlayerManager.Instance.playerGameObject.transform;
                agent.SetDestination(target.position);

                if (Vector3.Distance(transform.position, PlayerManager.Instance.playerGameObject.transform.position) <
                    attackRange)
                {
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0)
                    {
                        PlayerManager.Instance.takeDamage(stats.getEnemy().damage);
                        attackTimer = cooldown;
                    }
                }
                else attackTimer = cooldown;

                break;

            case state.dead:
                anim.SetTrigger("Death");
                foreach (Transform x in transform.GetChild(0).transform)
                    Destroy(x.gameObject);
                Destroy(agent);
                Destroy(GetComponent<Rigidbody>());
                if (GameObject.Find("Dropped Items"))
                    Instantiate(GetComponentInChildren<EnemyHandler>().getEnemy().drop, transform.position,
                        new Quaternion(), GameObject.Find("Dropped Items").transform);
                else
                    Instantiate(GetComponentInChildren<EnemyHandler>().getEnemy().drop, transform.position,
                        new Quaternion(), transform.parent);
                Destroy(this);
                break;

            case state.dummy:
                break;
        }

        if (Vector3.Distance(PlayerManager.Instance.playerGameObject.transform.position, transform.position) <
            sightRange) currentState = state.chase;

        if (currentState != state.dummy && currentState != state.dead)
        {
            anim.SetFloat("velocity", Mathf.Abs(agent.velocity.x));

            characterSprite.sortingOrder = Mathf.RoundToInt(transform.position.z * -100);
            characterSprite.flipX = Mathf.Sign(agent.velocity.x) == 1 ? false : true;

            if (transform.position.x < previousXPos && facingRight == true)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
                facingRight = false;
                //atk.flipHitCheckOffset();
            }

            if (transform.position.x > previousXPos && facingRight == false)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
                facingRight = true;
                //atk.flipHitCheckOffset();
            }

            transform.eulerAngles = rotationLock;

            previousXPos = transform.position.x;
        }
    }

    public void die() => currentState = state.dead;
}