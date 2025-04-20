using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MagnetHook : MonoBehaviour
{
    public PhysicalRope physicalRope;

    private Rigidbody magnetRb;
    private ConfigurableJoint jointToRope; // Joint gắn MagnetHook vào đoạn cuối của dây
    private ConfigurableJoint jointToObject; // Joint gắn MagnetHook vào object Interactable
    private bool isHookingObject = false;

    private void Start()
    {
        if (physicalRope == null)
        {
            Debug.LogError("PhysicalRope is not assigned in MagnetHook!");
            enabled = false;
            return;
        }

        magnetRb = GetComponent<Rigidbody>();
        if (magnetRb == null)
        {
            Debug.LogError("MagnetHook requires a Rigidbody!");
            enabled = false;
            return;
        }

        // Thiết lập MagnetHook để follow đoạn cuối của dây
        SetupRopeAttachment();
    }

    private void SetupRopeAttachment()
    {
        // Lấy đoạn cuối của dây
        Transform endTransform = physicalRope.GetEndTransform();
        Rigidbody endRigidbody = physicalRope.GetEndRigidbody();

        if (endTransform == null || endRigidbody == null)
        {
            Debug.LogError("Cannot attach to rope: End segment not found!");
            return;
        }

        // Di chuyển MagnetHook đến vị trí đoạn cuối
        transform.position = endTransform.position;

        // Tạo joint để gắn MagnetHook vào đoạn cuối của dây
        jointToRope = gameObject.AddComponent<ConfigurableJoint>();
        jointToRope.connectedBody = endRigidbody;
        jointToRope.autoConfigureConnectedAnchor = false;
        jointToRope.anchor = Vector3.zero;
        jointToRope.connectedAnchor = Vector3.zero; // Gắn vào tâm của đoạn cuối

        jointToRope.xMotion = ConfigurableJointMotion.Limited;
        jointToRope.yMotion = ConfigurableJointMotion.Limited;
        jointToRope.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = new SoftJointLimit { limit = physicalRope.GetSegmentLength() * 0.1f };
        jointToRope.linearLimit = limit;

        jointToRope.angularXMotion = ConfigurableJointMotion.Limited;
        jointToRope.angularYMotion = ConfigurableJointMotion.Limited;
        jointToRope.angularZMotion = ConfigurableJointMotion.Limited;

        JointDrive angularDrive = new JointDrive
        {
            positionSpring = physicalRope.GetSpringStrength(),
            positionDamper = physicalRope.GetDamperStrength(),
            maximumForce = Mathf.Infinity
        };
        jointToRope.angularXDrive = angularDrive;
        jointToRope.angularYZDrive = angularDrive;
    }

    public void HookObject(Transform obj)
    {
        if (obj == null || magnetRb == null || physicalRope == null)
        {
            Debug.LogWarning("Cannot hook: Invalid components.");
            return;
        }

        Rigidbody targetRb = obj.GetComponent<Rigidbody>();
        if (targetRb == null)
        {
            Debug.LogWarning("Target object does not have a Rigidbody!");
            return;
        }

        // Xóa joint cũ nếu có
        UnhookObject();

        // Tạo joint mới để gắn vào object
        jointToObject = gameObject.AddComponent<ConfigurableJoint>();
        jointToObject.connectedBody = targetRb;
        jointToObject.autoConfigureConnectedAnchor = false;
        jointToObject.anchor = Vector3.zero;
        jointToObject.connectedAnchor = Vector3.zero;

        jointToObject.xMotion = ConfigurableJointMotion.Limited;
        jointToObject.yMotion = ConfigurableJointMotion.Limited;
        jointToObject.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = new SoftJointLimit { limit = physicalRope.GetSegmentLength() * 0.1f };
        jointToObject.linearLimit = limit;

        jointToObject.angularXMotion = ConfigurableJointMotion.Limited;
        jointToObject.angularYMotion = ConfigurableJointMotion.Limited;
        jointToObject.angularZMotion = ConfigurableJointMotion.Limited;

        JointDrive angularDrive = new JointDrive
        {
            positionSpring = physicalRope.GetSpringStrength(),
            positionDamper = physicalRope.GetDamperStrength(),
            maximumForce = Mathf.Infinity
        };
        jointToObject.angularXDrive = angularDrive;
        jointToObject.angularYZDrive = angularDrive;

        isHookingObject = true;
    }

    public void UnhookObject()
    {
        if (jointToObject != null)
        {
            Destroy(jointToObject);
            jointToObject = null;
        }
        isHookingObject = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHookingObject || physicalRope == null) return;

        Debug.Log($"Collision with {collision.gameObject.name}, Tag: {collision.gameObject.tag}");
        if (collision.gameObject.CompareTag("Interactable"))
        {
            HookObject(collision.transform);
        }
    }

    private void OnDestroy()
    {
        UnhookObject();
        if (jointToRope != null)
        {
            Destroy(jointToRope);
        }
    }
}