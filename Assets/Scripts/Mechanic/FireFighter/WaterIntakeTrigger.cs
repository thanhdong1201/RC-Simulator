using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterIntakeTrigger : MonoBehaviour
{
    public bool isInTrigger { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
    }
}
