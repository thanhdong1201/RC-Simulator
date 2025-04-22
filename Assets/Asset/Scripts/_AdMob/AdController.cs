using UnityEngine;

public class AdController : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private VoidEventChannelSO showInterstitial;
    [SerializeField] private VoidEventChannelSO showRewarded;
    private void OnEnable()
    {
        showInterstitial.OnEventRaised += ShowInterstitialAd;
        showRewarded.OnEventRaised += ShowRewardedAd;
    }
    private void OnDestroy()
    {
        showInterstitial.OnEventRaised -= ShowInterstitialAd;
        showRewarded.OnEventRaised -= ShowRewardedAd;
    }
    [Button]
    public void ShowInterstitialAd()
    {
        if (AdManager.Instance.ShowInterstitial())
        {
            AnalyticsManager.Instance.LogAdImpression("interstitial");
        }
    }
    [Button]
    public void ShowRewardedAd()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            AnalyticsManager.Instance.LogAdImpression("rewarded");
        });
    }
}