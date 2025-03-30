using UnityEngine;
using UnityEngine.EventSystems;

public abstract class JoystickBaseInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Rect References")]
    [SerializeField] protected RectTransform containerRect;
    [SerializeField] protected RectTransform handleRect;

    [Header("Settings")]
    [SerializeField] protected float joystickRange = 50f;
    [SerializeField] protected float magnitudeMultiplier = 1f;
    [SerializeField] protected bool invertXOutputValue;
    [SerializeField] protected bool invertYOutputValue;

    [Header("Input Reader")]
    [SerializeField] protected InputReaderSO inputReader;

    protected virtual void Start() => ResetHandle();

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);
        position = ApplySizeDelta(position);
        Vector2 clampedPosition = ClampValuesToMagnitude(position);
        Vector2 outputPosition = ApplyInversionFilter(clampedPosition) * magnitudeMultiplier;

        HandleInput(outputPosition);

        if (handleRect) handleRect.anchoredPosition = clampedPosition * joystickRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HandleInput(Vector2.zero);
        ResetHandle();
    }

    protected abstract void HandleInput(Vector2 input);

    private void ResetHandle()
    {
        if (handleRect) handleRect.anchoredPosition = Vector2.zero;
    }

    private Vector2 ApplySizeDelta(Vector2 position)
    {
        float x = (position.x / containerRect.sizeDelta.x) * 2.5f;
        float y = (position.y / containerRect.sizeDelta.y) * 2.5f;
        return new Vector2(x, y);
    }

    private Vector2 ClampValuesToMagnitude(Vector2 position) => Vector2.ClampMagnitude(position, 1);

    private Vector2 ApplyInversionFilter(Vector2 position)
    {
        if (invertXOutputValue) position.x = -position.x;
        if (invertYOutputValue) position.y = -position.y;
        return position;
    }
}
