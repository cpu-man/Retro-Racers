using UnityEngine;
using UnityEngine.UI;

public class CarHUD : MonoBehaviour
{
    public Rigidbody carRigidbody;
    public Engine carEngine;
    public Text speedometerText;
    public Text rpmText;

    private void Update()
    {
        if (carRigidbody == null || carEngine == null) return;

        // Update speedometer
        float speed = carRigidbody.linearVelocity.magnitude * 3.6f; // Convert m/s to km/h
        speedometerText.text = "Speed: " + Mathf.Round(speed) + " km/h";

        // Update RPM gauge
        float rpm = carEngine.currentRPM;
        rpmText.text = "RPM: " + Mathf.Round(rpm);
    }
}
