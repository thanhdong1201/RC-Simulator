using UnityEngine;

public class WaterTank : MonoBehaviour
{
    [Header("WaterTank Settings")]
    [SerializeField] private float maxWater = 100f;
    [SerializeField] private float waterChangeRate = 0.5f;
    [SerializeField] private float currentWater;
    [SerializeField] private ParticleSystem waterSprayVfx;

    [Header("Events")]
    [SerializeField] private FloatEventChannelSO enginePowerEvent;
    [SerializeField] private InputReaderSO inputReader;

    private RaycastHit hit;
    private bool isSpraying = false;
    private bool isRefilling = false;

    private void OnEnable()
    {
        inputReader.InteractEvent += ()=> isSpraying = !isSpraying;
    }
    private void OnDestroy()
    {
        inputReader.InteractEvent -= () => isSpraying = !isSpraying;
    }
    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            if (hit.collider.CompareTag("Water"))
            {
                RefillWater();
            }
            if(hit.collider.CompareTag("Fire") && isSpraying)
            {
                FirePoint firePoint = hit.collider.GetComponent<FirePoint>();
                if(firePoint != null)
                {
                    firePoint.AddWater();
                }
            }
        }

        SprayWater();
    }
    private void SprayWater()
    {
        if (isSpraying)
        {
            if (CurrentWater > 0f)
            {
                if (!waterSprayVfx.isPlaying)
                {
                    waterSprayVfx.Play();
                }

                CurrentWater -= waterChangeRate * 0.8f * Time.deltaTime;
            }
            if (CurrentWater <= 0f)
            {
                isSpraying = false;
                waterSprayVfx.Stop();
            }
        }
        else
        {
            if (waterSprayVfx.isPlaying) waterSprayVfx.Stop();
        }
    }
    private void RefillWater()
    {
        CurrentWater += waterChangeRate * Time.deltaTime;
    }
    public float CurrentWater
    {
        get => currentWater;
        set
        {
            currentWater = Mathf.Clamp(value, 0f, maxWater);
            enginePowerEvent?.RaiseEvent(currentWater / maxWater);
        }
    }
}
