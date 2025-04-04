using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Transform target;       
    [SerializeField] private Vector3 positionOffset; 
    [SerializeField] private bool followRotation = true;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + positionOffset;

        if (followRotation)
        {
            transform.rotation = target.rotation;
        }
    }
}
