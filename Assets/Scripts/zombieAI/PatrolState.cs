using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    enemyAI myEnemy;
    private int nextWayPoint = 0;
    private bool[] visitedWaypoint;

    public PatrolState(enemyAI enemy)
    {
        myEnemy = enemy;
        visitedWaypoint = new bool[myEnemy.waypoints.Length];
        for (int i = 0; i < visitedWaypoint.Length; i++)
        {
            visitedWaypoint[i] = false;
        }
    }

    public void UpdateState()
    {
        myEnemy.navMeshAgent.destination = myEnemy.waypoints[nextWayPoint].position;
        var distance = Vector3.Distance(myEnemy.navMeshAgent.gameObject.transform.position, myEnemy.waypoints[nextWayPoint].position);
        try
        {
            if (distance <= 3f && !visitedWaypoint[nextWayPoint])
            {
                visitedWaypoint[nextWayPoint] = true;
                nextWayPoint++;
                if (nextWayPoint >= myEnemy.waypoints.Length)
                {
                    nextWayPoint = 0;
                    for (int i = 0; i < visitedWaypoint.Length; i++)
                    {
                        visitedWaypoint[i] = false;
                    }
                }
            }
        }
        catch{}
    }

    public void Impact() { }

    public void GoToAlertState()
    {
        myEnemy.navMeshAgent.speed = 1.6f;
        myEnemy.zombieAnim.speed = 2.5f;
        myEnemy.currentState = myEnemy.alertState;
    }

    public void GoToAttackState()
    {
        myEnemy.navMeshAgent.speed = 0.6f;
        myEnemy.zombieAnim.speed = 1.5f;
        myEnemy.currentState = myEnemy.attackState;
    }

    public void GoToPatrolState(){}

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Person")
        {
            GoToAlertState();
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Person")
        {
            GoToAlertState();
        }
    }

    public void OnTriggerExit(Collider col){}
}
