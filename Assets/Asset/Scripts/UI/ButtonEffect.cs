using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Tween tween;
    private Vector3 LocalScale;
    private void Awake()
    {
        LocalScale = this.transform.localScale;
    }
    private void OnDisable()
    {
        StopAnimation();
    }
    private void OnDestroy()
    {
        StopAnimation();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        tween = this.transform.DOScale(0.8f * LocalScale, 0.2f).SetUpdate(UpdateType.Normal, true);
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        tween = this.transform.DOScale(LocalScale, 0.2f).SetUpdate(UpdateType.Normal, true);
    }
    private void StopAnimation()
    {
        tween?.Kill();
        tween = null;
    }
}
