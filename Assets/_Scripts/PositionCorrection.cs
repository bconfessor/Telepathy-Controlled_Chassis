using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCorrection : MonoBehaviour {

    public GameObject PlayerGO;
    public float initialYPosition;

    public GameObject mainCam;//main camera Game Object
	// Use this for initialization
	void Start () {
        //Y remains constant, get initial y
        initialYPosition = transform.position.y;
	}

    // Update is called once per frame
    void Update() {
        //fix position of object
        transform.position = new Vector3(PlayerGO.transform.position.x, initialYPosition, PlayerGO.transform.position.z);
        if (transform.rotation.eulerAngles.y != mainCam.transform.rotation.eulerAngles.y)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, mainCam.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
