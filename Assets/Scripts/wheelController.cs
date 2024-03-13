using UnityEngine;

public class wheelController : MonoBehaviour
{
    [SerializeField] Transform SteeringWheelTrans;
    
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    [SerializeField] Light[] headlights; // Reference to headlights
    [SerializeField] Light[] taillights; // Reference to taillights

    public float acceleration = 500f;
    public float maxSpeed = 20f;  // Maximum speed of the car
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;
    public float brakingFriction = 2.0f; // Adjust this to control the friction during braking
    public float driftFriction = 0.8f; // Adjust this to control lateral friction during drifting
    public float driftControl = 0.2f; // Adjust this to control drifting responsiveness


    //private float currentAcceleration;
    //private float currentTurnAngle = 0f;
    
    private Rigidbody rigidBody;
    private float currentBreakForce = 0f;

    // Cache input values
    private float verticalInput;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += new Vector3(0, -1f, 0);

    }

    private void FixedUpdate()
    {
        // Cache input values
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);

        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed));

        // Calculate slope of the ground underneath the car
        float slopeAngle = GetSlopeAngle();
        // Get forward/reverse acceleration from the vertical axis (w and s keys).
        float currentAcceleration = acceleration * verticalInput * Mathf.Clamp01(1 - slopeAngle / maxSlopeAngle);

        // Apply braking force
        currentBreakForce = Input.GetKey(KeyCode.Space) ? brakingForce : 0f;

        // Gradually reduce speed when not accelerating or braking
        if (!Input.GetKey(KeyCode.Space) && Mathf.Approximately(currentAcceleration, 0f))
        {
            currentAcceleration -= Time.fixedDeltaTime * acceleration * brakingFriction;
        }


        /*
        // Apply braking friction
        frontRight.brakeTorque = currentBreakForce * (brakingFriction + speedFactor); // Adjust friction dynamically based on speed;
        frontLeft.brakeTorque = currentBreakForce * (brakingFriction + speedFactor);
        backLeft.brakeTorque = currentBreakForce * (brakingFriction + speedFactor);
        backRight.brakeTorque = currentBreakForce * (brakingFriction + speedFactor);
        */

        // apply Apply torque to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        // apply breaking force to all wheels.
        frontRight.brakeTorque= currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;


        // Modify steering
        float currentTurnAngle = maxTurnAngle * horizontalInput;
        if (forwardSpeed > maxSpeed * 0.75f)
        {
            currentTurnAngle *= 0.5f; // Reduce steering angle at high speeds
        }

        // Modify steering for drifting
        if (forwardSpeed > 0 && Input.GetKey(KeyCode.LeftShift)) // Assuming Left Shift initiates drifting
        {
            frontLeft.steerAngle = currentTurnAngle - (maxTurnAngle * driftControl);
            frontRight.steerAngle = currentTurnAngle + (maxTurnAngle * driftControl);
        }
        else
        {
            frontLeft.steerAngle = currentTurnAngle;
            frontRight.steerAngle = currentTurnAngle;
        }


        // Update wheel meshes
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);

        // Update steering wheel rotation
        UpdateSteeringWheelRotation();

        /*
        // gives input a negative and positive depending on if you press 'A' or 'D'
        float inputHori = Input.GetAxis("Horizontal");

        // multiplies by 1
        float multiplier = 1;
        if (inputHori > 0) multiplier = -1;

        SteeringWheelTrans.localEulerAngles = new Vector3(23.891f, 0, Mathf.Lerp(0, 90 * multiplier, Mathf.Abs(inputHori)));
        */

        // Control the lights
        if (Input.GetKeyDown(KeyCode.L)) // Example key to toggle lights
        {
            ToggleHeadlights();
            ToggleTaillights();
        }

    }

    // Method to get the slope angle of the ground underneath the car
    float GetSlopeAngle()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            return Vector3.Angle(hit.normal, Vector3.up);
        }
        return 0f;
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        // get wheel collider state.
        Vector3 postition;
        Quaternion rotation;
        col.GetWorldPose(out postition, out rotation);

        // get wheel transform state.
        trans.position = postition;
        trans.rotation = rotation;
    }

    void UpdateSteeringWheelRotation()
    {
        float inputHori = horizontalInput;
        float multiplier = inputHori > 0 ? -1 : 1;
        steeringWheelTrans.localEulerAngles = new Vector3(23.891f, 0, Mathf.Lerp(0, 90 * multiplier, Mathf.Abs(inputHori)));
    }

    // Method to toggle headlights
    void ToggleHeadlights()
    {
        foreach (Light headlight in headlights)
        {
            headlight.enabled = !headlight.enabled;
        }
    }

    // Method to toggle taillights
    void ToggleTaillights()
    {
        foreach (Light taillight in taillights)
        {
            taillight.enabled = !taillight.enabled;
        }
    }
    
}
