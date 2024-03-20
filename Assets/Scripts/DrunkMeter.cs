using UnityEngine;
using UnityEngine.UI;

public class DrunkMeter : MonoBehaviour
{
    public DrunkMechanic drunkMechanic; // Reference to the DrunkMechanic script
    public Slider drunkSlider; // Reference to the UI Slider representing the drunk meter

    void Update()
    {
        // Ensure that the references are set
        if (drunkMechanic == null || drunkSlider == null)
        {
            Debug.LogWarning("References not set for DrunkMeter!");
            return;
        }

        // Update the UI slider value based on the current drunk intensity
        drunkSlider.value = drunkMechanic.drunkIntensity / drunkMechanic.maxDrunkIntensity;
    }
}
