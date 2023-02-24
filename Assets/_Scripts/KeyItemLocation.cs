using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemLocation : MonoBehaviour {

    //if true and player enters trigger, give correspondent key item to them
    public bool hasKeyItem = false;

    public bool hasBeenActivated = false;//so player doesn't access same location multiple times

    AudioSource aSource;
    public AudioClip locationEnterSound;

    public void OnTriggerEnter(Collider other)
    {
        if (!hasBeenActivated && other.gameObject.tag == "Player")
        {
            hasBeenActivated = true;

            //play sound
            aSource.PlayOneShot(locationEnterSound);

            //check if spawn location contains some item
            if (hasKeyItem)
            {
                hasKeyItem = false;//no more item in this location
                //if so, check which item player got based on gameobject tag
                //TODO: For each case, change boolean in collectiblesManager
                Debug.Log("Current object touched is " + gameObject.tag);
                switch (gameObject.tag)
                {
                    //retina scanner gate path
                    case "QuantumNotebookTrigger":
                        Debug.Log("Got Quantum computer!");
                        CollectiblesManager.instance.GotQuantumComputer();
                        break;

                    case "CypherKeyTrigger":
                        Debug.Log("Got a new Cypher Key!");
                        CollectiblesManager.instance.GotNewCypherKey();
                        break;



                    //boulder path
                    case "ExplosiveTrigger":
                        Debug.Log("Got Explosives!");
                        CollectiblesManager.instance.GotExplosive();
                        break;

                    case "RemoteControlTrigger":
                        Debug.Log("Got Remote Control!");
                        CollectiblesManager.instance.GotRemoteControl();
                        break;


                    //military path
                    case "OverclockCircuitTrigger":
                        Debug.Log("Got Overclock Circuit!");
                        CollectiblesManager.instance.GotOverclockCircuit();
                        break;


                    default:
                        Debug.Log("nothing yet(BUT THIS SHOULDN'T HAPPEN!" + gameObject.tag);
                        break;
                }
            }
            else
            {
                //check if it's another key trigger
                switch (gameObject.tag)
                {
                    //retina scanner gate path
                    case "RetinaGateTrigger":
                        Debug.Log("Started hacking retina scanner!");
                        CollectiblesManager.instance.StartedHack();
                        break;



                    //boulder path
                    case "BoulderExplosionLocation":
                        Debug.Log("Blew up boulders!");
                        CollectiblesManager.instance.ExplodedBoulders();
                        break;



                    //military path
                    case "FuseBoxTrigger":
                        Debug.Log("Placed overclock circuit in Fuse Box and blew up power plant!");
                        CollectiblesManager.instance.ExplodedPowerPlant();
                        break;


                    case "EscapeRoute":
                        Debug.Log("Won the match!!");
                        CollectiblesManager.instance.ReachedEscapeRoute();
                        break;

                    default:
                        CollectiblesManager.instance.EnteredEmptyLocation(gameObject.tag);
                        Debug.Log("Nothing of interest in this location... Try another one!" + gameObject.tag);
                        break;

                }
            }

            //after being used, deactivate this ring
            gameObject.SetActive(false);
        }
    }

 


    // Use this for initialization
    void Start () {
        hasBeenActivated = false;
        aSource = GameObject.Find("PlayerMiscAudioSource").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
