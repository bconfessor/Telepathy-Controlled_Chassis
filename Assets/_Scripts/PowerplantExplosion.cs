using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerplantExplosion : MonoBehaviour {


    public GameObject explosionParentGO;//Gets the GO that's the parent of all the explosion particles GOs

    public void StartExplosionAnimation()
    {
        StartCoroutine(ExplosionCoroutine());
    }


    IEnumerator ExplosionCoroutine()
    {
        float timeToWaitForNext = 1.0f ;
        foreach(Transform explosionGO in explosionParentGO.transform)
        {
            explosionGO.gameObject.SetActive(true);
            //time it takes to get to new one depends on current one's tag
            //TODO: If it's explosion, get corresponding sound to play
            switch(explosionGO.tag)
            {
                case "Explosion":
                    timeToWaitForNext = 0.3f;
                    break;
                case "ExplosionBig":
                    timeToWaitForNext = 0.5f;
                    break;
                case "ExplosionSmall":
                    timeToWaitForNext = 0.2f;
                    break;
                case "Fire":
                    timeToWaitForNext = 0.1f;
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(timeToWaitForNext);
        }
    }

	// Use this for initialization
	void Start () {
        //StartExplosionAnimation();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
