using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public IEnemyState currentState;
    
    [HideInInspector] public GameObject zombieTarget;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator zombieAnim;
    [HideInInspector] public Transform[] waypoints;

    public AudioClip attackSound;
    public float life;
    public float timeBetweenAttacks;
    public float damageForce;
    public GameObject waypointsParent;
    public GameObject zombieDeadParticles;
    public GameObject zombieHitParticles;

    private GameObject player;
    private Rigidbody rgZombie;
    private bool dead;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        zombieAnim = GetComponent<Animator>();
        rgZombie = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        currentState = patrolState;

        waypoints = new Transform[waypointsParent.transform.childCount];
        for (var i = 0; i < waypointsParent.transform.childCount; i++)
        {
            waypoints[i] = waypointsParent.transform.GetChild(i);
        }
    }

    void Update()
    {
        if (life <= 0 && !dead)
        {
            dead = true;
            rgZombie.isKinematic = true;
            zombieAnim.SetTrigger("Dead");
            var particles = Instantiate(zombieDeadParticles, transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), transform);
            Destroy(particles, 1.5f);
            player.GetComponent<ExtraItemsScript>().DropRandomItem(transform.position);
            Destroy(gameObject, 3f);
        }
        else currentState.UpdateState();
    }

    public void Hit(float damage)
    {
        navMeshAgent.isStopped = true;
        Invoke("ReactivateWalking", 1.5f);
        life -= damage;
        zombieAnim.SetTrigger("Hitted");
        var particles = Instantiate(zombieHitParticles, transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), transform);
        Destroy(particles, 3f);
        currentState.Impact();
    }

    private void ReactivateWalking()
    {
        navMeshAgent.isStopped = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Person")
        {
            zombieTarget = col.gameObject;
        }
        if (!dead) currentState.OnTriggerEnter(col);
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Person")
        {
            zombieTarget = col.gameObject;
        }
        if (!dead) currentState.OnTriggerStay(col);
    }

    private void OnTriggerExit(Collider col)
    {
        if(!dead) currentState.OnTriggerExit(col);
    }
}
