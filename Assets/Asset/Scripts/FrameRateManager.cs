using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public enum FrameRateOption
    {
        Unlimit = -1,  
        FPS30 = 30,
        FPS60 = 60,
        FPS90 = 90,
        FPS120 = 120
    }

    public FrameRateOption selectedFrameRate = FrameRateOption.FPS60;

    void Start()
    {
        if (selectedFrameRate == FrameRateOption.Unlimit)
        {
            Application.targetFrameRate = -1; 
        }
        else
        {
            Application.targetFrameRate = (int)selectedFrameRate;
        }
    }
}
