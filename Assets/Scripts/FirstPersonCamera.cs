using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform car; // Reference to the car GameObject

    private void Update()
    {
        // Set the camera position and rotation relative to the car
        transform.position = car.position + car.forward * 0.3f + car.up * 1.4f; // Adjust position as needed
        transform.rotation = car.rotation;
    }
}