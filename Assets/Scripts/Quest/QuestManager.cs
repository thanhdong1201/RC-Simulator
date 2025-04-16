using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestSO activeQuestSO;

    private QuestUI questUI;

    private void Awake()
    {
        questUI = FindFirstObjectByType<QuestUI>();
    }
    private void Start()
    {
        questUI.SetUpQuest(activeQuestSO);
    }

}
