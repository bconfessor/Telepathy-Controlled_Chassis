using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    public Animator anim;

    public float playerAnimatorSpeed = 2.0f;

    public float instantBackwardSpeed = 80f;
    public float maxBackwardWalkSpeed = 350f;
    public float rotationSpeed = 30f;//in degreees (I suppose?)
    public float previousY; //saves previous Y Euler rotation; used to start rotation animation

    public float errorMargin = 0.05f; //Error margin for the controller input(accounting for loose sticks)
    public GameObject mainCam;
    
    public float maxHeight = 0.35f;
    public float moveVert, moveHoriz;

    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        anim.speed = playerAnimatorSpeed;
        //yRotation = transform.rotation.y;
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        //position correction
        if (transform.position.y > maxHeight)
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);


        if (PlayerHealth.instance.isAlive)
        {
            //player always rotates in Y axis along with camera
            if (transform.rotation.eulerAngles.y != mainCam.transform.rotation.eulerAngles.y)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, mainCam.transform.eulerAngles.y, transform.eulerAngles.z);

            moveVert = Input.GetAxis("Vertical");
            anim.SetFloat("moveForward-Backward", moveVert);

          
            
            
            //Get horizontal movement, if there is any, make it rotate at a certain speed
            moveHoriz = Input.GetAxis("Horizontal");
            
            //if (Mathf.Abs(moveHoriz) >= errorMargin)
            //if current Y is different than previous
            if(previousY != transform.eulerAngles.y)
            {
                previousY = transform.eulerAngles.y;
                //is also moving horizontally, rotate it (with animation if standing idle)
                float direction;
                if (moveHoriz < 0)
                    direction = -1f;
                else
                    direction = 1f;
                float newY = transform.rotation.y + (direction * rotationSpeed);
                //Quaternion temp = new Quaternion(transform.rotation.x, newY, transform.rotation.z, transform.rotation.w);
                //transform.rotation = temp;
                //transform.Rotate(0, direction * rotationSpeed * Time.deltaTime, 0, Space.World);
                anim.SetBool("IsRotating", true);

            }
            else//is not rotating
                anim.SetBool("IsRotating", false);
                
        }
        
    }
}
