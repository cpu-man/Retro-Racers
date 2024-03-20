using UnityEngine;

public class DrunkMechanic : MonoBehaviour
{
    // Parameters for drunk effects
    public float maxDrunkIntensity = 1.0f;
    public float drunkIncreaseRate = 0.1f;
    public float maxInputDistortion = 1.0f;
    public float maxLatency = 0.2f; // Maximum latency added to input
    public float maxDelay = 0.1f; // Maximum delay added to input

    private float drunkIntensity = 0.0f;
    private WheelController carController;
    private Rigidbody playerRigidbody;

    private float inputLatency = 0.0f;
    private float inputDelay = 0.0f;

    void Start()
    {
        carController = GetComponent<WheelController>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Simulate drunk effects
        if (Input.GetKeyDown(KeyCode.F)) // Change to KeyCode.F to use the "F" key for drinking
        {
            Drink();
            Debug.Log("Player is drinking. Drunk intensity: " + drunkIntensity);
        }
    }

    void FixedUpdate()
    {
        // Apply drunk effects to car controls
        ApplyDrunkControls();
    }

    void Drink()
    {
        drunkIntensity = Mathf.Clamp(drunkIntensity + drunkIncreaseRate, 0.0f, maxDrunkIntensity);

        // Adjust input latency and delay based on drunk intensity
        inputLatency = Mathf.Lerp(0.0f, maxLatency, drunkIntensity / maxDrunkIntensity);
        inputDelay = Mathf.Lerp(0.0f, maxDelay, drunkIntensity / maxDrunkIntensity);
    }

    void ApplyDrunkControls()
    {
        // Apply input distortion based on drunk intensity
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Apply input latency
        float distortedHorizontalInput = Mathf.Clamp(horizontalInput, -1.0f, 1.0f);
        float distortedVerticalInput = Mathf.Clamp(verticalInput, -1.0f, 1.0f);

        // Apply input delay
        float delayedHorizontalInput = distortedHorizontalInput + Random.Range(-inputDelay, inputDelay);
        float delayedVerticalInput = distortedVerticalInput + Random.Range(-inputDelay, inputDelay);

        // Gradually apply drunk effects to car controls
        float gradualIntensity = Mathf.Clamp01(drunkIntensity / maxDrunkIntensity);

        // Apply distorted input to car controller with added latency
        carController.horizontalInput = Mathf.Lerp(distortedHorizontalInput, delayedHorizontalInput, gradualIntensity);
        carController.verticalInput = Mathf.Lerp(distortedVerticalInput, delayedVerticalInput, gradualIntensity);
    }
}