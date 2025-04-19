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
    // Gọi từ UI Button (Interstitial)
    [Button]
    public void ShowInterstitialAd()
    {
        if (AdManager.Instance.ShowInterstitial())
        {
            Debug.Log("Interstitial Ad triggered");
        }
    }

    // Gọi từ UI Button (Rewarded)
    [Button]
    public void ShowRewardedAd()
    {
        AdManager.Instance.ShowRewardedAd(() =>
        {
            Debug.Log($"Rewarded Ad completed");
        });
    }
}