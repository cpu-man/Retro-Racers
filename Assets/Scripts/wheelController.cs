using UnityEngine;


public class wheelController : MonoBehaviour
{
    public Engine engine; // Reference to the Engine script

    [SerializeField] Transform steeringWheelTrans;

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
    public float driftControl = 0.2f; // Adjust this to control drifting responsiveness
    public float handbrakeForce = 1000f; // Adjust this to control the handbrake force


    private Rigidbody rigidBody;
    private float currentBreakForce = 0f;
    private float horizontalInput;
    private float verticalInput;
    private bool isHandbrakeActivated = false; // Flag to track handbrake activation
    private const float maxSlopeAngle = 45f; // Max slope angle for the car

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

        // Check if handbrake is activated
        if (Input.GetKey(KeyCode.Space))
        {
            isHandbrakeActivated = true;
        }
        else
        {
            isHandbrakeActivated = false;
        }

        // Calculate current speed in km/h
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity) * 3.6f;

        // Calculate slope of the ground underneath the car
        float slopeAngle = GetSlopeAngle();
        // Get forward/reverse acceleration from the vertical axis (w and s keys).
        float currentAcceleration = acceleration * verticalInput * Mathf.Clamp01(1 - slopeAngle / maxSlopeAngle);

        // Apply braking force
        currentBreakForce = Input.GetKey(KeyCode.Space) ? breakingForce : 0f;

        // Gradually reduce speed when not accelerating or braking
        if (!isHandbrakeActivated && !Input.GetKey(KeyCode.Space) && Mathf.Approximately(currentAcceleration, 0f))
        {
            // Check if the car is on a slope
            if (Mathf.Abs(slopeAngle) < maxSlopeAngle) // Adjust maxSlopeAngle as needed
            {
                // If on flat ground, prevent backward movement
                currentAcceleration = Mathf.Clamp(forwardSpeed, 0f, currentAcceleration);
            }
            else
            {
                // If on a slope, allow backward movement
                currentAcceleration -= Time.fixedDeltaTime * acceleration * brakingFriction;
            }
        }

        // Convert maxSpeed from km/h to m/s
        float maxSpeedMS = maxSpeed / 3.6f;

        // Set throttle input for the engine
        engine.SetThrottle(verticalInput);

        // Apply torque from the engine
        float engineTorque = engine.GetCurrentTorque();
        //float engineTorque = engine.ApplyThrottle(verticalInput);
        frontRight.motorTorque = engineTorque;
        frontLeft.motorTorque = engineTorque;

        // Limit the maximum speed in km/h
        if (forwardSpeed > maxSpeed)
        {
            // Calculate the velocity required to reach maxSpeed
            Vector3 targetVelocity = transform.forward * (maxSpeed / 3.6f);
            // Apply a braking force to gradually reduce the speed
            rigidBody.linearVelocity = Vector3.Lerp(rigidBody.linearVelocity, targetVelocity, Time.deltaTime * brakingFriction);
        }

        // Modify steering
        float currentTurnAngle = maxTurnAngle * horizontalInput;
        float turnMultiplier = 1f;

        
        /*
        if (forwardSpeed > maxSpeed * 0.75f)
        {
            turnMultiplier = 0.5f; // Reduce steering angle at high speeds
        }
        */

        // Modify steering for drifting
        if (forwardSpeed > 0 && Input.GetKey(KeyCode.LeftShift)) // Assuming Left Shift initiates drifting
        {
            frontLeft.steerAngle = currentTurnAngle - (maxTurnAngle * driftControl * turnMultiplier);
            frontRight.steerAngle = currentTurnAngle + (maxTurnAngle * driftControl * turnMultiplier);
        }
        else
        {
            frontLeft.steerAngle = currentTurnAngle * turnMultiplier;
            frontRight.steerAngle = currentTurnAngle * turnMultiplier;
        }

        // Apply torque to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        // Apply braking force to all wheels.
        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;

        // Update wheel meshes
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);

        // Update steering wheel rotation
        UpdateSteeringWheelRotation();

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
        // Get wheel collider state.
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        // Get wheel transform state.
        trans.position = position;
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
