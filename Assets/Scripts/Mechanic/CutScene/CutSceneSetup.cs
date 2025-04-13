using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static CutSceneStep;

public class CutSceneSetup : MonoBehaviour
{
    [SerializeField] private UIToggleSO uiToggleSO;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private List<CutSceneStep> cutSceneSteps;

    private void Start()
    {
        StartCoroutine(PlayCutScene());
    }
    private IEnumerator PlayCutScene()
    {
        uiToggleSO.TogglePanel(UIPanel.CutScene);

        for (int i = 0; i < cutSceneSteps.Count; i++)
        {
            var step = cutSceneSteps[i];

            if (step.cutSceneEvent != null)
            {
                step.cutSceneEvent.Invoke();
            }

            // Áp dụng kiểu blend cho bước hiện tại
            if (i < cutSceneSteps.Count)
            {
                cinemachineBrain.m_DefaultBlend = new CinemachineBlendDefinition(
                    step.blendStyle == BlendStyle.EaseInOut ? CinemachineBlendDefinition.Style.EaseInOut : CinemachineBlendDefinition.Style.Cut,
                    step.blendStyle == BlendStyle.EaseInOut ? step.blendDuration : 0f
                );
            }
            Debug.Log("Step: "+ i);
            // Chờ thời gian giữ (hold duration)
            yield return new WaitForSeconds(step.blendDuration + step.holdDuration);
        }

        uiToggleSO.TogglePanel(UIPanel.Quest);
        yield return new WaitForSeconds(3f);
        uiToggleSO.TogglePanel(UIPanel.GamePlay);
    }
}

[Serializable]
public class CutSceneStep
{
    public enum BlendStyle { EaseInOut, Cut } 
    public float holdDuration;
    public float blendDuration;
    public BlendStyle blendStyle;
    public UnityEvent cutSceneEvent; 
}