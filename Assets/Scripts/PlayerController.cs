using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigibody3D;

    [SerializeField]
    ConfigurableJoint mainJoint;

    [SerializeField]
    Animator animator;

    //input
    Vector2 moveInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;

    //Controller settings
    float maxSpeed = 3;

    //States
    bool isGrounded = false;

    //Raycasts
    RaycastHit[] raycastHits = new RaycastHit[10];

    //Syncing of physics objects
    SyncPhysicsObject[] SyncPhysicsObjects;

    void Awake() 
    {
        SyncPhysicsObjects = GetComponentsInChildren<SyncPhysicsObject>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            isJumpButtonPressed = true;
    }

    void FixedUpdate()
    {
        //Assume that we are not grounded
        isGrounded = false;

        //check if we are grounded
        int numberofHits = Physics.SphereCastNonAlloc(rigibody3D.position, 0.1f, transform.up * -1, raycastHits, 0.5f);

        //check for valid results
        for (int i = 0; i < numberofHits; i++)
        {
            //Ignore self hits
            if (raycastHits[i].transform.root == transform)
                continue;

            isGrounded = true;
            break;
        }

        //Apply extra gravity to character to make is less floaty
        if (!isGrounded)
            rigibody3D.AddForce(Vector3.down * 10);
            //rigibody3D.velocity = new Vector3(moveInputVector.x * maxSpeed, rigibody3D.velocity.y, moveInputVector.y * maxSpeed);


        float inputMagnitued = moveInputVector.magnitude;

        Vector3 localVelocifyVsForward = transform.forward * Vector3.Dot(transform.forward, rigibody3D.velocity);

        float localForwardVelocity = localVelocifyVsForward.magnitude;

        if (inputMagnitued != 0)
        {
            Quaternion desiredDirection = Quaternion.LookRotation(new Vector3(moveInputVector.x, 0, moveInputVector.y * -1), transform.up);

            //Rotate target towards direction
            mainJoint.targetRotation = Quaternion.RotateTowards(mainJoint.targetRotation, desiredDirection, Time.fixedDeltaTime * 300);

            // 保持角色不在X、Z軸上傾斜
            rigibody3D.rotation = Quaternion.Euler(0, rigibody3D.rotation.eulerAngles.y, 0);

            // Vector3 localVelocifyVsForward = transform.forward * Vector3.Dot(transform.forward, rigibody3D.velocity);

            // float localForwardVelocity = localVelocifyVsForward.magnitude;

            if (localForwardVelocity < maxSpeed)
            {
                //move the character in the direction it is facing
                rigibody3D.AddForce(transform.forward * inputMagnitued * 30);
            }
        }

        if (isGrounded && isJumpButtonPressed)
        {
            rigibody3D.AddForce(Vector3.up * 20, ForceMode.Impulse);

            isJumpButtonPressed = false;
        }
        
        animator.SetFloat("movementSpeed", localForwardVelocity * 0.4f);

        //Update the jointts rotation base on the animations
        for(int i = 0; i< SyncPhysicsObjects.Length; i++)
        {
            SyncPhysicsObjects[i].UpdateJointFromAnimation();
        }
    }
}
