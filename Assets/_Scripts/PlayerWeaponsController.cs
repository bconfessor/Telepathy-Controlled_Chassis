using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponsController : MonoBehaviour {

    public static PlayerWeaponsController instance=null;

    public GameObject cameraGO;
    public AudioSource playerAudioSource;
    public GameObject missileWeaponPrefab;

    public AudioClip bulletShotSound, bulletEmptyClipSound, bulletFallingSound;
    public AudioClip missileShotSound, missileEmptyClipSound;

    public GameObject[] muzzleFlashesGOs;

    public float bulletTravelDistance = 80.0f;
    public float missileTravelDistance = 40.0f;

    public GameObject[] missileSpawnPositions;//2-4 possible positions for missile spawning


    public int currentBulletAmmo;//amount of bullets currently in player's possession
    public int maxAmountBulletAmmo = 60;
    public float bulletBaseDamage = 15.0f;
    public float bulletDamageRange = 5.0f;//amount bullet damage varies
    public bool isFiringBullet = false;//locks the player from firing too many in sucession
    public float bulletFiringRate = 0.3f;//time it takes to fire one bullet


    public int currentMissileAmmo;
    public int maxAmountMissileAmmo = 5;
    public float missileBaseDamage = 100.0f;
    public float missileDamageRange = 20f;
    public bool isFiringMissile = false;//locks the player from firing too many in sucession
    public float missileFiringRate = 3.0f;//time it takes to fire one bullet
    

    public int currentBlastBarrageAmmo; //TODO: Implement special attack?
    public int maxAmountBlastBarrageAmmo = 2;


    public void RestoreBulletAmmo(int bulletAmountToAdd)
    {
        if (currentBulletAmmo+bulletAmountToAdd >= maxAmountBulletAmmo)//can't surpass threshold
                    currentBulletAmmo = maxAmountBulletAmmo; 
        else
            currentBulletAmmo += bulletAmountToAdd;
        
    }

    public void RestoreMissileAmmo(int missileAmountToAdd)
    {
        if (currentMissileAmmo+missileAmountToAdd >= maxAmountMissileAmmo)//can't surpass threshold
                    currentMissileAmmo = maxAmountMissileAmmo;
        else
            currentMissileAmmo += missileAmountToAdd;
        
    }

    IEnumerator WaitForNewBulletReady()
    {
        yield return new WaitForSeconds(bulletFiringRate);
        isFiringBullet = false;
    }

    public void StopShootingBullets()
    {
        //turn off the sounds and muzzle flashes

        foreach(GameObject g in muzzleFlashesGOs)
        {
            g.SetActive(false);

        }
        playerAudioSource.Stop();
        //play 'bullets falling down' clip
        playerAudioSource.PlayOneShot(bulletFallingSound);
    }

    //shoots raycasted bullets; 
    public void ShootBullet()
    {
        //if there's no ammo, don't shoot
        if(currentBulletAmmo<=0)
        {
            Debug.Log("No bullets");
            //As a precaution, in case there are still any lit
            foreach (GameObject g in muzzleFlashesGOs)
                g.SetActive(false);
                
            return;
        }
        //else, there are bullets, shoot

        isFiringBullet = true;

        //activate muzzle flashes
        foreach(GameObject g in muzzleFlashesGOs)
        {
            g.SetActive(true);
        }

        //stop current audio so it doesn't get too polluted
        playerAudioSource.Stop();
        //play shot audio
        playerAudioSource.PlayOneShot(bulletShotSound);
        Debug.Log("Shot bullet");
        //remove one bullet
        currentBulletAmmo -= 1;
        //Updates UI
        UIManager.instance.AddCurrentAmmoUIValue(-1);

        RaycastHit hit;
        //make player wait to be able to fire another shot, regardless of hitting something
        StartCoroutine(WaitForNewBulletReady());
        //For now, draw shot ray
        Debug.DrawRay(cameraGO.transform.position, cameraGO.transform.forward*bulletTravelDistance, Color.red, 50.0f);

        if(Physics.Raycast(cameraGO.transform.position, cameraGO.transform.forward*bulletTravelDistance, out hit))
        {
            Debug.Log("hit Something");
            if(hit.transform.tag=="Enemy")
            {
                //Hit an enemy
                Debug.Log("Hit a " + hit.transform.name);
                hit.transform.GetComponent<EnemyHealth>().DamageEnemy(Random.Range(bulletBaseDamage-bulletDamageRange, bulletBaseDamage+bulletDamageRange));
                
            }
        }
        
    }

    //makes player wait a little until they can fire another missile
    IEnumerator WaitForNewMissileReady()
    {
        yield return new WaitForSeconds(missileFiringRate);
        isFiringMissile = false;
    }

    public void ShootMissile()
    {

        //first, check if there are any missiles
        if(currentMissileAmmo<=0)
        {
            Debug.Log("No missiles");
            return;
        }


        isFiringMissile = true;
        currentMissileAmmo--;
        playerAudioSource.PlayOneShot(missileShotSound);

        //Update UI
        UIManager.instance.AddCurrentMissileUIValue(-1);

        //instantiates missile prefab(for now, only in one position
        GameObject newMissile = Instantiate(missileWeaponPrefab, missileSpawnPositions[0].transform.position, Quaternion.identity);

        //After that, give origin to new missile so it knows where it started
        newMissile.GetComponent<MissileExplosionScript>().originGO = missileSpawnPositions[0];

        //Run missile wait period
        StartCoroutine(WaitForNewMissileReady());
    }




    //wait a while, then get main camera
    IEnumerator GetLateCam()
    {
        yield return new WaitForSeconds(1.5f);
        cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
    }




    // Use this for initialization
    void Start () {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
            instance = this;

        StartCoroutine(GetLateCam());
        
        //load weapons with half the max amount of ammo
        currentBulletAmmo = maxAmountBulletAmmo / 2;
        currentMissileAmmo = maxAmountMissileAmmo / 2;

        //Load UI
        UIManager.instance.ChangeCurrentAmmoUIValue(currentBulletAmmo);
        UIManager.instance.ChangeMaxAmmoUIValue(maxAmountBulletAmmo);
        UIManager.instance.ChangeCurrentMissileUIValue(currentMissileAmmo);
        UIManager.instance.ChangeMaxMissileUIValue(maxAmountMissileAmmo);
        
        //load this with only 1, but for now won't be implemented
        currentBlastBarrageAmmo = 1;


        //load components
        cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
        playerAudioSource = gameObject.GetComponent<AudioSource>();
	}
	


	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetMouseButton(0) && !isFiringBullet && PlayerHealth.instance.isAlive)
        {
            ShootBullet();
        }
        else if (Input.GetMouseButtonUp(0) && PlayerHealth.instance.isAlive && currentBulletAmmo>0)
        {
            StopShootingBullets();
        }

        if(Input.GetMouseButtonDown(1) && !isFiringMissile && PlayerHealth.instance.isAlive)
        {
            ShootMissile();

        }

	}
}
