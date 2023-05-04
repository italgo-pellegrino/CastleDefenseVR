using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script that implement that a Enemy could be hit and killed
 */
public class Target : MonoBehaviour
{
    public float startLifePoints = 100;
    protected float lifePoints;
    public bool destroyOnDeath = true;

    private void Awake()
    {
        lifePoints = startLifePoints;
    }

    public virtual void Hit(float damage)
    {
        lifePoints -= damage;
        if(lifePoints <= 0)
        {
            Die();
        }
       //Debug.Log(lifePoints);
    }

    public void Die()
    {
        if (destroyOnDeath)
        {
            Destroy(gameObject);
            FindObjectOfType<GameManager>().EnemyDied();
        }
    }
}
