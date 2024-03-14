using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform car; // Reference to the car GameObject
    public Rigidbody carRigidbody; // Reference to the car's Rigidbody component
    public float minFOV = 60f; // Minimum FOV
    public float maxFOV = 90f; // Maximum FOV
    public float minSpeed = 0f; // Minimum speed for minimum FOV
    public float maxSpeed = 50f; // Maximum speed for maximum FOV

    private void Update()
    {
        // Set the camera position and rotation relative to the car
        transform.position = car.position + car.forward * 0.3f + car.up * 1.4f; // Adjust position as needed
        transform.rotation = car.rotation;

        // Calculate the speed ratio (0 to 1) based on the car's speed within the minSpeed to maxSpeed range
        float speedRatio = Mathf.Clamp01((carRigidbody.linearVelocity.magnitude - minSpeed) / (maxSpeed - minSpeed));

        // Interpolate FOV between minFOV and maxFOV based on the speed ratio
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, speedRatio);

        // Apply the target FOV to the camera
        Camera.main.fieldOfView = targetFOV;
    }
}
