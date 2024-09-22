using System;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay;
    public float realMinutesPerGameHour = 1f; // Adjust this to set the time scale, e.g., 1 real minute = 1 game hour
    private float realSecondsPerGameMinute;
    private float timeElapsed;
    private int currentHour;
    private int currentMinute;

    // Define a delegate and an event that passes the current in-game time
    public delegate void TimeEvent(int hour, int minute);
    public static event TimeEvent OnFifteenMinuteInterval;

    private void Start()
    {
        // Calculate how many real seconds are in one game minute based on the scale
        realSecondsPerGameMinute = (realMinutesPerGameHour * 60f) / 60f;

        currentHour = 6;  // Starting at 6 AM
        currentMinute = 0;

        // Initial update to display the starting time
        UpdateTimeDisplay();
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // Advance time when real seconds per game minute have passed
        if (timeElapsed >= realSecondsPerGameMinute)
        {
            currentMinute++;
            timeElapsed = 0;

            if (currentMinute >= 60)
            {
                currentHour++;
                currentMinute = 0;
            }

            if (currentHour >= 24)
            {
                currentHour = 0;
            }

            // Trigger the event every 15 minutes and pass the current time
            if (currentMinute % 15 == 0)
            {
                if (OnFifteenMinuteInterval != null)
                {
                    OnFifteenMinuteInterval.Invoke(currentHour, currentMinute);
                }

                // Only update the time display every 15 minutes
                UpdateTimeDisplay();
            }
        }
    }

    // Update the time display in the UI
    private void UpdateTimeDisplay()
    {
        string hourText = currentHour.ToString("00");
        string minuteText = currentMinute.ToString("00");
        timeDisplay.text = hourText + ":" + minuteText;
    }

    // You can call this method to adjust the time scale during the game
    public void SetTimeScale(float newRealMinutesPerGameHour)
    {
        realMinutesPerGameHour = newRealMinutesPerGameHour;
        realSecondsPerGameMinute = (realMinutesPerGameHour * 60f) / 60f;
    }
}
