using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelicopterShop : MonoBehaviour
{
    [Header("Cinemachine Virtual Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [Header("Data")]
    [SerializeField] private HelicopterListSO helicopterListSO;
    [Header("List of Models")]
    [SerializeField] private List<Transform> lookAtModels;
    [SerializeField] private List<GameObject> models;

    private HelicopterSO currentHelicopter;
    private int currentIndex = 0;

    [Header("Button")]
    [SerializeField] private Button equippBtn;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button previousBtn;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI helicopterNameTxt;


    private void Awake()
    {
        equippBtn.onClick.AddListener(Equipp);
        buyBtn.onClick.AddListener(Buy);
        nextBtn.onClick.AddListener(Next);
        previousBtn.onClick.AddListener(Previous);
    }
    private void OnDestroy()
    {
        equippBtn.onClick.RemoveListener(Equipp);
        buyBtn.onClick.RemoveListener(Buy);
        nextBtn.onClick.RemoveListener(Next);
        previousBtn.onClick.RemoveListener(Previous);
    }
    private void Start()
    {
        Setup();
    }
    private void Setup()
    {
        currentHelicopter = helicopterListSO.GetCurrentHelicopter();

        if (currentHelicopter != null)
        {
            for (int i = 0; i < lookAtModels.Count; i++)
            {
                if (currentHelicopter == lookAtModels[i].GetComponent<HelicopterName>().HelicopterData)
                {
                    currentIndex = i;
                    break;
                }
            }
        }

        SetTarget(currentIndex);
    }

    private void Equipp()
    {
        helicopterListSO.SetCurrentHelicopter(currentHelicopter);
    }
    private void Buy()
    {

    }
    public void Next()
    {
        if (lookAtModels.Count == 0) return;

        currentIndex = (currentIndex + 1) % lookAtModels.Count;
        SetTarget(currentIndex);
    }

    public void Previous()
    {
        if (lookAtModels.Count == 0) return;

        currentIndex = (currentIndex - 1 + lookAtModels.Count) % lookAtModels.Count;
        SetTarget(currentIndex);
    }
    private void SetTarget(int index)
    {
        for (int i = 0; i < models.Count; i++)
        {
            models[i].SetActive(i == index);
        }

        virtualCamera.Follow = lookAtModels[index];
        virtualCamera.LookAt = lookAtModels[index];

        currentHelicopter = lookAtModels[index].GetComponent<HelicopterName>().HelicopterData;
        helicopterNameTxt.text = currentHelicopter.Name;
    }

}
