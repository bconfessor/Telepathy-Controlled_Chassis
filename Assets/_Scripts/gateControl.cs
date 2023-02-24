using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateControl : MonoBehaviour {

    /// <summary>
    /// Time it takes for the gate to open completely
    /// </summary>
    public float timeToSpin = 5.0f;

    public void  OpenGate()
    {
        StartCoroutine(OpenGateCoRoutine());
    }

    IEnumerator OpenGateCoRoutine()
    {
        float frameUpdate = 0.1f; //time WaitForSeconds waits
        float degreesPerSecond = (90.0f*frameUpdate) / timeToSpin;
        float counter = 0.0f; //controls while
        while (counter <= timeToSpin)
        {
            counter += frameUpdate;
            this.transform.Rotate(new Vector3(0, 0, -degreesPerSecond));

            yield return new WaitForSeconds(frameUpdate);

        }
    }


	// Use this for initialization
	void Start () {
        //StartCoroutine(OpenGate());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
