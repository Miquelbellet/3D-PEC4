using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStatePerson : IEnemyStatePerson
{
    personAI myPerson;

    public AttackStatePerson(personAI person)
    {
        myPerson = person;
    }

    public void UpdateState() { }

    public void Impact() { }

    public void GoToAttackState() { }
    
    public void GoToPatrolState() {
        myPerson.navMeshAgent.speed = 0.5f;
        myPerson.personAnim.speed = 1f;
        myPerson.currentState = myPerson.patrolState;
    }

    public void GoToAlertState() {
        myPerson.currentState = myPerson.alertState;
    }

    public void OnTriggerEnter(Collider col) { }

    public void OnTriggerStay(Collider col) { }

    public void OnTriggerExit(Collider col)
    {
        GoToPatrolState();
    }
}
