using UnityEngine;

public class GameManeger : MonoBehaviour
{
    [SerializeField]private float timer = 0f; // Timer to keep track of the game time
    private string TimerText; // Text to display the timer in the UI

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string UpdateTimerText()
    {
        timer += Time.deltaTime; // Increment the timer by the time since the last frame
        TimerText = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timer / 60), Mathf.FloorToInt(timer % 60));
        return TimerText; // Return the formatted timer text
    }
}
