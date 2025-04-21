using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class GameAnalyticsManager : MonoBehaviour
{
    public static GameAnalyticsManager Instance { get; private set; }

    private bool isInitialized = false;
    private const string GAME_NAME = "Helicopter_RC_Simulator";

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

    /// <summary>
    /// Khởi tạo Firebase Analytics.
    /// </summary>
    private void InitializeFirebase()
    {
        Debug.Log("[GameAnalyticsManager] 🔄 Initializing Firebase Analytics...");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var status = task.Result;

            if (status == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                isInitialized = true;
                Debug.Log($"[GameAnalyticsManager] ✅ Firebase Analytics initialized for: {GAME_NAME}");
                LogGameStart();
            }
            else
            {
                Debug.LogError($"[GameAnalyticsManager] ❌ Firebase init failed: {status}");
            }
        });
    }

    public void LogGameStart()
    {
        if (!EnsureInitialized("LogGameStart")) return;

        FirebaseAnalytics.LogEvent("game_start", new Parameter("game_name", GAME_NAME));
        Debug.Log("[GameAnalyticsManager] 📊 Event logged: game_start");
        EnableAnalytics();
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
            new Parameter("position_z", pos.z)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: crash | Reason: {reason} | Position: {pos}");
    }

    public void LogUpgrade(string type, int level, int cost, string currency)
    {
        if (!EnsureInitialized("LogUpgrade")) return;

        FirebaseAnalytics.LogEvent(
            "upgrade_event",
            new Parameter("upgrade_type", type),
            new Parameter("upgrade_level", level),
            new Parameter("cost", cost),
            new Parameter("currency", currency)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: upgrade | Type: {type} | Level: {level} | Cost: {cost} {currency}");
    }

    public void LogAdImpression(string adType, string adUnitId, string adPlatform)
    {
        if (!EnsureInitialized("LogAdImpression")) return;

        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventAdImpression,
            new Parameter("ad_type", adType),
            new Parameter("ad_unit_id", adUnitId),
            new Parameter("ad_platform", adPlatform)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: ad_impression | Type: {adType} | Unit: {adUnitId} | Platform: {adPlatform}");
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

    public void LogCustomEvent(string eventName, params Parameter[] parameters)
    {
        if (!EnsureInitialized("LogCustomEvent")) return;

        if (string.IsNullOrEmpty(eventName) || eventName.Length > 40)
        {
            Debug.LogWarning($"[GameAnalyticsManager] ⚠️ Invalid event name: {eventName}");
            return;
        }

        FirebaseAnalytics.LogEvent(eventName, parameters);
        Debug.Log($"[GameAnalyticsManager] 📊 Custom event logged: {eventName}");
    }

    public void LogAchievementUnlocked(string id, string name)
    {
        if (!EnsureInitialized("LogAchievementUnlocked")) return;

        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventUnlockAchievement,
            new Parameter("achievement_id", id),
            new Parameter("achievement_name", name)
        );
        Debug.Log($"[GameAnalyticsManager] 📊 Event logged: achievement_unlocked | {name}");
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

    public void DisableAnalytics()
    {
        if (!EnsureInitialized("DisableAnalytics")) return;

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(false);
        Debug.Log("[GameAnalyticsManager] 🔒 Analytics disabled");
    }

    public void EnableAnalytics()
    {
        if (!EnsureInitialized("EnableAnalytics")) return;

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Debug.Log("[GameAnalyticsManager] 🔓 Analytics enabled");
    }

    /// <summary>
    /// Kiểm tra đã init Firebase chưa, nếu chưa thì log warning.
    /// </summary>
    private bool EnsureInitialized(string methodName)
    {
        if (!isInitialized)
        {
            Debug.LogWarning($"[GameAnalyticsManager] ⚠️ {methodName} failed: Firebase chưa khởi tạo");
            return false;
        }
        return true;
    }
}
