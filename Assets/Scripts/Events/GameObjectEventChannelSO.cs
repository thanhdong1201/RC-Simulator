using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/GameObject Event Channel")]
public class GameObjectEventChannelSO : ScriptableObject
{
    public UnityAction<GameObject> OnEventRaised;

    public void RaiseEvent(GameObject value)
    {
        OnEventRaised?.Invoke(value);
    }
}
