using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Vector3 LocalScale;
    private void Awake()
    {
        LocalScale = this.transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        this.transform.DOScale(0.8f * LocalScale, 0.2f);
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        this.transform.DOScale(LocalScale, 0.2f);
    }
}
