using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHoverScript : MonoBehaviour {

    public float initialYPosition = 0.6f;
    public float maxYRange = 0.3f;
    public float selfSpinSpeed = 5f;

    IEnumerator SpinAroundItself()
    {
        while(true)
        {
            transform.Rotate(Vector3.up, selfSpinSpeed, Space.Self);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(transform.position.x, initialYPosition, transform.position.z);
        StartCoroutine(SpinAroundItself());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
