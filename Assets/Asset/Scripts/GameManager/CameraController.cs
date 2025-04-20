using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float sensitivityX = 2f;
    [SerializeField] private float sensitivityY = 2f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private float resetSpeed = 2f;      // Hệ số reset (có thể tăng giá trị này nếu reset quá chậm)\n
    [SerializeField] private float resetThreshold = 0.01f; // Ngưỡng để coi đã về 0

    [SerializeField] private InputReaderSO inputReader;

    private Vector2 lookInput;
    private float yaw = 0f;
    private float pitch = 0f;
    private Coroutine resetCoroutine;

    private void Awake()
    {
        yaw = 0f;
        pitch = 0f;
    }

    private void OnEnable() => inputReader.LookEvent += OnLookInput;
    private void OnDisable() => inputReader.LookEvent -= OnLookInput;

    private void OnLookInput(Vector2 value)
    {
        lookInput = value;

        // Nếu có input, cập nhật góc và dừng coroutine reset (nếu đang chạy)
        if (value != Vector2.zero)
        {
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
                resetCoroutine = null;
            }
            yaw += value.x * sensitivityX * Time.deltaTime;
            pitch = Mathf.Clamp(pitch - value.y * sensitivityY * Time.deltaTime, minPitch, maxPitch);
        }
        else
        {
            // Nếu không có input và coroutine reset chưa chạy, bắt đầu reset camera
            if (resetCoroutine == null)
            {
                resetCoroutine = StartCoroutine(ResetCamera());
            }
        }
    }

    private void LateUpdate()
    {
        if (lookTarget == null) return;
        // Cập nhật rotation khi có input hoặc đang reset
        if (lookInput != Vector2.zero || resetCoroutine != null)
        {
            lookTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    private IEnumerator ResetCamera()
    {
        // Reset mượt mà về 0 dựa trên độ chênh lệch hiện tại (distance)
        while (Mathf.Abs(yaw) > resetThreshold || Mathf.Abs(pitch) > resetThreshold)
        {
            // Tính khoảng cách lớn nhất giữa yaw và pitch so với 0
            float distance = Mathf.Max(Mathf.Abs(yaw), Mathf.Abs(pitch));
            // Điều chỉnh bước di chuyển: khi distance lớn thì bước di chuyển lớn, giúp reset nhanh hơn
            float moveStep = resetSpeed * distance * Time.deltaTime;
            yaw = Mathf.MoveTowards(yaw, 0f, moveStep);
            pitch = Mathf.MoveTowards(pitch, 0f, moveStep);
            lookTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
            yield return null;
        }

        // Đảm bảo đặt chính xác về 0
        yaw = 0f;
        pitch = 0f;
        lookTarget.rotation = Quaternion.identity;
        resetCoroutine = null;
    }
}
