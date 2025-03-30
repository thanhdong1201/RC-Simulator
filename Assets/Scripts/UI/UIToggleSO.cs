using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UIToggleSO", menuName = "ScriptableObjects/UIToggleSO")]
public class UIToggleSO : ScriptableObject
{
    public UnityAction<UIPanel> OnTogglePanel;

    public void TogglePanel(UIPanel panel)
    {
        OnTogglePanel?.Invoke(panel);
    }
    public void ResumeBtn()
    {
        TogglePanel(UIPanel.GamePlay);
        SetGamePaused(false);
    }
    public void PauseBtn()
    {
        TogglePanel(UIPanel.Pause);
        SetGamePaused(true);
    }
    public void LevelBtn()
    {
        TogglePanel(UIPanel.Level);
        SetGamePaused(true);
    }
    public void SetGamePaused(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        AudioListener.pause = isPaused;
    }
    private void OnEnable()
    {
        SetGamePaused(false);
    }
}
