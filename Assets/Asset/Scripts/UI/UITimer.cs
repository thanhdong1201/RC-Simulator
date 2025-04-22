using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if(GameManager.Instance == null || GameManager.Instance.Timer == null) return;
        timerText.text = GameManager.Instance.Timer.GetTime();
    }
}
