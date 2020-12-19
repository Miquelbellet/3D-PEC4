using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        var maxDist = Mathf.Infinity;
        GameObject closePerson = null;
        float distance = Mathf.Infinity;
        GameObject[] personsInGame = GameObject.FindGameObjectsWithTag("Person");
        List<GameObject> tempTanks = personsInGame.ToList();
        tempTanks.Add(player);
        personsInGame = tempTanks.ToArray();

        foreach (GameObject civil in personsInGame)
        {
            distance = Vector3.Distance(transform.position, civil.transform.position);
            if (distance < maxDist)
            {
                closePerson = civil.gameObject;
                maxDist = distance;
            }
        }
        if (closePerson != null && distance < foundPlayerRange)
        {
            nearPlayer = true;
            navMeshZombie.destination = closePerson.transform.position;
            navMeshZombie.speed = 1.6f;
            zombieAnim.speed = 2.5f;
            if (distance < attackRange)
            {
                attackingTimer += Time.deltaTime;
                if (attackingTimer > attackCurrency && !dead)
                {
                    attackingTimer = 0;
                    zombieAnim.SetTrigger("Punch");
                    if (closePerson.tag == "Player")
                    {
                        player.GetComponent<PlayerController>().playerGetHit(zombieDamage);
                    }
                    else
                    {
                        closePerson.GetComponent<PersonAiScript>().personGetHit(zombieDamage);
                    }
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
