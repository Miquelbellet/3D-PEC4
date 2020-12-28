using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    enemyAI myEnemy;

    public AlertState(enemyAI enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        if (myEnemy.zombieTarget)
        {
            float distance = Vector3.Distance(myEnemy.zombieTarget.transform.position, myEnemy.transform.position);
            if (distance >= 1f)
            {
                myEnemy.navMeshAgent.destination = myEnemy.zombieTarget.transform.position;
            }
            else
            {
                GoToAttackState();
            }
        }
        else GoToPatrolState();
    }

    public void Impact() { }

    public void GoToAlertState() { }

    public void GoToAttackState()
    {
        myEnemy.navMeshAgent.speed = 1.6f;
        myEnemy.zombieAnim.speed = 1.5f;
        myEnemy.currentState = myEnemy.attackState;
    }

    public void GoToPatrolState()
    {
        myEnemy.navMeshAgent.speed = 0.6f;
        myEnemy.zombieAnim.speed = 1.5f;
        myEnemy.currentState = myEnemy.patrolState;
    }

    public void OnTriggerEnter(Collider col) { }
    public void OnTriggerStay(Collider col) { }
    public void OnTriggerExit(Collider col)
    {
        GoToPatrolState();
    }
}
