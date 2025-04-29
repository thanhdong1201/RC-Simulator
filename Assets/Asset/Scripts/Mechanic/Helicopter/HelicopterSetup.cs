using System.Collections.Generic;
using UnityEngine;

public class HelicopterSetup : MonoBehaviour
{
    [Tooltip("Nếu có HelicopterSO thì chỉ có loại heli đó được chơi ở màn này, nếu không thì được chọn tự do")]
    [SerializeField] private HelicopterSO helicopterSO;
    [SerializeField] private List<GameObject> helicopterList = new List<GameObject>();
    [SerializeField] private HelicopterListSO helicopterListSO;
    //[SerializeField] private FollowTransform tpsCam;
    //[SerializeField] private FollowTransform fpsCam;
    [SerializeField] private Transform tpsCam;
    [SerializeField] private Transform fpsCam;

    private void Awake()
    {
        if(helicopterSO != null)
        {
            helicopterListSO.SetCurrentHelicopter(helicopterSO);
        }
    }
    private void Start()
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
                    tpsCam.SetParent(helicopterList[i].transform);
                    tpsCam.localPosition = Vector3.zero;
                    tpsCam.localRotation = Quaternion.identity;
                    tpsCam.localScale = Vector3.one;

                    foreach (Transform child in helicopterList[i].transform)
                    {
                        if (child.name == "FpsCam")
                        {
                            fpsCam.SetParent(child);
                            fpsCam.localPosition = Vector3.zero;
                            fpsCam.localRotation = Quaternion.identity;
                            fpsCam.localScale = Vector3.one;
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
            //for (int i = 0; i < helicopterList.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        helicopterList[i].SetActive(true);
            //        tpsCam.SetTarget(helicopterList[i].transform);
            //    }
            //    else
            //    {
            //        helicopterList[i].SetActive(false);
            //    }

            //}
        }

    }
}
