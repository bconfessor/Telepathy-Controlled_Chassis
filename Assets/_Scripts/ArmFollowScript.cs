using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmFollowScript : MonoBehaviour {

    public GameObject armToFollow;
    public GameObject copyArmToActivate;
    public bool armWasDestroyed = false;

    public void DestroyArm()
    {
            if (!armWasDestroyed)
            {
                armWasDestroyed = true;//so it only plays once
                //activates child
                copyArmToActivate.SetActive(true);
            }
    }


	// Use this for initialization
	void Start () {
        armWasDestroyed = false;
	}
	


	// Update is called once per frame
	void Update () {
        //just keeps copying position and  rotation of real arm if player is still alive
        if (PlayerHealth.instance.isAlive && !armWasDestroyed)
        {
            transform.position = armToFollow.transform.position;
            transform.rotation = armToFollow.transform.rotation;

        }
        
	}
}
