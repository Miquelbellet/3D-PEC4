using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAiScript : MonoBehaviour
{
    public float zombieDamage;
    public float zombieLife;
    public float destinationUpdateTime;
    public float foundPlayerRange;
    public float attackRange;
    public float attackCurrency;
    public float zombieDestinationDistance;
    public GameObject zombieHitParticles;
    public GameObject zombieDeadParticles;

    private NavMeshAgent navMeshZombie;
    private Animator zombieAnim;
    private Rigidbody rgZombie;
    private GameObject player;
    private float time;
    private float attackingTimer;
    private bool nearPlayer;
    private bool arrivedInDestination;
    private Vector3 destinationPos;
    private bool dead;
    private float distance;

    void Start()
    {
        navMeshZombie = GetComponent<NavMeshAgent>();
        zombieAnim = GetComponent<Animator>();
        rgZombie = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        SetDestination();
    }

    void Update()
    {
        time += Time.deltaTime;
        if ((time >= destinationUpdateTime && !nearPlayer) || arrivedInDestination)
        {
            SetDestination();
        }
        var destinationDistance = Vector3.Distance(transform.position, destinationPos);
        if (destinationDistance <= 0.2f) arrivedInDestination = true;
        else arrivedInDestination = false;

        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < foundPlayerRange)
        {
            nearPlayer = true;
            navMeshZombie.destination = player.transform.position;
            navMeshZombie.speed = 1.6f;
            zombieAnim.speed = 2.5f;
            if (distance < attackRange)
            {
                attackingTimer += Time.deltaTime;
                if (attackingTimer > attackCurrency && !dead)
                {
                    attackingTimer = 0;
                    zombieAnim.SetTrigger("Punch");
                    player.GetComponent<PlayerController>().playerGetHit(zombieDamage);
                }
            }
        }
        else
        {
            if (nearPlayer)
            {
                nearPlayer = false;
                SetDestination();
                navMeshZombie.speed = 0.6f;
                zombieAnim.speed = 1.5f;
            }
        }
    }

    private void SetDestination()
    {
        float randomX = Random.Range(-zombieDestinationDistance, zombieDestinationDistance);
        float randomZ = Random.Range(-zombieDestinationDistance, zombieDestinationDistance);
        destinationPos = new Vector3(randomX, 0, randomZ);
        navMeshZombie.destination = destinationPos;
        time = 0;
    }

    private void ReactivateWalking()
    {
        navMeshZombie.isStopped = false;
    }

    public void ZombieHitted(float damage)
    {
        if (!dead)
        {
            navMeshZombie.isStopped = true;
            Invoke("ReactivateWalking", 1.5f);
            zombieLife -= damage;
            if (zombieLife < 0)
            {
                dead = true;
                rgZombie.isKinematic = true;
                zombieAnim.SetTrigger("Dead");
                var particles = Instantiate(zombieDeadParticles, transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), transform);
                Destroy(particles, 1.5f);
                player.GetComponent<ExtraItemsScript>().DropRandomItem(transform.position);
                Destroy(gameObject, 3f);
            }
            else
            {
                zombieAnim.SetTrigger("Hitted");
                var particles = Instantiate(zombieHitParticles, transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), transform);
                Destroy(particles, 3f);
            }
        }
    }
}
