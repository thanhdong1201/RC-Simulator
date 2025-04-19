using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] private Transform sceneSetupParent;
    private SceneSO targetScene;
    [SerializeField] private List<SceneSetup> sceneSetupList;

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
        selectUI.SetActive(true);
    }

    [Header("UI References")]
    [SerializeField] private GameObject selectUI;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Button continueButton;

    private AsyncOperation asyncLoad;

    public void LoadScene()
    {
        PrepareToLoadScene();
    }
    private void PrepareToLoadScene()
    {
        loadingUI.SetActive(true);
        loadingText.text = "Loading...";
        continueButton.gameObject.SetActive(false);
        continueButton.onClick.AddListener(OnContinueButtonClicked);
        StartCoroutine(LoadSceneAsyncRoutine());
    }

    private IEnumerator LoadSceneAsyncRoutine()
    {
        asyncLoad = SceneManager.LoadSceneAsync(targetScene.name);
        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSeconds(3f);
        loadingText.text = "Ready! Tap to continue";
        continueButton.gameObject.SetActive(true);
    }

    private void OnContinueButtonClicked()
    {
        asyncLoad.allowSceneActivation = true;
    }
    private void OnDestroy()
    {
        continueButton.onClick.RemoveListener(OnContinueButtonClicked);
    }
}
