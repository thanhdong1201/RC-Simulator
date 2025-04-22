using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestSO activeQuestSO;

    private QuestUI questUI;
    private Timer timer;

    private void Awake()
    {
        questUI = FindFirstObjectByType<QuestUI>();
    }
    private void OnEnable()
    {
        activeQuestSO.OnQuestCompleted += CompleteLevel;
    }
    private void OnDestroy()
    {
        activeQuestSO.OnQuestCompleted -= CompleteLevel;
    }

    private void Start()
    {
        questUI.SetUpQuest(activeQuestSO);
        timer = GameManager.Instance.Timer;
        
        AnalyticsManager.Instance.WaitForInitialization(() => StartLevel());
    }

    [Button]
    public void StartLevel()
    {
        AnalyticsManager.Instance.LogLevelStart(activeQuestSO.QuestName);
    }
    [Button]
    public void CompleteLevel()
    {
        AnalyticsManager.Instance.LogLevelComplete(activeQuestSO.QuestName, timer.GetTime());
    }
    [Button]
    public void FailLevel()
    {
        AnalyticsManager.Instance.LogLevelFail(activeQuestSO.QuestName, timer.GetTime());
    }
}
