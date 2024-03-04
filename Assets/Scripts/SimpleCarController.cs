using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;           // Movement speed of the car
    public float rotationSpeed = 100f;  // Rotation speed of the car
    public float maxSteerAngle = 30f;   // Maximum steering angle of the car
    public float torque = 2000f;        // Torque applied for steering

    private Rigidbody rb;
    private float currentSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input from arrow keys or A/D keys for movement and rotation
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        // Calculate acceleration
        currentSpeed += moveInput * acceleration * Time.fixedDeltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Apply forward force for acceleration
        rb.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        // Apply steering torque
        float turnTorque = steerInput * torque * Time.fixedDeltaTime;
        rb.AddTorque(transform.up * turnTorque);

        // Limit the maximum turn torque
        float currentAngularVelocity = rb.angularVelocity.y;
        if (Mathf.Abs(currentAngularVelocity) > maxSteerAngle)
        {
            float sign = Mathf.Sign(currentAngularVelocity);
            rb.angularVelocity = new Vector3(0f, maxSteerAngle * sign, 0f);
        }
        /*
        // Calculate movement and rotation based on input
         Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
         Quaternion rotation = Quaternion.Euler(0f, steerInput * rotationSpeed * Time.fixedDeltaTime, 0f);

         // Apply movement and rotation to the car
         rb.MovePosition(rb.position + movement);
         if (movement.magnitude > 0.1f)
         rb.MoveRotation(rb.rotation * rotation);*/
    }
}
