using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    [SerializeField] private Transform magnetHolder;
    private Rigidbody rb;
    public void Drop()
    {
        if (rb != null)
        {
            rb.transform.SetParent(null);
            rb.isKinematic = false;
            rb = null;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
            if (otherRb != null)
            {
                rb = otherRb;
                rb.isKinematic = true;
                rb.transform.SetParent(magnetHolder);
                rb.transform.localPosition = Vector3.zero;
                rb.transform.localRotation = Quaternion.identity;


            }
            Debug.Log("Helicopter collided with magnet");
        }
    }
}
