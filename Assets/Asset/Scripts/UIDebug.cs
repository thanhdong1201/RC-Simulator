using TMPro;
using UnityEngine;

public class UIDebug : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private float deltaTime = 0.0f;

    private void Update()
    {
        ShowFPS();
    }
    private void ShowFPS()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.SetText("FPS: " + Mathf.Ceil(fps));
    }
}
