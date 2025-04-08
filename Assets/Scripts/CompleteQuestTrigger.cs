using UnityEngine;

public class CompleteQuestTrigger : MonoBehaviour
{
    [SerializeField] private QuestSO quest;

    private void OnTriggerEnter(Collider other)
    {
        quest.CompleteQuest();
    }
}
