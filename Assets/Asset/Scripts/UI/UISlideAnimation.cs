using UnityEngine;
using DG.Tweening;

public class UISlideAnimation : MonoBehaviour
{
    [SerializeField] private float delay = 0f;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private Ease easeOpen = Ease.OutBack;
    [SerializeField] private Ease easeClose = Ease.InBack;
    [SerializeField] private float slideOffset = 800f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Tween tween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        PlayOpenAnimation();
    }

    private void OnDisable()
    {
        StopAnimation();
    }

    private void OnDestroy()
    {
        StopAnimation();
    }

    public void PlayOpenAnimation()
    {
        rectTransform.anchoredPosition = originalPosition - new Vector2(0, slideOffset);
        tween = rectTransform.DOAnchorPos(originalPosition, duration).SetEase(easeOpen).SetDelay(delay).SetUpdate(UpdateType.Normal, true);
    }

    public void PlayCloseAnimation()
    {
        tween = rectTransform.DOAnchorPos(originalPosition - new Vector2(0, slideOffset), duration).SetEase(easeClose).SetDelay(delay).SetUpdate(UpdateType.Normal, true).OnComplete(() => gameObject.SetActive(false));
    }

    private void StopAnimation()
    {
        tween?.Kill();
        tween = null;
    }
}
