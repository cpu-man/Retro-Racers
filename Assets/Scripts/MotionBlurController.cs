using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MotionBlurController : MonoBehaviour
{
    public Rigidbody carRigidbody; // Reference to the car's Rigidbody component
    public float minSpeed = 0f; // Minimum speed for no motion blur
    public float maxSpeed = 50f; // Maximum speed for maximum motion blur
    public float maxMotionBlur = 1f; // Maximum motion blur intensity at max speed

    private PostProcessVolume postProcessVolume;
    private MotionBlur motionBlur;

    private void Start()
    {
        // Get the Post Process Volume component
        postProcessVolume = GetComponent<PostProcessVolume>();

        // Initialize the Motion Blur effect
        postProcessVolume.profile.TryGetSettings(out motionBlur);
    }

    private void Update()
    {
        // Calculate the speed ratio (0 to 1) based on the car's speed within the minSpeed to maxSpeed range
        float speedRatio = Mathf.Clamp01((carRigidbody.linearVelocity.magnitude - minSpeed) / (maxSpeed - minSpeed));

        // Apply motion blur based on speed
        float motionBlurIntensity = Mathf.Lerp(0f, maxMotionBlur, speedRatio);
        motionBlur.shutterAngle.value = motionBlurIntensity;
    }
}
