using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump;
using GoogleMobileAds.Ump.Api;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    [Header("AdMob Ad Unit IDs (Replace with real IDs in production)")]
    [SerializeField] private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";

    [Header("Ad Settings")]
    [SerializeField] private float interstitialCooldown = 10f;

    public Action OnRewardedAdCompleted { get; set; }

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private float lastInterstitialTime;

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
    }

    private void Start()
    {
        ConsentRequestParameters request = new ConsentRequestParameters();
        ConsentInformation.Update(request, error =>
        {
            if (error != null)
            {
                Debug.LogWarning($"[AdManager] UMP Update Failed: {error.Message}");
                InitializeAdMob();
                return;
            }

            ConsentForm.LoadAndShowConsentFormIfRequired(formError =>
            {
                if (formError != null)
                {
                    Debug.LogWarning($"[AdManager] UMP Form Failed: {formError.Message}");
                }
                InitializeAdMob();
            });
        });
    }

    private void InitializeAdMob()
    {
        if (!ConsentInformation.CanRequestAds())
        {
            Debug.LogWarning("[AdManager] Cannot request ads yet due to missing consent.");
            return;
        }

        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.Initialize(initStatus =>
        {
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();
        });
    }

    #region Banner Ads
    public void LoadBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        //bannerView = new BannerView(bannerAdUnitId, adaptiveSize, AdPosition.Bottom);
        bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);
        bannerView.LoadAd(new AdRequest());
        bannerView.Hide();
        Debug.Log("[AdManager] Banner Ad Loaded");
    }

    public void ShowBanner()
    {
        if (bannerView != null)
        {
            bannerView.Show();
            Debug.Log("[AdManager] Banner Ad Shown");
        }
        else
        {
            Debug.LogWarning("[AdManager] Banner Ad not ready, reloading");
            LoadBannerAd();
        }
    }

    public void HideBanner()
    {
        bannerView?.Hide();
    }
    #endregion

    #region Interstitial Ads
    public void LoadInterstitialAd()
    {
        interstitialAd?.Destroy();

        InterstitialAd.Load(interstitialAdUnitId, new AdRequest(), (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogWarning($"[AdManager] Interstitial Ad Failed: {error.GetMessage()}");
                Invoke(nameof(LoadInterstitialAd), 10f);
                return;
            }

            interstitialAd = ad;

            interstitialAd.OnAdPaid += (adValue) =>
                Debug.Log($"[AdManager] Interstitial Ad Paid: {adValue.Value / 1_000_000f} {adValue.CurrencyCode}");

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("[AdManager] Interstitial Ad Closed");
                LoadInterstitialAd();
            };

            interstitialAd.OnAdFullScreenContentFailed += (err) =>
            {
                Debug.LogWarning($"[AdManager] Interstitial Ad Failed to show: {err.GetMessage()}");
                LoadInterstitialAd();
            };
        });
    }

    public bool ShowInterstitial()
    {
        if (Time.time - lastInterstitialTime < interstitialCooldown)
        {
            return false;
        }

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            lastInterstitialTime = Time.time;
            Debug.Log("[AdManager] Interstitial Ad Shown");
            return true;
        }

        Debug.LogWarning("[AdManager] Interstitial Ad not ready, reloading");
        LoadInterstitialAd();
        return false;
    }
    #endregion

    #region Rewarded Ads
    public void LoadRewardedAd()
    {
        rewardedAd?.Destroy();

        RewardedAd.Load(rewardedAdUnitId, new AdRequest(), (ad, error) =>
        {
            if (error != null)
            {
                Debug.LogWarning($"[AdManager] Rewarded Ad Failed: {error.GetMessage()}");
                Invoke(nameof(LoadRewardedAd), 10f);
                return;
            }

            rewardedAd = ad;

            rewardedAd.OnAdPaid += (adValue) =>
                Debug.Log($"[AdManager] Rewarded Ad Paid: {adValue.Value / 1_000_000f} {adValue.CurrencyCode}");

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("[AdManager] Rewarded Ad Closed");
                LoadRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += (err) =>
            {
                Debug.LogWarning($"[AdManager] Rewarded Ad Failed to show: {err.GetMessage()}");
                LoadRewardedAd();
            };
        });
    }

    public bool ShowRewardedAd(Action onRewardComplete = null)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(reward =>
            {
                Debug.Log("[AdManager] User earned reward");
                OnRewardedAdCompleted?.Invoke();
                onRewardComplete?.Invoke();
            });
            Debug.Log("[AdManager] Rewarded Ad Shown");
            return true;
        }

        Debug.LogWarning("[AdManager] Rewarded Ad not ready, reloading");
        LoadRewardedAd();
        return false;
    }
    #endregion

    private void OnDestroy()
    {
        bannerView?.Destroy();
        interstitialAd?.Destroy();
        rewardedAd?.Destroy();
    }
}
