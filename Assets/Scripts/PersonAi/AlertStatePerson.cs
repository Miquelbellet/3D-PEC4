using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertStatePerson : IEnemyStatePerson
{
    personAI myPerson;

    public AlertStatePerson(personAI person)
    {
        myPerson = person;
    }

    public void UpdateState()
    {
        if (myPerson.zombieComming)
        {
            float distance = Vector3.Distance(myPerson.zombieComming.transform.position, myPerson.transform.position);
            if (distance >= 1f)
            {
                Vector3 dirToPlayer = myPerson.transform.position - myPerson.zombieComming.transform.position;
                Vector3 newPos = myPerson.transform.position + dirToPlayer;
                myPerson.navMeshAgent.SetDestination(newPos);
            }
        }
    }

    public void Impact() { }

    public void GoToAlertState() { }

    public void GoToAttackState()
    {
        myPerson.navMeshAgent.speed = 0.5f;
        myPerson.personAnim.speed = 1f;
        myPerson.currentState = myPerson.attackState;
    }

    public void GoToPatrolState()
    {
        myPerson.navMeshAgent.speed = 0.5f;
        myPerson.personAnim.speed = 1f;
        myPerson.personAnim.SetBool("running", false);
        myPerson.currentState = myPerson.patrolState;
    }

    public void OnTriggerEnter(Collider col) { }
    public void OnTriggerStay(Collider col) { }
    public void OnTriggerExit(Collider col)
    {
        GoToPatrolState();
    }
}
