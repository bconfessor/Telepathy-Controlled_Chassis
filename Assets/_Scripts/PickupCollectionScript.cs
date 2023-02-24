using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCollectionScript : MonoBehaviour {


    public int pickupValue = 0;//amount of units of bullets/missiles/health this pickup has
    AudioSource aSource;
    public AudioClip pickupSound;
    bool pickupActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !pickupActivated)
        {
            //activated pickup
            pickupActivated = true;
            //play pickup sound
            aSource.PlayOneShot(pickupSound);
            //gives player whatever the pickup has
            switch(gameObject.tag)
            {
                case "HealthPickup":
                    //restores player health
                    PlayerHealth.instance.RestorePlayerHealth(pickupValue);
                    //Update UI
                    UIManager.instance.AddHPUIValue(pickupValue);
                    break;

                case "BulletPickup":
                    PlayerWeaponsController.instance.RestoreBulletAmmo(pickupValue);
                    //Update UI
                    UIManager.instance.AddCurrentAmmoUIValue(pickupValue);
                    break;

                case "MissilePickup":
                    //Restores player's missiles
                    PlayerWeaponsController.instance.RestoreMissileAmmo(pickupValue);
                    //Updates UI
                    UIManager.instance.AddCurrentMissileUIValue(pickupValue);
                    break;

                default:
                    Debug.Log("Pickup ERROR: Not supposed to go in here");
                    break;

            }
            //regardless of which it was, destroy after use (giving it some time to play sound)
            Destroy(gameObject, 1.0f);
        }
    }

    // Use this for initialization
    void Start () {
        aSource = GetComponent<AudioSource>();
        pickupActivated = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
