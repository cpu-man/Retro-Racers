using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public float speed = 10f;           // Movement speed of the car
    public float rotationSpeed = 100f;  // Rotation speed of the car
    public float maxSteerAngle = 30f;   // Maximum steering angle of the car

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input from arrow keys or A/D keys for movement and rotation
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        // Calculate movement and rotation based on input
        Vector3 movement = transform.forward * moveInput * speed * Time.fixedDeltaTime;
        Quaternion rotation = Quaternion.Euler(0f, steerInput * rotationSpeed * Time.fixedDeltaTime, 0f);

        // Apply movement and rotation to the car
        rb.MovePosition(rb.position + movement);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
