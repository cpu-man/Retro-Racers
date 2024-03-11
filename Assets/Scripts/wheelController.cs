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
    private float currentBreakForce = 0f;
    //private float currentTurnAngle = 0f;
    public float centreOfGravityOffset = -1f;
    

    private Rigidbody rigidBody;
    private Animation steeringWheelAnimation; // Reference to the steering wheel animation


    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling

        rigidBody.centerOfMass += new Vector3(0, 0, centreOfGravityOffset);

        // Get the Animation component from the GameObject that contains your animation
        steeringWheelAnimation = GetComponentInChildren<Animation>(); // Adjust this based on your hierarchy

    }

    private void FixedUpdate()
    {

        // Get forward/reverse acceleration from the vertical axis (w and s keys).
        float currentAcceleration = acceleration * Input.GetAxis("Vertical");

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);

        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed));

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(0, acceleration, speedFactor);


        
        // If we're pressing space, give currentBrekingForce a value.
        if (Input.GetKey(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0f;

        // Apply braking friction
        frontRight.brakeTorque = currentBreakForce * brakingFriction;
        frontLeft.brakeTorque = currentBreakForce * brakingFriction;
        backLeft.brakeTorque = currentBreakForce * brakingFriction;
        backRight.brakeTorque = currentBreakForce * brakingFriction;

        // apply Apply torque to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        // apply breaking force to all wheels.
        frontRight.brakeTorque= currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;

        float currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");

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
        /*
        // take care of the steering.
        float currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
        */

        // Update wheel meshes
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);

        // Control the animation based on the turn angle and wheel rotation
        if (steeringWheelAnimation != null)
        {
            /*
            float normalizedTurnAngle = currentTurnAngle / maxTurnAngle;
            float maxRotation = Mathf.Max(frontLeftTransform.localEulerAngles.y, frontRightTransform.localEulerAngles.y);
            float normalizedRotation = maxRotation / maxTurnAngle;
            float normalizedAnimationTime = Mathf.Max(Mathf.Abs(normalizedTurnAngle), normalizedRotation);
            steeringWheelAnimation["SteeringWheelAnimation"].normalizedTime = Mathf.Clamp01(normalizedAnimationTime);
            steeringWheelAnimation.Play("SteeringWheelAnimation");*/
            

        }

        // gives input a negative and positive depending on if you press 'A' or 'D'
        float inputHori = Input.GetAxis("Horizontal");

        // multiplies by 1
        float multiplier = 1;
        if (inputHori > 0) multiplier = -1;

        SteeringWheelTrans.localEulerAngles = new Vector3(23.891f, 0, Mathf.Lerp(0, 90 * multiplier, Mathf.Abs(inputHori)));

        // Control the lights
        if (Input.GetKeyDown(KeyCode.L)) // Example key to toggle lights
        {
            ToggleHeadlights();
            ToggleTaillights();
        }

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
