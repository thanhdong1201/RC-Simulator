using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelicopterList", menuName = "Helicopter/Helicopter List")]
public class HelicopterListSO : ScriptableObject
{
    [SerializeField] private List<HelicopterSO> list = new();
    [SerializeField] private HelicopterSO currentHelicopter;

    public HelicopterSO GetCurrentHelicopter() => currentHelicopter;

    public void SetCurrentHelicopter(HelicopterSO hc)
    {
        currentHelicopter = hc;
    }

    public List<HelicopterSO> GetAll() => list;
}
