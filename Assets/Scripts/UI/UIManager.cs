using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIToggleSO uiToggle;
    [Header("Panel")]
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private GameObject levelPanel;

    private Dictionary<UIPanel, GameObject> uiPanels;

    private void Awake()
    {
        InitializePanels();
    }
    private void OnEnable()
    {
        uiToggle.OnTogglePanel += TogglePanel;
    }
    private void OnDestroy()
    {
        uiToggle.OnTogglePanel -= TogglePanel;
    }
    private void InitializePanels()
    {
        uiPanels = new Dictionary<UIPanel, GameObject>
        {
            {UIPanel.GamePlay, gamePlayPanel},
            {UIPanel.Pause, pausePanel},
            {UIPanel.GameOver, gameOverPanel},
            {UIPanel.Complete, completePanel},
            {UIPanel.Level, levelPanel},
        };

        TogglePanel(UIPanel.GamePlay);
    }
    private void TogglePanel(UIPanel panel)
    {
        foreach (var key in uiPanels.Keys)
        {
            uiPanels[key]?.SetActive(key == panel);
        }
    }
}
public enum UIPanel
{
    None,
    MainMenu,
    GamePlay,
    Pause,
    GameOver,
    Complete,
    Tutorial,
    Level,
}
