using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float sensitivityX = 150f;
    [SerializeField] private float sensitivityY = 150f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private float resetSpeed = 3f;

    [SerializeField] private InputReaderSO inputReader;

    private Vector2 lookInput;
    private float yaw = 0f;
    private float pitch = 0f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        if (lookTarget == null) return;

        initialPosition = Vector3.zero;
        initialRotation = Quaternion.identity;
        yaw = 0f;
        pitch = 0f;

        lookTarget.position = initialPosition;
        lookTarget.rotation = initialRotation;
    }

    private void OnEnable() => inputReader.LookEvent += OnLookInput;
    private void OnDisable() => inputReader.LookEvent -= OnLookInput;

    private void OnLookInput(Vector2 value)
    {
        lookInput = value;

        if (value != Vector2.zero)
        {
            yaw += value.x * sensitivityX * Time.deltaTime;
            pitch = Mathf.Clamp(pitch - value.y * sensitivityY * Time.deltaTime, minPitch, maxPitch);
        }
    }

    private void LateUpdate()
    {
        if (lookTarget == null) return;

        if (lookInput != Vector2.zero)
        {
            lookTarget.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        else
        {
            // Reset default rotation when have no input
            float moveStep = resetSpeed * Time.deltaTime;
            lookTarget.localRotation = Quaternion.Slerp(lookTarget.localRotation, initialRotation, moveStep);

            // Update yaw and pitch to 0
            float maxAngle = Mathf.Max(Mathf.Abs(yaw), Mathf.Abs(pitch));
            float angleStep = resetSpeed * maxAngle * Time.deltaTime;
            yaw = Mathf.MoveTowards(yaw, 0f, angleStep);
            pitch = Mathf.MoveTowards(pitch, 0f, angleStep);
        }
    }
}