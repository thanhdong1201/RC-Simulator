using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HelicopterList", menuName = "Helicopter/Helicopter List")]
public class HelicopterListSO : ScriptableObject
{
    [SerializeField] private List<HelicopterSO> list = new();
    [SerializeField] private HelicopterSO current;

    public HelicopterSO GetCurrentHelicopter() => current;

    public void SetCurrentHelicopter(HelicopterSO h)
    {
        current = h;
    }

    public List<HelicopterSO> GetAll() => list;
}
