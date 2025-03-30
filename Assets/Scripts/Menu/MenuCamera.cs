using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [Header("Cinemachine Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("List of Targets")]
    [SerializeField] private List<Transform> targets;

    private int currentIndex = 0;

    private void Start()
    {
        if (targets.Count > 0)
        {
            SetTarget(currentIndex);
        }
    }

    public void Next()
    {
        if (targets.Count == 0) return;

        currentIndex = (currentIndex + 1) % targets.Count;
        SetTarget(currentIndex);
    }

    public void Previous()
    {
        if (targets.Count == 0) return;

        currentIndex = (currentIndex - 1 + targets.Count) % targets.Count;
        SetTarget(currentIndex);
    }

    private void SetTarget(int index)
    {
        virtualCamera.Follow = targets[index];
        virtualCamera.LookAt = targets[index];
    }
}
