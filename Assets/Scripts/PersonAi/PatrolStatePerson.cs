using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStatePerson : IEnemyStatePerson
{
    personAI myPerson;
    private int nextWayPoint = 0;
    private bool[] visitedWaypoint;

    public PatrolStatePerson(personAI person)
    {
        myPerson = person;
        visitedWaypoint = new bool[myPerson.waypoints.Length];
        for (int i = 0; i < visitedWaypoint.Length; i++)
        {
            visitedWaypoint[i] = false;
        }
    }

    public void UpdateState()
    {
        myPerson.navMeshAgent.destination = myPerson.waypoints[nextWayPoint].position;
        var distance = Vector3.Distance(myPerson.navMeshAgent.gameObject.transform.position, myPerson.waypoints[nextWayPoint].position);
        if (distance <= 3f && !visitedWaypoint[nextWayPoint])
        {
            visitedWaypoint[nextWayPoint] = true;
            nextWayPoint++;
            if (nextWayPoint >= myPerson.waypoints.Length)
            {
                nextWayPoint = 0;
                for (int i = 0; i < visitedWaypoint.Length; i++)
                {
                    visitedWaypoint[i] = false;
                }
            }
        }
    }

    public void Impact() { }

    public void GoToAlertState()
    {
        myPerson.navMeshAgent.speed = 1f;
        myPerson.personAnim.speed = 1f;
        myPerson.personAnim.SetBool("running", true);
        myPerson.currentState = myPerson.alertState;
    }

    public void GoToAttackState() { }

    public void GoToPatrolState(){}

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemie")
        {
            GoToAlertState();
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Enemie")
        {
            GoToAlertState();
        }
    }

    public void OnTriggerExit(Collider col){}
}
