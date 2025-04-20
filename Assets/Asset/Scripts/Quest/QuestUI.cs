using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private UIToggleSO uiToggle;
    [SerializeField] private TextMeshProUGUI questNameText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private TextMeshProUGUI questNoficationText;
    [SerializeField] private TextMeshProUGUI questProgressText;

    private QuestSO quest;

    private void OnDestroy()
    {
        quest.OnQuestCompleted -= OnQuestCompleted;
        quest.OnProgressStep -= UpdateText;
    }
    public void SetUpQuest(QuestSO questSO)
    {
        quest = questSO;

        quest.OnQuestCompleted += OnQuestCompleted;
        quest.OnProgressStep += UpdateText;

        quest.ResetQuestStep();

        questNameText.text = quest.QuestName;
        questProgressText.text = quest.QuestObjective + $": {quest.CurrentStep}/{quest.TotalSteps}";
        questDescriptionText.text = quest.Description;
    }
    private void OnQuestCompleted()
    {
        uiToggle.TogglePanel(UIPanel.Complete);
    }
    private void UpdateText()
    {
        questProgressText.text = quest.QuestObjective + $": {quest.CurrentStep}/{quest.TotalSteps}";
        if (quest.CurrentStep >= quest.TotalSteps)
        {
            questNoficationText.gameObject.SetActive(true);
            questNoficationText.text = "Landing on helicopter platform!";
        }
    }
}
