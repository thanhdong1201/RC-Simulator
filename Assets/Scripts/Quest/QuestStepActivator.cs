using UnityEngine;
using UnityEngine.Events;

//Use for trigger Quest step
public class QuestStepActivator : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    [SerializeField] private UnityEvent onActivateEvent;
    private bool isActivated = false;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            quest.ProgressStep();
            onActivateEvent?.Invoke();
        }
    }
}
