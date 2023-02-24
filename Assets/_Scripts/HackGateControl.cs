using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackGateControl : MonoBehaviour {

    public GameObject leftGateHingeGO, rightGateHingeGO;

    public float timeToSpin = 5.0f;
    public float maxAngleToSpin = 60.0f; //how many degrees object will turn

    public void OpenGate()
    {
        StartCoroutine(SpinLeftGateHinge());
        StartCoroutine(SpinRightGateHinge());
    }


    IEnumerator SpinLeftGateHinge()
    {
        //spin it clockwise(positive)
        float frameUpdate = 0.1f; //time WaitForSeconds waits
        float degreesPerSecond = (maxAngleToSpin * frameUpdate) / timeToSpin;
        float counter = 0.0f; //controls while
        while (counter <= timeToSpin)
        {
            counter += frameUpdate;
            leftGateHingeGO.transform.Rotate(new Vector3(0, degreesPerSecond, 0));

            yield return new WaitForSeconds(frameUpdate);

        }
    }

    IEnumerator SpinRightGateHinge()
    {
        //spin it counterclockwise(negative)
        float frameUpdate = 0.1f; //time WaitForSeconds waits
        float degreesPerSecond = (maxAngleToSpin * frameUpdate) / timeToSpin;
        float counter = 0.0f; //controls while
        while (counter <= timeToSpin)
        {
            counter += frameUpdate;
            rightGateHingeGO.transform.Rotate(new Vector3(0, -degreesPerSecond, 0));

            yield return new WaitForSeconds(frameUpdate);

        }
    }

	// Use this for initialization
	void Start () {
        //OpenGate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
