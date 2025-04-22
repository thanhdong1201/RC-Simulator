using Firebase;
using Firebase.Analytics;
using System;
using System.Collections;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private bool isInitialized = false;
    private const string GAME_NAME = "Helicopter_RC_Simulator";

    private int maxRetryAttempts = 3;
    private float retryDelay = 5f;
    private int retryCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var status = task.Result;

            if (status == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                isInitialized = true;
                Debug.Log($"[GameAnalyticsManager] ✅ Firebase Analytics initialized for: {GAME_NAME}");
                retryCount = 0;
            }
            else
            {
                Debug.LogError($"[GameAnalyticsManager] ❌ Firebase init failed: {status}");
                if (retryCount < maxRetryAttempts)
                {
                    retryCount++;
                    Debug.Log($"[GameAnalyticsManager] 🔄 Retrying initialization (Attempt {retryCount}/{maxRetryAttempts})...");
                    Invoke(nameof(InitializeFirebase), retryDelay);
                }
                else
                {
                    Debug.LogError("[GameAnalyticsManager] ❌ Max retry attempts reached. Firebase Analytics will not be available.");
                }
            }
        });
    }

    public void LogGameStart()
    {
        if (!EnsureInitialized("LogGameStart")) return;

        FirebaseAnalytics.LogEvent("game_start", new Parameter("game_name", GAME_NAME));
        Debug.Log("[GameAnalyticsManager] 📊 Event logged: game_start");
    }

    public void LogLevelStart(string levelName)
    {
        if (!EnsureInitialized("LogLevelStart")) return;

        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelStart,
            new Parameter("level_name", levelName)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: level_start | Level: {levelName}");
    }

    public void LogLevelComplete(string levelName, string completionTime)
    {
        if (!EnsureInitialized("LogLevelComplete")) return;

        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelEnd,
            new Parameter("level_name", levelName),
            new Parameter("completion_time_seconds", completionTime),
            new Parameter("success", 1)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: level_complete | Level: {levelName} | Time: {completionTime}s");
    }

    public void LogLevelFail(string levelName, string timeSpent)
    {
        if (!EnsureInitialized("LogLevelFail")) return;

        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelEnd,
            new Parameter("level_name", levelName),
            new Parameter("time_spent_seconds", timeSpent),
            new Parameter("success", 0)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: level_fail | Level: {levelName} | Time: {timeSpent}s");
    }

    public void LogCrash(string reason, Vector3 pos)
    {
        if (!EnsureInitialized("LogCrash")) return;

        FirebaseAnalytics.LogEvent(
            "crash_event",
            new Parameter("crash_reason", reason),
            new Parameter("position_x", pos.x),
            new Parameter("position_y", pos.y),
            new Parameter("position_z", pos.z),
            new Parameter("device_model", SystemInfo.deviceModel),
            new Parameter("game_version", Application.version)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: crash | Reason: {reason} | Position: {pos}");
    }

    public void LogAdImpression(string adType)
    {
        if (!EnsureInitialized("LogAdImpression")) return;

        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventAdImpression,
            new Parameter("ad_type", adType)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: ad_impression | Type: {adType}");
    }

    public void LogRewardedAdCompleted(string adUnitId, string rewardType, int amount)
    {
        if (!EnsureInitialized("LogRewardedAdCompleted")) return;

        FirebaseAnalytics.LogEvent(
            "rewarded_ad_completed",
            new Parameter("ad_unit_id", adUnitId),
            new Parameter("reward_type", rewardType),
            new Parameter("reward_amount", amount)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: rewarded_ad_completed | Reward: {amount} {rewardType}");
    }

    public void LogSessionDuration(string seconds)
    {
        if (!EnsureInitialized("LogSessionDuration")) return;

        FirebaseAnalytics.LogEvent(
            "session_duration",
            new Parameter("duration_seconds", seconds)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: session_duration | {seconds}s");
    }

    private bool EnsureInitialized(string methodName)
    {
        if (!isInitialized)
        {
            Debug.LogWarning($"[GameAnalyticsManager] ⚠️ {methodName} failed: Firebase chưa khởi tạo");
            return false;
        }
        return true;
    }
    public void WaitForInitialization(Action action)
    {
        StartCoroutine(Delay(action));
    }
    private IEnumerator Delay(Action action)
    {
        while (!isInitialized)
        {
            yield return null;
        }
        action?.Invoke();
    }
}
