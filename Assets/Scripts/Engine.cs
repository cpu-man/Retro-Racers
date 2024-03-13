using UnityEngine;

public class Engine : MonoBehaviour
{
    public AnimationCurve torqueCurve; // Define your torque curve in the Unity editor

    public float maxRPM = 6000f; // Maximum RPM of the engine
<<<<<<< HEAD
    
    private float currentRPM; // Current RPM of the engine
    private float currentTorque; // Current torque output of the engine


    // Method to calculate current torque based on RPM and throttle input
    public float GetCurrentTorque(float throttleInput)
    {
        // Interpolate torque based on current RPM using the torque curve
        currentTorque = torqueCurve.Evaluate(currentRPM / maxRPM);

        // Adjust torque based on throttle input
        currentTorque *= throttleInput;

=======

    private float currentRPM; // Current RPM of the engine
    private float currentTorque; // Current torque output of the engine

    public float GetCurrentTorque()
    {
        // Interpolate torque based on current RPM using the torque curve
        currentTorque = torqueCurve.Evaluate(currentRPM / maxRPM);
>>>>>>> parent of c24ff6f (engine update wip)
        return currentTorque;
    }

    public void SetThrottle(float throttleInput)
    {
        // Adjust RPM based on throttle input
        float targetRPM = maxRPM * throttleInput;
        currentRPM = Mathf.Clamp(targetRPM, 0f, maxRPM);
    }
}
