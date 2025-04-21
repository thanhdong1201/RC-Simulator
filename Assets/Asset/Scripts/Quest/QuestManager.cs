using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestSO activeQuestSO;

    private QuestUI questUI;
    private Timer timer;

    private void Awake()
    {
        questUI = FindFirstObjectByType<QuestUI>();
        timer = FindFirstObjectByType<Timer>();
    }
    private void OnEnable()
    {
        //activeQuestSO.OnQuestCompleted += CompleteLevel;
    }

    private void Start()
    {
        questUI.SetUpQuest(activeQuestSO);
        //StartLevel();
    }
    //[Button]
    //public void StartLevel()
    //{
    //    GameAnalyticsManager.Instance.LogLevelStart(activeQuestSO.QuestName);
    //}
    //[Button]
    //public void CompleteLevel()
    //{
    //    GameAnalyticsManager.Instance.LogLevelComplete(activeQuestSO.QuestName, timer.GetTime());
    //    GameAnalyticsManager.Instance.LogSessionDuration(timer.GetTime());
    //    Debug.Log($"Sending play time: {timer.GetTime()}");
    //}
    //[Button]
    //public void FailLevel()
    //{
    //    GameAnalyticsManager.Instance.LogLevelFail(activeQuestSO.QuestName, timer.GetTime());
    //}

    //public void OnCrash(string crashReason, Vector3 position)
    //{
    //    GameAnalyticsManager.Instance.LogCrash(crashReason, position);
    //}
}
