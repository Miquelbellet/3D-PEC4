using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    enemyAI myEnemy;
    float actualTimeBetweenAttacks = 0;

    public AttackState(enemyAI enemy)
    {
        myEnemy = enemy;
    }

    public void UpdateState()
    {
        if (myEnemy.zombieTarget)
        {
            actualTimeBetweenAttacks += Time.deltaTime;
            float distance = Vector3.Distance(myEnemy.zombieTarget.transform.position, myEnemy.transform.position);
            if (distance < 1f)
            {
                Vector3 lookDirection = myEnemy.zombieTarget.transform.position - myEnemy.transform.position;
                myEnemy.transform.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(lookDirection.x, 0, lookDirection.z));
                if (actualTimeBetweenAttacks > myEnemy.timeBetweenAttacks)
                {
                    actualTimeBetweenAttacks = 0;
                    myEnemy.zombieAnim.SetTrigger("Punch");
                    myEnemy.GetComponent<AudioSource>().PlayOneShot(myEnemy.attackSound);

                    if (myEnemy.zombieTarget.tag == "Player") myEnemy.zombieTarget.GetComponent<PlayerController>().playerGetHit(myEnemy.damageForce);
                    if (myEnemy.zombieTarget.tag == "Person") myEnemy.zombieTarget.GetComponent<personAI>().Hit(myEnemy.damageForce);
                }
            }
            else
            {
                GoToAlertState();
            }
        }
        else GoToPatrolState();
    }

    public void Impact() { }

    public void GoToAttackState() { }
    
    public void GoToPatrolState() {
        myEnemy.navMeshAgent.speed = 0.6f;
        myEnemy.zombieAnim.speed = 1.5f;
        myEnemy.currentState = myEnemy.patrolState;
    }

    public void GoToAlertState()
    {
        myEnemy.currentState = myEnemy.alertState;
    }

    public void OnTriggerEnter(Collider col) { }

    public void OnTriggerStay(Collider col) { }

    public void OnTriggerExit(Collider col)
    {
        GoToPatrolState();
    }
}
