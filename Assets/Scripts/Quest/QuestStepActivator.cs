using UnityEngine;

//Use for trigger Quest step
public class QuestStepActivator : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    private bool isActivated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            quest.ProgressStep();
        }
    }
}
