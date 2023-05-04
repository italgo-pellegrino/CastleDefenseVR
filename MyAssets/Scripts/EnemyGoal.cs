using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

/**
 * This Script destroy the Enemy if he reaches the Castle
 */
public class EnemyGoal : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Target>() != null)
        {
            gameManager.EnemyAttackedCastle();
            Destroy(other.gameObject);
        }
    }
}
