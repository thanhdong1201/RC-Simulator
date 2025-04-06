using System.Collections.Generic;
using UnityEngine;

public class HelicopterSetup : MonoBehaviour
{
    [SerializeField] private List<GameObject> helicopterList = new List<GameObject>();
    [SerializeField] private HelicopterListSO helicopterListSO;
    [SerializeField] private FollowTransform followTransform;

    private void Awake()
    {
        SetUp();
    }
    private void SetUp()
    {
        HelicopterSO currentHelicopter = helicopterListSO.GetCurrentHelicopter();

        if(currentHelicopter != null)
        {
            for (int i = 0; i < helicopterList.Count; i++)
            {
                if (helicopterList[i].GetComponent<HelicopterName>().HelicopterData == currentHelicopter)
                {
                    helicopterList[i].SetActive(true);
                    followTransform.SetTarget(helicopterList[i].transform);
                }
                else
                {
                    helicopterList[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < helicopterList.Count; i++)
            {
                if (i == 0)
                {
                    helicopterList[i].SetActive(true);
                    followTransform.SetTarget(helicopterList[i].transform);
                }
                else
                {
                    helicopterList[i].SetActive(false);
                }

            }
        }

    }
}
