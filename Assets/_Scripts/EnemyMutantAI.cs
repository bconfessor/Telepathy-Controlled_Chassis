using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMutantAI : MonoBehaviour {

    public GameObject playerGO;

    EnemyHealth myHealthScript;
    Animator anim;
    AudioSource enemyAudioSource;
    public float fieldOfViewDistance = 50.0f;
    public float fieldOfAttackDistance = 2.0f;
    public float normalAnimatorSpeed = 1.4f;

    public float enemyAttackPower = 5.0f;
    public bool enemyIsAttacking = false;
    public float enemyMovementSpeed = 5.0f;

    public AudioClip attackSound;

    bool PlayerIsWithinSight()
    {
        //checks if the distance between this enemy and player is smaller than the maximum distance the enemy can see
        return Vector3.Distance(transform.position, playerGO.transform.position) <= fieldOfViewDistance;
    }

    bool PlayerIsWithinAttackDistance()
    {
        //checks if the distance between this enemy and player is smaller than the maximum distance the enemy can shoot
        return Vector3.Distance(transform.position, playerGO.transform.position) <= fieldOfAttackDistance;
    }

    IEnumerator EnemyAttack()
    {
        //only actually does attack animation and damage player if enemy is not attacking at the moment
        if(!enemyIsAttacking)
        {
            enemyIsAttacking = true;
            
            enemyAudioSource.Stop();
            enemyAudioSource.PlayOneShot(attackSound);

            PlayerHealth.instance.DamagePlayer(enemyAttackPower);
            //Enemy attack animation
            anim.SetTrigger("PlayerWithinReach");

            //2s, because I said so
            yield return new WaitForSeconds(2.0f);

            //AFTER ALL THIS, THEN enemy may attack again
            enemyIsAttacking = false;
        }
    }

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.speed = normalAnimatorSpeed;
        myHealthScript = this.gameObject.GetComponent<EnemyHealth>();
        playerGO = GameObject.FindGameObjectWithTag("Player");

        enemyAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myHealthScript.isAlive)
        {
            if (PlayerIsWithinSight())
            {
                //make enemy run
                anim.SetBool("PlayerWithinSight", true);

                //Make enemy face player
                transform.LookAt(playerGO.transform);

                //if it's close enough to shoot...
                if (PlayerIsWithinAttackDistance())
                {
                    anim.SetBool("PlayerWithinSight", false);
                    StartCoroutine(EnemyAttack());

                }
                //if it's not...
                else
                {
                    //and just move enemy towards player
                    //MoveEnemyTowardsPlayer();

                }
            }
            else
            {
                //away from everything
                anim.SetBool("PlayerWithinSight", false);
            }
        }
    }
}
