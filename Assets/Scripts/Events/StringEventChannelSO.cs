using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/String Event Channel")]
public class StringEventChannelSO : ScriptableObject
{
    public UnityAction<string> OnEventRaised;

    public void RaiseEvent(string text)
    {
        OnEventRaised?.Invoke(text);
    }
}