using UnityEngine;

public class SteeringWheelAnimation : MonoBehaviour
{
    private Animator animator;
    private float currentSteeringAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Get steering input (between -1 and 1) and convert it to an angle range (-45 to 45 degrees)
        float steeringInput = Input.GetAxis("Horizontal");
        currentSteeringAngle = Mathf.Lerp(-45f, 45f, (steeringInput + 1f) / 2f);

        // Update the animation parameter (if using parameters)
        animator.SetFloat("SteeringAngle", currentSteeringAngle);
    }
}
