using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosionScript : MonoBehaviour {

    public float explosionRadius = 7f;
    public float missileSpeed = 5f;
    public bool exploded = false;

    GameObject cameraGO; //uses it to look at where it should point towards
    public GameObject originGO;
    public GameObject explosionGO;//saves an explosion GameObject here

    Vector3 startPosition;


    public void Explode()
    {
        //exploded, so...
        exploded = true;

        //turns off GO's Mesh 
        //TODO

        //Creates a fireball around it
        GameObject newExplosion = Instantiate(explosionGO, transform.position, Quaternion.identity);


        //Gets list of ALL enemies, so it can see which ones it will damage
        GameObject[] listOfEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        float baseDamage = PlayerWeaponsController.instance.missileBaseDamage;
        float damageRange = PlayerWeaponsController.instance.missileDamageRange;

        foreach(GameObject enemy in listOfEnemies)
        {
            //if it's close, damage it
            if(Vector3.Distance(transform.position, enemy.transform.position)<=explosionRadius)
            {
                //damage varies a bit
                enemy.GetComponent<EnemyHealth>().DamageEnemy(Random.Range(baseDamage - damageRange, baseDamage + damageRange));
            }
        }

        //at the end, destroy itself and the fireball around it (with a delay)
        Destroy(newExplosion, 10f);
        Destroy(this.gameObject);

    }


    public void OnTriggerEnter(Collider other)
    {
        //only goes off if it gets near an enemy, or if it hits an obstacle like the ground or a building
        if ((other.tag == "Enemy" || other.tag == "Ground" || other.tag == "House")&&!exploded)
            Explode();
    }


    void PointTowardsAimDirection()
    {
        RaycastHit hit;
        Vector3 endPosition;//position where missile will go to
        //check where camera's sight intercepts with an object
        if (Physics.Raycast(cameraGO.transform.position, cameraGO.transform.forward * PlayerWeaponsController.instance.missileTravelDistance, out hit))
        {
            endPosition = hit.point;

            transform.LookAt(endPosition);
        }

    }

    IEnumerator StartRocketPropulsion()
    {
        //infinite propulsion forward
        while(true)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward*missileSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    private void Awake()
    {
        cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Use this for initialization
    void Start () {
        

        //once spawned, face direction camera is facing
        PointTowardsAimDirection();

        //save start point
        startPosition = originGO.transform.position;

        //move in that direction
        StartCoroutine(StartRocketPropulsion());
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Vector3.Distance(transform.position, startPosition)>=PlayerWeaponsController.instance.missileTravelDistance)
        {
            Explode();//explodes after getting too far from origin
        }
	}
}
