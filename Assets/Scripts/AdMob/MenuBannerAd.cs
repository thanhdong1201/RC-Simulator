using UnityEngine;

public class MenuBannerAd : MonoBehaviour
{
    private void Start()
    {
        AdManager.Instance?.ShowBanner();
    }

    private void OnDestroy()
    {
        AdManager.Instance?.HideBanner();
    }
}
