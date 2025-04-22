using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Timer Timer { get; private set; }


    [Header("Events")]
    [SerializeField] private VoidEventChannelSO menuEvent;
    [SerializeField] private VoidEventChannelSO replayEvent;

    private void OnEnable()
    {
        menuEvent.OnEventRaised += MainMenu;
        replayEvent.OnEventRaised += Replay;
    }
    private void OnDestroy()
    {
        menuEvent.OnEventRaised -= MainMenu;
        replayEvent.OnEventRaised -= Replay;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Timer = gameObject.AddComponent<Timer>();
    }
    private void Start()
    {
        AnalyticsManager.Instance.WaitForInitialization(()=> LogStartSession());
    }
    private void Update()
    {
        Timer.UpdateTimer();
    }

    #region Analytics
    private void OnApplicationPause(bool pause)
    {
        if (pause) LogSessionDuration();
    }
    private void OnApplicationQuit()
    {
        LogSessionDuration();
    }
    private void LogStartSession()
    {
        AnalyticsManager.Instance.LogGameStart();
    }
    private void LogSessionDuration()
    {
        AnalyticsManager.Instance.LogSessionDuration(Timer.GetTime());
    }
    private void LogCrash(string crashReason, Vector3 position)
    {
        AnalyticsManager.Instance.LogCrash(crashReason, position);
    }
    #endregion

    #region Load Scene
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
