using UnityEngine;
using TMPro;

public class CarHUD : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public Engine engine;
    public TextMeshProUGUI speedometerText;
    public TextMeshProUGUI rpmText;


    private void Update()
    {
        if (carRigidbody == null || engine == null) return;

        // Update speedometer
        float speed = carRigidbody.linearVelocity.magnitude * 3.6f; // Convert m/s to km/h
        speedometerText.text = "Speed: " + Mathf.Round(speed) + " km/h";

        // Update RPM gauge
        float rpm = engine.currentRPM;
        rpmText.text = "RPM: " + Mathf.Round(rpm);
    }
}
