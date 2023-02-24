using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float flinchPercentage = 0.2f;
    public bool isAlive = true;
    public float HP = 100.0f;//base
    public int pointsGiven = 100;//How many points in the score this enemy gives upon death

    public GameObject enemyMeshGO; //Enemy itself, or its child, that contains its mesh renderer

    Animator anim;
    AudioSource enemyAudioSource;
    public AudioClip deathSound;
        

    void EnemyDied()
    {
        isAlive = false;
        //Run death animation
        anim.SetTrigger("EnemyDied");

        //TODO: Give player points related to this enemy
        UIManager.instance.AddValueToScore(pointsGiven);

        //See if enemy dropped any ammo or health
        gameObject.GetComponent<EnemyDropManager>().CheckForAmmoDrops();
        gameObject.GetComponent<EnemyDropManager>().CheckForHealthDrop();

        //run player death sound
        enemyAudioSource.Stop();
        enemyAudioSource.PlayOneShot(deathSound);

        //deactivate its player controller 
        if(gameObject.GetComponent<PlayerController>()!=null)
            gameObject.GetComponent<PlayerController>().enabled = !gameObject.GetComponent<PlayerController>().enabled;

        //Finally, destroy enemy after a certain amount of time
        GameObject.Destroy(this.gameObject, 15.0f);
    }

    

    public void DamageEnemy(float damageTaken)
    {
        HP -= damageTaken;
        if (isAlive)//only checks for damage/flinch if it's alive
        {
            if (HP <= 0.0f)
            {
                EnemyDied();
                if(GetComponent<EnemySoldierAI>()!=null)
                    GetComponent<EnemySoldierAI>().SoldierFall();
            }
            else
            {
                //check if flinched
                if (Random.Range(0.0f, 1.0f) < flinchPercentage)
                {
                    //flinched
                    //TODO: Cut and add 'flinch' animation from Animatii folder
                    anim.SetTrigger("Flinched");
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        enemyAudioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
