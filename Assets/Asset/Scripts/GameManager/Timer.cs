using UnityEngine;

public class Timer : MonoBehaviour
{
    private float elapsedTime;
    private float lastUpdateTime;
    private const float UPDATE_INTERVAL = 1f; // Cập nhật mỗi giây

    private void Start()
    {
        elapsedTime = 0f;
        lastUpdateTime = Time.realtimeSinceStartup;
    }

    public void UpdateTimer()
    {
        float currentTime = Time.realtimeSinceStartup;
        if (currentTime - lastUpdateTime >= UPDATE_INTERVAL)
        {
            elapsedTime += currentTime - lastUpdateTime;
            lastUpdateTime = currentTime;
        }
    }

    public string GetTime()
    {
        int minutes = (int)(elapsedTime / 60f);
        int seconds = (int)(elapsedTime % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}
