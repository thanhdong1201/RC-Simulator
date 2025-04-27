using UnityEngine;

public class HelicopterUIGamePlay : MonoBehaviour
{
    [SerializeField] private GameObject fireFightingUI;
    [SerializeField] private GameObject transportUI;
    [SerializeField] private GameObject combatUI;
    [SerializeField] private HelicopterListSO helicopterListSO;

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Awake()
    {
        fireFightingUI.SetActive(false);
        combatUI.SetActive(false);
        transportUI.SetActive(false);
        SetUp();
    }
    private void SetUp()
    {
        HelicopterSO currentHelicopter = helicopterListSO.GetCurrentHelicopter();

        if (currentHelicopter.Type == HelicopterType.Toy)
        {
          
        }
        if (currentHelicopter.Type == HelicopterType.Transport)
        {
            transportUI.SetActive(true);
        }
        if (currentHelicopter.Type == HelicopterType.Firefighting)
        {
            fireFightingUI.SetActive(true);
        }
        if (currentHelicopter.Type == HelicopterType.Combat)
        {
            combatUI.SetActive(true);
        }
    }
}
