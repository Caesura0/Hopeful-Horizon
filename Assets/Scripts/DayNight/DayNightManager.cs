using UnityEngine;
using UnityEngine.Rendering.Universal;
public class DayNightManager : MonoBehaviour
{
    public Light2D globalLight;  // Reference to the Global Light 2D component
    public float dayIntensity = 1f;   // Light intensity during the day
    public float nightIntensity = 0.2f;  // Light intensity during the night
    public int sunriseHour = 6;  // The hour the sun rises
    public int sunsetHour = 18;  // The hour the sun sets
    public float transitionDuration = 2f;  // Duration of the fade (in seconds)

    private float targetIntensity;
    private float currentTransitionTime;

    private void OnEnable()
    {
        // Subscribe to the 15-minute interval event
        TimeManager.OnFifteenMinuteInterval += AdjustLighting;
    }

    private void OnDisable()
    {
        // Unsubscribe when this script is disabled
        TimeManager.OnFifteenMinuteInterval -= AdjustLighting;
    }

    // Adjust lighting based on the time of day
    // Adjust lighting based on the time of day
    private void AdjustLighting(int hour, int minute)
    {
        if (globalLight != null)
        {
            if (hour >= sunriseHour && hour < sunsetHour)
            {
                // Daytime: Target the day intensity
                targetIntensity = dayIntensity;
            }
            else
            {
                // Nighttime: Target the night intensity
                targetIntensity = nightIntensity;
            }

            // Reset the transition time to start fading smoothly
            currentTransitionTime = 0;
        }
    }

    private void Update()
    {
        if (globalLight != null && currentTransitionTime < transitionDuration)
        {
            // Smoothly transition the light intensity over time using Mathf.Lerp
            currentTransitionTime += Time.deltaTime;
            float t = currentTransitionTime / transitionDuration;
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, targetIntensity, t);
        }
    }
}
