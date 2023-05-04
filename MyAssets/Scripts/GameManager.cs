using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(AudioSource))]

/**
 * This Script does the Mainpart of the Gamelogic
 */
public class GameManager : MonoBehaviour
{
    public AudioClip sfxBackgroundMusic;
    public AudioClip sfxEnemyAttackedCastle;
    public AudioClip sfxWin;
    public AudioClip sfxLose;
    public Text uiGameStatus;
    private AudioSource audioSource;


    [SerializeField] private GameObject Enemy = null;
    [SerializeField] private Transform SpawnPosEast = null;
    [SerializeField] private Transform SpawnPosNorth = null;
    [SerializeField] private Transform SpawnPosWest = null;

    //If zero and every Enemy is allready started the Game is Over. Don't change
    private int enemysAlive = 0; 
    private bool lastEnemyStarted = false;

    //When castleHealth reaches 0 you lose
    [SerializeField] private int castleHealth = 100;

    //When enemyHealth reach 0 the enemyunit die, add a random Value between addMinHealth and addMaxHealth
    [SerializeField] private int enemyHealth = 10;
    [SerializeField] private int addMinHealth = 0;
    [SerializeField] private int addMaxHealth = 20;

    //The amount of enemys that spawn. Each Unit spawns between minTimeToSpawn and maxTimeToSpawn in Seconds
    [SerializeField] private int enemyNumbers = 10;
    [SerializeField] private int minTimeToSpawn = 1;
    [SerializeField] private int maxTimeToSpawn = 6;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sfxBackgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
        StartCoroutine(waiter());     
        UpdateStatus();
    }

    //Start the Spawn and a random time interval
    IEnumerator waiter()
    {
        while (enemyNumbers > 0)
        {
            int random_number = new Random().Next(minTimeToSpawn, maxTimeToSpawn);           
            instantiateEnemy();
            enemysAlive++;
            enemyNumbers--;
            yield return new WaitForSeconds(random_number);
            UpdateStatus();
        }
        lastEnemyStarted = true;       
    }

    //Updated the Status that get displayed
    private void UpdateStatus()
    {          
        if (castleHealth <= 0)
        {
            uiGameStatus.text = "Game Over!";
            audioSource.clip = sfxLose;
            audioSource.loop = false;
            audioSource.Play();
        }
        else if (enemysAlive <= 0 && lastEnemyStarted == true)
        {
            uiGameStatus.text = "You Won!";
            audioSource.clip = sfxWin;
            audioSource.loop = false;
            audioSource.Play();
        }
        else 
        {
            uiGameStatus.text = "Castle: " + castleHealth + "% Enemies Left: " + enemysAlive;
        }     
    }
    //When Enemy dies the Status get Updated
   public void EnemyDied()
    {
        enemysAlive--;
        UpdateStatus();
    }

    //When the enemy reaches the castle, it damage the castle till you lose
    public void EnemyAttackedCastle()
    {
        castleHealth -= 20;
        enemysAlive--;
        audioSource.PlayOneShot(sfxEnemyAttackedCastle);      
        UpdateStatus();
        if(castleHealth <= 0)
        {
            Target[] enemys = FindObjectsOfType<Target>();
            foreach(Target e in enemys)
            {
                Destroy(e);
            }
        }
    }

    //Method to instantiate the Enemy in a random position near 3 Spawnpoints
    public void instantiateEnemy()
    {
        int random_number = new Random().Next(1, 4); // Generates a number between 1 to 3
        Vector3 randomSpawn;
        if (random_number.Equals(1))
        {
            randomSpawn = SpawnPosEast.transform.position;
        }else if (random_number.Equals(2))
        {
            randomSpawn = SpawnPosNorth.transform.position;
        }
        else
        {
            randomSpawn = SpawnPosWest.transform.position;
        }
        GameObject.Instantiate(Enemy, randompositionNear(randomSpawn), Quaternion.identity);
        Enemy.GetComponent<SimpleAI>().startLifePoints = enemyHealth + new Random().Next(addMinHealth,addMaxHealth);     
    }
    
    //Method that returns a randomposition near the input position.
    public Vector3 randompositionNear(Vector3 pos)
    {
        float random_floatX = new Random().Next(-5, 5);
        float random_floatY = new Random().Next(-5, 5);
        float random_floatZ = new Random().Next(-5, 5);
        return new Vector3(pos.x+random_floatX, pos.y + random_floatY, pos.z + random_floatZ); 
    }
}
