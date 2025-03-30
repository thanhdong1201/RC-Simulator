using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private InputReaderSO inputReader;
    [SerializeField] private InputDirection direction;
    [SerializeField] private Vector2 inputDirection;
    private void Awake()
    {
        InitializeInput();
    }
    private void InitializeInput()
    {
        switch (direction)
        {
            case InputDirection.Up:
                inputDirection = Vector2.up;
                break;
            case InputDirection.Down:
                inputDirection = Vector2.down;
                break;
            case InputDirection.Left:
                inputDirection = Vector2.left;
                break;
            case InputDirection.Right:
                inputDirection = Vector2.right;
                break;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        inputReader.OnPower(inputDirection);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        inputReader.OnPower(Vector2.zero);
    }
}
public enum InputDirection { Up, Down, Left, Right }
