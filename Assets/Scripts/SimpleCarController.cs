using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public float acceleration = 10f;    // Acceleration rate of the car
    public float deceleration = 5f;     // Deceleration rate of the car
    public float maxSpeed = 20f;        // Maximum speed of the car
    public float rotationSpeed = 100f;  // Rotation speed of the car
    public float maxSteerAngle = 30f;   // Maximum steering angle of the car

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input from arrow keys or W/S keys for acceleration/deceleration and A/D keys for rotation
        float accelerationInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        // Calculate movement and rotation based on input
        float currentSpeed = rb.velocity.magnitude;
        if (accelerationInput > 0)
        {
            // Accelerate
            rb.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        else if (accelerationInput < 0)
        {
            // Decelerate
            rb.AddForce(-transform.forward * deceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        // Clamp speed to the maximum speed
        if (currentSpeed > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Calculate rotation based on input
        Quaternion rotation = Quaternion.Euler(0f, steerInput * rotationSpeed * Time.fixedDeltaTime, 0f);

        // Apply rotation to the car
        rb.MoveRotation(rb.rotation * rotation);
    }
}
