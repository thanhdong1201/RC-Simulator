using System.Collections.Generic;
using UnityEngine;

public class HelicopterSetup : MonoBehaviour
{
    [SerializeField] private List<GameObject> helicopterList = new List<GameObject>();
    [SerializeField] private HelicopterListSO helicopterListSO;
    [SerializeField] private FollowTransform tpsCam;
    [SerializeField] private FollowTransform fpsCam;

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
                    tpsCam.SetTarget(helicopterList[i].transform);
                    
                    foreach (Transform child in helicopterList[i].transform)
                    {
                        if (child.name == "FpsCam")
                        {
                            fpsCam.SetTarget(child);
                            break;
                        }
                    }
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
                    tpsCam.SetTarget(helicopterList[i].transform);
                }
                else
                {
                    helicopterList[i].SetActive(false);
                }

            }
        }

    }
}
