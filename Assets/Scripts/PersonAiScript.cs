using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PersonAiScript : MonoBehaviour
{
    public float personLife;
    public float destinationUpdateTime;
    public float foundPlayerRange;
    public float personDestinationDistance;
    public GameObject personHitParticles;
    public GameObject personDeadParticles;

    private NavMeshAgent navMeshPerson;
    private Animator personAnim;
    private Rigidbody rgPerson;
    private GameObject player;
    private float time;
    private bool nearPlayer;
    private bool arrivedInDestination;
    private Vector3 destinationPos;
    private bool dead;
    void Start()
    {
        navMeshPerson = GetComponent<NavMeshAgent>();
        personAnim = GetComponent<Animator>();
        rgPerson = GetComponent<Rigidbody>();
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
        GameObject[] personsInGame = GameObject.FindGameObjectsWithTag("Enemie");
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
            navMeshPerson.destination = player.transform.position;
            navMeshPerson.speed = 1.6f;
            personAnim.speed = 2.5f;
        }
        else
        {
            if (nearPlayer)
            {
                nearPlayer = false;
                SetDestination();
                navMeshPerson.speed = 0.6f;
                personAnim.speed = 1.5f;
            }
        }
    }

    private void SetDestination()
    {
        float randomX = Random.Range(-personDestinationDistance, personDestinationDistance);
        float randomZ = Random.Range(-personDestinationDistance, personDestinationDistance);
        destinationPos = new Vector3(randomX, 0, randomZ);
        navMeshPerson.destination = destinationPos;
        time = 0;
    }

    public void personGetHit(float damage)
    {
        if (!dead)
        {

        }
    }
}
