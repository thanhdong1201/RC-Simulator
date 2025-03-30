using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObjects/Quest")]
public class QuestSO : ScriptableObject
{
    [SerializeField] private string questName;
    [SerializeField] private string description;
    [SerializeField] private string questObjective;
    [SerializeField] private int totalSteps; 
    [SerializeField] private int currentStep;
    [SerializeField] private bool isCompleted;

    public event Action OnProgressStep;
    public event Action OnQuestCompleted;

    public string QuestName => questName;
    public string QuestObjective => questObjective;
    public string Description => description;
    public int TotalSteps => totalSteps;
    public bool IsCompleted => isCompleted;
    public int CurrentStep => currentStep;

    private void OnEnable()
    {
        currentStep = 0;
    }
    public void ProgressStep()
    {
        currentStep++;
        OnProgressStep?.Invoke();
        if (currentStep >= totalSteps)
        {
            isCompleted = true;
            OnQuestCompleted?.Invoke();
        }
    }
}
