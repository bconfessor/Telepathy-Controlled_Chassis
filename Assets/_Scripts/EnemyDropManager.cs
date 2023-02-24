using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropManager : MonoBehaviour {

    public GameObject bulletAmmoPrefab, missileAmmoPrefab, healthPickupPrefab;

    public float bulletDropPercentage = 0.2f;//chance enemy has of dropping bullet ammo upon death
    public float missileDropPercentage = 0.1f;//chance enemy has of dropping missile ammo upon death
    public float healthDropPercentage = 0.15f;

    public int bulletAmmoDropped = 10;//amount of ammo this enemy drops
    public int missileAmmoDropped = 1;//amount of missiles this enemy drops
    public int healthAmountDropped = 10;//amount of health this enemy drops
    


    public void CheckForAmmoDrops()
    {
        Vector3 spawnPos = new Vector3(gameObject.transform.position.x, 3f, gameObject.transform.position.z);
        //check for bullet drop first
        if(Random.Range(0.0f, 1.0f)<bulletDropPercentage)
        {
            //dropped bullet ammo

            //Instantiate bullet ammo prefab
            GameObject newBulletPickup = Instantiate(bulletAmmoPrefab);
            newBulletPickup.transform.position = spawnPos;
            //Put ammo value in prefab created
            newBulletPickup.GetComponent<PickupCollectionScript>().pickupValue = bulletAmmoDropped;

        }
        else if (Random.Range(0.0f, 1.0f)<missileDropPercentage)
        {
            //dropped missile ammo
            
            //Instantiate missile ammo prefab
            GameObject newMissilePickup = Instantiate(missileAmmoPrefab);
            newMissilePickup.transform.position = spawnPos;

            //Put ammo value in prefab instantiated
            newMissilePickup.GetComponent<PickupCollectionScript>().pickupValue = missileAmmoDropped;
        }
    }

    //independent from bullet drops
    public void CheckForHealthDrop()
    {
        Vector3 spawnPos = new Vector3(gameObject.transform.position.x, 3f, gameObject.transform.position.z);

        if (Random.Range(0.0f, 1.0f)<healthDropPercentage)
        {
            //dropped health pickup
            GameObject newHealthPickup = Instantiate(healthPickupPrefab);
            newHealthPickup.transform.position = spawnPos+new Vector3(0,0,1);//so it doesn't spawn on top of another

            //Put ammo val in prefab instantiated
            newHealthPickup.GetComponent<PickupCollectionScript>().pickupValue = healthAmountDropped;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
