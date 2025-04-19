using DG.Tweening;
using UnityEngine;

public class UILoopScaleAnimation : MonoBehaviour
{
    [SerializeField] private float targetScale = 1.1f;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private Ease ease = Ease.InOutSine;
    [SerializeField] private bool playOnEnable = true;

    private RectTransform rectTransform;
    private Tween scaleTween;
    private Vector3 startScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startScale = rectTransform.localScale;
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            StartLoop();
        }
    }

    private void OnDestroy()
    {
        StopLoop();
    }
    public void StartLoop()
    {
        StopLoop();
        rectTransform.localScale = startScale;

        scaleTween = rectTransform
            .DOScale(targetScale, duration)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); 
    }

    public void StopLoop()
    {
        scaleTween?.Kill();
        scaleTween = null;
        rectTransform.localScale = startScale;
    }
}
