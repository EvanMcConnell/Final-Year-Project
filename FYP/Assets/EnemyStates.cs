using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    enum state { idle, chase, dead }
    [SerializeField] state currentState;
    NavMeshAgent agent;
    [SerializeField] Transform target;
    Animator anim;
    SpriteRenderer characterSprite;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        characterSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) {
            case state.idle:

                break;

            case state.chase:
                agent.SetDestination(target.position);
                break;

            case state.dead:
                anim.SetTrigger("Death");
                foreach (Transform x in transform.GetChild(0).transform)
                    Destroy(x.gameObject);
                Destroy(agent);
                Destroy(this);
                break;
        }

        anim.SetFloat("velocity", Mathf.Abs(agent.velocity.x));

        characterSprite.sortingOrder = Mathf.RoundToInt(transform.position.z * -100);
        characterSprite.flipX = Mathf.Sign(agent.velocity.x) == 1 ? false : true;
        //transform.eulerAngles = Mathf.Sign(agent.velocity.x) == 1 ?
        //    new Vector3(0, 0, 0) :
        //    new Vector3(0, 0, 180);
        transform.eulerAngles = new Vector3Int();
    }

    public void die() => currentState = state.dead;
}
