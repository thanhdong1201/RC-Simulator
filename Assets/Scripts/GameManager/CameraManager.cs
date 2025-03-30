using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cameras = new List<GameObject>();
    private int currentCameraIndex = 0;
    private void Awake()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].SetActive(i == 0);
        }
    }
    public void SwitchCamera()
    {
        currentCameraIndex++;
        if (currentCameraIndex >= cameras.Count)
        {
            currentCameraIndex = 0;
        }

        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].SetActive(i == currentCameraIndex);
        }
    }
}
