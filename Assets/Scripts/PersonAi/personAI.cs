using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class personAI : MonoBehaviour
{
    [HideInInspector] public PatrolStatePerson patrolState;
    [HideInInspector] public AlertStatePerson alertState;
    [HideInInspector] public AttackStatePerson attackState;
    [HideInInspector] public IEnemyStatePerson currentState;
    
    [HideInInspector] public GameObject zombieComming;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator personAnim;
    [HideInInspector] public Transform[] waypoints;

    public AudioClip hittedSound;
    public float life;
    public GameObject zombiePrefab;
    public GameObject waypointsParent;
    public GameObject personDeadParticles;

    private Rigidbody rgZombie;
    private AudioSource personAS;
    private bool dead;

    void Start()
    {
        personAnim = GetComponent<Animator>();
        rgZombie = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        personAS = GetComponent<AudioSource>();
        personAS.volume = PlayerPrefs.GetFloat("SoundsVolume", 1);

        waypoints = new Transform[waypointsParent.transform.childCount];
        for (var i = 0; i < waypointsParent.transform.childCount; i++)
        {
            waypoints[i] = waypointsParent.transform.GetChild(i);
        }

        patrolState = new PatrolStatePerson(this);
        alertState = new AlertStatePerson(this);
        attackState = new AttackStatePerson(this);
        currentState = patrolState;
    }

    void Update()
    {
        if (life <= 0 && !dead)
        {
            dead = true;
            rgZombie.isKinematic = true;
            personAnim.SetTrigger("Dead");
            if (PlayerPrefs.GetInt("ParticleSystem", 1) == 1)
            {
                var particles = Instantiate(personDeadParticles, transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), transform);
                Destroy(particles, 1f);
            }
            Destroy(gameObject, 1.5f);
            Instantiate(zombiePrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }
        else currentState.UpdateState();
    }

    public void Hit(float damage)
    {
        life = 0;
        personAS.PlayOneShot(hittedSound);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemie")
        {
            zombieComming = col.gameObject;
        }
        if (!dead) currentState.OnTriggerEnter(col);
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Enemie")
        {
            zombieComming = col.gameObject;
        }
        if (!dead) currentState.OnTriggerStay(col);
    }

    private void OnTriggerExit(Collider col)
    {
        if(!dead) currentState.OnTriggerExit(col);
    }
}
