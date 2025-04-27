using UnityEngine;
using UnityEngine.Events;

public class RingTrigger : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    [SerializeField] private UnityEvent onActivateEvent;

    private bool isActivated = false;

    private void OnTriggerExit(Collider other)
    {
        if (isActivated) return;
        if (other.CompareTag("Player")) 
        {
            isActivated = true;
            quest.ProgressStep();
            onActivateEvent?.Invoke();
        }
    }
}
