using UnityEngine;
using UnityEngine.Events;

//Use for trigger Quest step
public class QuestStepActivator : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    [SerializeField] private bool onTriggerEnter = true;
    [SerializeField] private UnityEvent onActivateEvent;

    private bool isActivated = false;
    private void OnTriggerEnter(Collider other)
    {
        if(onTriggerEnter)
        {
            AddStep(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!onTriggerEnter)
        {
            AddStep(other);
        }
    }
    private void AddStep(Collider other)
    {
        if (isActivated) return;
        if (other.CompareTag("Player") || other.CompareTag("Interactable"))
        {
            quest.ProgressStep();
            onActivateEvent?.Invoke();

            if(quest.CurrentStep >= quest.TotalSteps)
            {
                isActivated = true;
            }
        }
    }
}
