using UnityEngine;

public class SmokeEffect : MonoBehaviour
{
    public Rigidbody carRigidbody; // Reference to the car's Rigidbody
    public float emissionRateMultiplier = 10f; // Adjust this multiplier for desired emission rate sensitivity

    private ParticleSystem smokeParticleSystem; // Reference to the ParticleSystem

    private void Start()
    {
        smokeParticleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Calculate emission rate based on car's velocity magnitude
        float emissionRate = carRigidbody.linearVelocity.magnitude * emissionRateMultiplier;

        // Set particle emission rate
        var emission = smokeParticleSystem.emission;
        emission.rateOverTime = emissionRate;
    }
}
