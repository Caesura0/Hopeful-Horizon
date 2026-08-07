using System;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public TextMeshProUGUI timeDisplay;
    public float realMinutesPerGameHour = 1f; // Adjust this to set the time scale, e.g., 1 real minute = 1 game hour
    private float realSecondsPerGameMinute;
    private float timeElapsed;
    private int currentHour;
    private int currentMinute;
    private int currentDay = 1;

    // Define a delegate and an event that passes the current in-game time
    public delegate void TimeEvent(int hour, int minute);
    public static event TimeEvent OnFifteenMinuteInterval;
    public static event TimeEvent OnThirtyMinuteInterval;
    
    public static event Action<int> OnHourChanged;
    public static event Action<int> OnDayChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
                OnHourChanged?.Invoke(currentHour);
            }

            if (currentHour >= 24)
            {
                currentHour = 0;
                currentDay++;
                OnDayChanged?.Invoke(currentDay);
            }

            // Trigger the event every 15 minutes and pass the current time
            if (currentMinute % 15 == 0)
            {
                if (OnFifteenMinuteInterval != null)
                {
                    OnFifteenMinuteInterval.Invoke(currentHour, currentMinute);
                }
                
                if (currentMinute % 30 == 0)
                {
                    if (OnThirtyMinuteInterval != null)
                    {
                        OnThirtyMinuteInterval.Invoke(currentHour, currentMinute);
                    }
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

    public int GetCurrentHour() => currentHour;
    public int GetCurrentMinute() => currentMinute;
}
