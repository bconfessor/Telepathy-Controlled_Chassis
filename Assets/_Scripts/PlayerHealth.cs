using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public static PlayerHealth instance = null;
    public GameObject[] leftArmGOs, rightArmGOs; //real arms' parts
    public GameObject leftJointGO, rightJointGO; //Joints GOs
    public GameObject copyLeftArmGO, copyRightArmGO;//Fake arms, to be used to fall to the ground

    Animator anim;

    public float playerHealth = 100.0f;
    public float playerMaxHealth = 100.0f;
    public float flinchPercentage = 0.1f;

    bool leftArmDestroyed, rightArmDestroyed = false;
    public bool isAlive = true;
    
    public void PlayerDied()
    {
        //make it not be alive anymore, triggers the death animation/explosions and defeat screen
        isAlive = false;

        //Activate Trigger on Animator
        anim.SetTrigger("playerDied");

        //TODO: Trigger defeat explosions in mech

        //TODO: Trigger defeat screen
        VictoryDefeatManagement.instance.PlayerLostGame();
    }

    public void RestorePlayerHealth(float healthToRecover)
    {
        if (playerHealth + healthToRecover >= playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        else
            playerHealth += healthToRecover;
    }

    public void DamagePlayer(float damageTaken)
    {
        if (isAlive)
        {
            playerHealth -= damageTaken;

            //Destroy right arm if life is low enough
            if(playerHealth<=65.0f && !rightArmDestroyed)
            {
                rightArmDestroyed = true;
                DestroyRightArm();
            }

            //Destroy left arm if life is low enough
            if (playerHealth <= 30.0f && !leftArmDestroyed)
            {
                leftArmDestroyed = true;
                DestroyLeftArm();
            }



            if (playerHealth <= 0.0f && isAlive)//don't do it if it's already dead
            {
                playerHealth = 0.0f;
                PlayerDied();
            }
            else
            {
                //check if flinched
                if (Random.Range(0.0f, 1.0f) < flinchPercentage)
                {
                    //flinched
                    anim.SetTrigger("flinched");
                }
            }
        }
        //At the end, update UI
        UIManager.instance.ChangeCurrentHPUIValue(playerHealth);
    }


    void DestroyLeftArm()
    {
        //TODO: Make explosion FX at Joint location

        //turn off arm
        foreach (GameObject leftArmGO in leftArmGOs)
        {
            leftArmGO.SetActive(false);
        }
        //Turn on fake arm
        copyLeftArmGO.GetComponent<ArmFollowScript>().DestroyArm();
        
        //Finally, activate joint GO
        leftJointGO.SetActive(true);
        
    }


    void DestroyRightArm()
    {
        //TODO: Make explosion FX at Joint location

        //turn off arm
        foreach (GameObject rightArmGO in rightArmGOs)
        {
            rightArmGO.SetActive(false);
        }
        //Turn on fake arm
        copyRightArmGO.GetComponent<ArmFollowScript>().DestroyArm();
        
        //Finally, activate joint GO
        rightJointGO.SetActive(true);
    }



    // Use this for initialization
    void Start () {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;

        anim = GetComponent<Animator>();

        //Load UI
        UIManager.instance.ChangeCurrentHPUIValue(playerHealth);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Destroy left arm");
            DestroyLeftArm();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Destroy Right Arm");
            DestroyRightArm();
        }
    }
}
