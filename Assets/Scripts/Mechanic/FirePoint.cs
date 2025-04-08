using UnityEngine;

public class FirePoint : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    [SerializeField] private ParticleSystem fireVfx;
    [SerializeField] private float waterNeeded = 5f; // Lượng nước cần để dập lửa

    private float currentWater = 0f;
    private bool alreadyExtinguished = false;   
    public void AddWater()
    {
        currentWater += Time.deltaTime;
        if (currentWater >= waterNeeded)
            Extinguish();
    }

    private void Extinguish()
    {
        if (alreadyExtinguished) return; 
        alreadyExtinguished = true; 
        quest.ProgressStep();
        fireVfx.Stop();
        Destroy(this, 2f); // Không cho thêm nước nữa
    }
}
