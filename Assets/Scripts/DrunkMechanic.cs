using System.Collections;
using UnityEngine;

public class DrunkMechanic : MonoBehaviour
{
    // Parameters for drunk effects
    public Material objectRenderer; // Reference to the object's renderer
    public float maxDrunkIntensity = 1.0f;
    public float drunkIncreaseRate = 0.1f;
    public float drunkThreshold = 0.5f; // Drunk intensity threshold to start showing effects
    public float maxInputDistortion = 1.0f;
    public float maxLatency = 0.2f; // Maximum latency added to input
    public float maxDelay = 0.1f; // Maximum delay added to input
    public Animator bottleAnimator;
    public AudioSource bottleAudioSource; // Reference to the AudioSource component

    public float drunkIntensity = 0.0f;
    private bool effectsActivated = false;


    private wheelController carController;
    private Rigidbody playerRigidbody;

    private float inputLatency = 0.0f;
    private float inputDelay = 0.0f;

    void Start()
    {
        carController = GetComponent<wheelController>();
        playerRigidbody = GetComponent<Rigidbody>();
        
        // Cache input values and apply controls
        CacheAndApplyControls();
        
        // Ensure the objectRenderer reference is assigned
        if (objectRenderer == null)
        {
            Debug.LogError("Object renderer reference is not assigned.");
            return;
        }

        // Set initial drunk intensity to zero
        objectRenderer.SetFloat("_DrunkIntensity", 0.0f);
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
    
    void CacheAndApplyControls()
    {
        // Cache input values
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // Apply controls with a delay
        StartCoroutine(ApplyControls(verticalInput, horizontalInput));
    }

    IEnumerator ApplyControls(float verticalInput, float horizontalInput)
    {
        // Delay before applying controls
        yield return new WaitForSeconds(3);

        // Apply controls
        carController.verticalInput = verticalInput;
        carController.horizontalInput = horizontalInput;
    }

    void Drink()
    {
        drunkIntensity = Mathf.Clamp(drunkIntensity + drunkIncreaseRate, 0.0f, maxDrunkIntensity);
        
        // Set drunk intensity to the shader
        objectRenderer.SetFloat("_DrunkIntensity", drunkIntensity);
        
        // Activate effects if the threshold is reached
        if (drunkIntensity >= drunkThreshold)
        {
            effectsActivated = true;
        }

        // Adjust input latency and delay based on drunk intensity
        inputLatency = Mathf.Lerp(0.0f, maxLatency, drunkIntensity / maxDrunkIntensity);
        inputDelay = Mathf.Lerp(0.0f, maxDelay, drunkIntensity / maxDrunkIntensity);
        
        // Trigger the drinking animation
        
        if (bottleAnimator != null)
        {
            bottleAnimator.SetTrigger("Drink");
            
            // Play the bottle sound
            if (bottleAudioSource != null && bottleAudioSource.clip != null)
            {
                bottleAudioSource.Play();
            }
        }
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
