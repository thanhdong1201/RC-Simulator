using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Transform sceneSetupParent;
    [SerializeField] private List<SceneSetup> sceneSetupList;

    [Header("UI Loading")]
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Button continueBtn;

    [Header("UI Level View")]
    [SerializeField] private GameObject levelViewUI;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI levelDescription;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button startBtn;

    private AsyncOperation asyncLoad;
    private SceneSO targetScene;

    private void Start()
    {
        startBtn.onClick.AddListener(PrepareToLoadScene);
        loadingUI.SetActive(false);
        levelViewUI.SetActive(false);
    }
    private void OnEnable()
    {
        foreach (SceneSetup setup in sceneSetupList)
        {
            setup.SetUp(this);
        }
    }
    public void SetTargetScene(SceneSO sceneSO)
    {
        targetScene = sceneSO;
        levelName.text = targetScene.questSO.name;
        levelDescription.text = targetScene.questSO.Description;
        levelViewUI.SetActive(true);
    }
    private void PrepareToLoadScene()
    {
        loadingText.text = "Loading...";
        continueBtn.onClick.AddListener(OncontinueBtnClicked);
        continueBtn.gameObject.SetActive(false);
        loadingUI.SetActive(true);
        StartCoroutine(LoadSceneAsyncRoutine());
    }

    private IEnumerator LoadSceneAsyncRoutine()
    {
        asyncLoad = SceneManager.LoadSceneAsync(targetScene.name);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(3f);
        loadingText.text = "Ready! Tap to continue";
        continueBtn.gameObject.SetActive(true);
    }

    private void OncontinueBtnClicked()
    {
        asyncLoad.allowSceneActivation = true;
    }
    private void OnDestroy()
    {
        continueBtn.onClick.RemoveListener(OncontinueBtnClicked);
        startBtn.onClick.RemoveListener(PrepareToLoadScene);
    }
}
