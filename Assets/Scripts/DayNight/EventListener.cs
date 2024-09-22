using UnityEngine;

public class EventListener : MonoBehaviour
{
    private void OnEnable()
    {
        TimeManager.OnFifteenMinuteInterval += HandleTimeEvent;
    }

    private void OnDisable()
    {
        TimeManager.OnFifteenMinuteInterval -= HandleTimeEvent;
    }

    private void HandleTimeEvent(int hour, int minute)
    {
        Debug.Log($"15 minutes have passed in the game! Current time: {hour}:{minute:00}");
        // Add your logic here for what should happen based on the time
    }
}