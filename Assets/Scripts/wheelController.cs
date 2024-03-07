using UnityEngine;

public class wheelController : MonoBehaviour
{
    
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    public float acceleration = 500f;
    public float maxSpeed = 20f;  // Maximum speed of the car
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;

    
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
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

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



        // apply Apply torque to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        // apply breaking force to all wheels.
        frontRight.brakeTorque= currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;

        // take care of the steering.
        float currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        // Update wheel meshes
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);

        // Control the animation based on the turn angle and wheel rotation
        if (steeringWheelAnimation != null)
        {
            float normalizedTurnAngle = currentTurnAngle / maxTurnAngle;
            float maxRotation = Mathf.Max(frontLeftTransform.localEulerAngles.y, frontRightTransform.localEulerAngles.y);
            float normalizedRotation = maxRotation / maxTurnAngle;
            float normalizedAnimationTime = Mathf.Max(Mathf.Abs(normalizedTurnAngle), normalizedRotation);
            steeringWheelAnimation["SteeringWheelAnimation"].normalizedTime = Mathf.Clamp01(normalizedAnimationTime);
            steeringWheelAnimation.Play("SteeringWheelAnimation");
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
}
