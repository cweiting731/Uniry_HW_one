using UnityEngine;

public class Player : MonoBehaviour {
    public Rigidbody rigibody3D;
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;
    public Transform followTarget; // Assign the FollowTarget GameObject in the Inspector

    public Animator animator;
    SyncPhysicsObject[] SyncPhysicsObjects;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        if (followTarget == null) {
            Debug.LogError("FollowTarget is not assigned. Please assign it in the Inspector.");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        #region Player Based Rotation

        // Rotate the Follow Target transform based on the input
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        followTarget.rotation *= Quaternion.AngleAxis(mouseX, Vector3.up);

        #endregion

        #region Vertical Rotation

        float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        followTarget.rotation *= Quaternion.AngleAxis(mouseY, Vector3.right);

        // Clamp the Up/Down rotation
        var angles = followTarget.localEulerAngles;
        angles.z = 0; // Prevent roll
        var angle = followTarget.localEulerAngles.x;

        if (angle > 180 && angle < 330) {
            angles.x = 330;
        } else if (angle < 180 && angle > 50) {
            angles.x = 50;
        }

        followTarget.localEulerAngles = angles;

        #endregion

        #region Player Movement

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = (followTarget.forward * moveVertical + followTarget.right * moveHorizontal).normalized * moveSpeed *
                           Time.deltaTime;
        Vector3 newPosition = rb.position + movement;
        rb.MovePosition(newPosition);

        #endregion

        transform.rotation = Quaternion.Euler(0, followTarget.rotation.eulerAngles.y, 0);
        followTarget.localEulerAngles = new Vector3(angles.x, 0, 0);
        ////////////////////////////////////////////////////////////
        
    }

    void FixedUpdate()
    {
        Vector3 localVelocifyVsForward = transform.forward * Vector3.Dot(transform.forward, rigibody3D.linearVelocity);
        float localForwardVelocity = localVelocifyVsForward.magnitude;

        animator.SetFloat("movementSpeed", localForwardVelocity * 0.4f);

        //Update the jointts rotation base on the animations
        for (int i = 0; i < SyncPhysicsObjects.Length; i++)
        {
            SyncPhysicsObjects[i].UpdateJointFromAnimation();
        }
    }
}