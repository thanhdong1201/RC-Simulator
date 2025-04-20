//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro;
//using UnityEngine.UI;
//using GoogleMobileAds.Api;
//using System.Collections;

//public class LoadingSceneManager : MonoBehaviour
//{
//    [Header("UI References")]
//    public GameObject loadingUI;
//    public TextMeshProUGUI loadingText;
//    public Button continueButton;

//    private BannerView bannerView;
//    private static string targetSceneName = "Level1"; // Default
//    private AsyncOperation asyncLoad;

//    public void SetTargetScene(SceneSO sceneSO)
//    {
//        targetSceneName = sceneSO.name;

//        loadingUI.SetActive(true);
//        loadingText.text = "Loading...";
//        continueButton.gameObject.SetActive(false);
//        continueButton.onClick.AddListener(OnContinueButtonClicked);

//        ShowBannerAd();
//        StartCoroutine(LoadSceneAsyncRoutine());
//    }

//    private IEnumerator LoadSceneAsyncRoutine()
//    {
//        asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
//        asyncLoad.allowSceneActivation = false;

//        while (asyncLoad.progress < 0.9f)
//        {
//            loadingText.text = $"Loading... {(asyncLoad.progress * 100f):0}%";
//            yield return null;
//        }

//        loadingText.text = "Ready! Tap to continue";
//        continueButton.gameObject.SetActive(true);
//    }

//    private void OnContinueButtonClicked()
//    {
//        continueButton.interactable = false;
//        bannerView?.Destroy();
//        asyncLoad.allowSceneActivation = true;
//    }

//    private void ShowBannerAd()
//    {
//        string adUnitId = "ca-app-pub-3940256099942544/6300978111"; // Test ID

//        AdSize size = new AdSize(320, 100);
//        bannerView = new BannerView(adUnitId, size, AdPosition.Center);
//        AdRequest request = new AdRequest();
//        bannerView.LoadAd(request);
//    }
//}
