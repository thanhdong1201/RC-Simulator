using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    [SerializeField] private UIToggleSO uiToggle;
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private TextMeshProUGUI questProgressText;

    private void OnEnable()
    {
        quest.OnQuestCompleted += OnQuestCompleted;
        quest.OnProgressStep += UpdateText;
    }
    private void OnDisable()
    {
        quest.OnQuestCompleted -= OnQuestCompleted;
        quest.OnProgressStep -= UpdateText;
    }
    private void OnQuestCompleted()
    {
        uiToggle.TogglePanel(UIPanel.Complete);
    }
    private void Start()
    {
        questNameText.text = quest.QuestName;
        //questDescriptionText.text = quest.Description;
        questProgressText.text = quest.QuestObjective + $": {quest.CurrentStep}/{quest.TotalSteps}";
    }
    private void UpdateText()
    {
        questProgressText.text = quest.QuestObjective + $": {quest.CurrentStep}/{quest.TotalSteps}";
    }
}
