using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIGauge : MonoBehaviour
{
    [SerializeField] private Image enginePowerBar;
    [SerializeField] private Ease easeType;
    [SerializeField] private FloatEventChannelSO enginePowerEvent;
    private float previousValue = 0f;

    private void OnEnable()
    {
        enginePowerEvent.OnEventRaised += UpdateGauge;
    }
    private void OnDisable()
    {
        enginePowerEvent.OnEventRaised -= UpdateGauge;
    }
    private void Start()
    {
        enginePowerBar.fillAmount = 0.075f;
    }
    private void UpdateGauge(float value)
    {
        enginePowerBar.fillAmount = DOVirtual.EasedValue(0.075f, 0.435f, value, easeType); // Hoặc Ease.InCubic, Ease.InExpo
    }
}
