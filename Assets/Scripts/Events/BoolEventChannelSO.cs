using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/Bool Event Channel")]
public class BoolEventChannelSO : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;

    public void RaiseEvent(bool value)
    {
        OnEventRaised?.Invoke(value);
    }
}