using UnityEngine;

public class WaterTank : MonoBehaviour
{
    [Header("WaterTank Settings")]
    [SerializeField] private float maxWater = 100f;
    [SerializeField] private float waterChangeRate = 0.5f;
    [SerializeField] private float currentWater;
    [SerializeField] private float maxRaycastDistance = 1f;
    [SerializeField] private ParticleSystem waterSprayVfx;

    [Header("References")]
    [SerializeField] private WaterIntakeTrigger waterIntakeTrigger;

    [Header("Events")]
    [SerializeField] private FloatEventChannelSO enginePowerEvent;
    [SerializeField] private InputReaderSO inputReader;

    private RaycastHit hit;
    private bool isSpraying = false;

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
        if (Physics.Raycast(waterIntakeTrigger.transform.position, Vector3.down, out hit, maxRaycastDistance))
        {
            if(hit.collider.CompareTag("Fire") && isSpraying)
            {
                FirePoint firePoint = hit.collider.GetComponent<FirePoint>();
                if(firePoint != null)
                {
                    firePoint.AddWater();
                }
            }
        }

        if (waterIntakeTrigger.isInTrigger)
        {
            RefillWater();
        }

        SprayWater();
    }
    private void SprayWater()
    {
        if (isSpraying)
        {
            if (CurrentWater > 0f)
            {
                if (!waterSprayVfx.isPlaying) waterSprayVfx.Play();

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
        if (CurrentWater < maxWater)
        {
            CurrentWater += waterChangeRate * Time.deltaTime;
        }
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * maxRaycastDistance);
    }
}
