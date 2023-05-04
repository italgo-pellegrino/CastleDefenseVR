using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * This Script direct the Enemys to the targeted Position (Castle).
 */
public class SimpleAI : Target
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private AudioSource audioSource;
    
    //Setup the navMeshAgent 
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnemyGoal target = FindObjectOfType<EnemyGoal>();
        navMeshAgent.SetDestination(target.transform.position);
    }

  
    void Update()
    {
        transform.LookAt(navMeshAgent.steeringTarget);
    }

    //If Enemy get hitted it will show a hit animation if he survive the hit or a death animation if he dies
    public override void Hit(float damage)
    {
        lifePoints -= damage;
        if(lifePoints >= 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            animator.SetBool("Dead", true);
            navMeshAgent.isStopped = true;
            GetComponent<Collider>().enabled = false;
            foreach ( Arrow arrow in GetComponentsInChildren<Arrow>())
            {
                Destroy(arrow.gameObject);
            }
            FindObjectOfType<GameManager>().EnemyDied();
        }
    }
}
