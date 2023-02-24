using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierAI : MonoBehaviour {

    public GameObject playerGO;

    EnemyHealth myHealthScript;
    Animator anim;
    AudioSource enemyAudioSource;
    public float fieldOfViewDistance = 30.0f;
    public float fieldOfAttackDistance = 10.0f;
    public float normalAnimatorSpeed = 1.0f;
    
    public float enemyAttackPower = 2.0f;
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

    

    void MoveEnemyTowardsPlayer()
    {
        
        //Make enemy move toward player (transform? Rigidbody? Dunno...)
        transform.position = Vector3.MoveTowards(transform.position, playerGO.transform.position, enemyMovementSpeed * Time.deltaTime);
        
    }

    IEnumerator EnemyAttack()
    {
        //only actually does attack animation and damage player if enemy is not attacking at the moment
        if (!enemyIsAttacking)
        {
            enemyIsAttacking = true;

            enemyAudioSource.Stop();
            enemyAudioSource.PlayOneShot(attackSound);

            PlayerHealth.instance.DamagePlayer(enemyAttackPower);
            //Enemy attack animation
            anim.SetTrigger("PlayerWithinAim");

            //0.5s, because I said so
            yield return new WaitForSeconds(0.5f);

            //AFTER ALL THIS, THEN enemy may attack again
            enemyIsAttacking = false;
        }
    }

    public void SoldierFall()
    {
        StartCoroutine(SoldierFallDown());
    }

    public float timeToFallDown = 0.5f;
    IEnumerator SoldierFallDown()
    {
        float frameUpdate = Time.deltaTime; //time WaitForSeconds waits
        float degreesPerSecond = (90.0f * frameUpdate) / timeToFallDown;
        float counter = 0.0f; //controls while
        while (counter <= timeToFallDown)
        {
            counter += frameUpdate;
            this.transform.Rotate(new Vector3(-degreesPerSecond, 0, 0));

            yield return new WaitForSeconds(frameUpdate);

        }
    }


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        //anim.speed = 0.6f;//make enemies slower than player
        myHealthScript = this.gameObject.GetComponent<EnemyHealth>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        enemyAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (myHealthScript.isAlive && PlayerHealth.instance.isAlive)
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
                    anim.SetTrigger("PlayerWithinAim");

                    StartCoroutine(EnemyAttack());
                }
                //if it's not...
                else
                {
                    //and just move enemy towards player
                    MoveEnemyTowardsPlayer();

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
