using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private int timeInSeconds = 120;
    private int currentSeconds;
    private int currentMinutes;

    [SerializeField] private TextMeshProUGUI timerText;

    private bool isStopped = false;

    private void Start()
    {
        int MINUTE_IN_SECONDS = 60;

        currentMinutes = Mathf.FloorToInt(timeInSeconds / MINUTE_IN_SECONDS);
        currentSeconds = timeInSeconds % MINUTE_IN_SECONDS;

        InvokeRepeating("Tick", 0f, 1f); // tick co sekundê
    }

    void Tick()
    {
        if(isStopped) return;

        currentSeconds -= 1;

        if (currentSeconds < 0)
        {
            currentMinutes -= 1;
            currentSeconds += 60;

            if(currentMinutes < 0)
            {
                Debug.Log("Koniec czasu");
            }
        }

        UpdateTimerText(GetTimerAsString());
    }

    private void UpdateTimerText(string text)
    {
        timerText.text = text;
    }

    private string GetTimerAsString()
    {
        string minutes = currentMinutes.ToString();
        string seconds = currentSeconds.ToString();

        if(currentSeconds < 10)
        {
            seconds = "0" + seconds;
        }

        return minutes + ":" + seconds;
    }

    public float GetPastTime()
    {
        return timeInSeconds - (currentMinutes * 60) - currentSeconds;
    }

    public void Stop()
    {
        isStopped = true;
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        isStopped = false;
        Time.timeScale = 1f;
    }
}
