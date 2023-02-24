using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

    public static EnemySpawnManager instance = null;
    public int currentAmountOfEnemies = 0; //current amount of enemies in the scene
    public int maxAmountOfEnemies = 30; //max amount of enemies allowed in the scene

    public GameObject[] spawnLocations;//locations where enemies can spawn
    public GameObject mutantEnemyGO, giantMutantEnemyGO, zombieEnemyGO, soldierEnemyGO;




    //time it takes to check if enemies have dropped below max, so it can spawn more
    public float enemyCheckCooldown = 10.0f;

    //time it takes to create another enemy
    public float enemySpawnCooldown = 2.0f;


    IEnumerator PerformCheckAndSpawn()
    {
        //Debug.Log("Checking for enemies...");
        //performs check of how many enemies are currently spawned in the scene
        currentAmountOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        //Debug.Log("Found " + currentAmountOfEnemies + " enemies.");
        
        if (currentAmountOfEnemies < maxAmountOfEnemies)
        {
            //spawn a new enemy at one of the spawn locations
            int spawnIndex = Random.Range(0, spawnLocations.Length-1);
            int enemyToSpawn = Random.Range(0, 3);
            GameObject newEnemy=null;
            if(enemyToSpawn==0)
            {
                //spawn soldier
                newEnemy = Instantiate(soldierEnemyGO);
                //Debug.Log("Spawn soldier");
            }
            else if (enemyToSpawn==1)
            {
                //spawn mutant
                newEnemy = Instantiate(mutantEnemyGO);
                //Debug.Log("Spawn Mutant");

            }
            else if(enemyToSpawn==2)
            {
                //spawn zombie
                newEnemy = Instantiate(zombieEnemyGO);
                //Debug.Log("Spawn Zombie");
            }
            else//enemyToSpawn==3
            {
                //spawn giant mutant
                newEnemy = Instantiate(giantMutantEnemyGO);
                //Debug.Log("Spawn Giant Mutant");
            }
            //either way, change its tranform to that of the spawn location
            newEnemy.transform.position = spawnLocations[spawnIndex].transform.position;

            //after spawning one, wait a while before doing another check
            yield return new WaitForSeconds(enemySpawnCooldown);
        }
        else//Already full of enemies; wait a while before doing another check
            yield return new WaitForSeconds(enemyCheckCooldown);

        // at the end, run check again, infinitely
        StartCoroutine(PerformCheckAndSpawn());

    }


	// Use this for initialization
	void Start () {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        StartCoroutine(PerformCheckAndSpawn());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
