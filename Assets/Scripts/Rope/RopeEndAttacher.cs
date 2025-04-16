using UnityEngine;

public class RopeEndAttacher : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader;
    [SerializeField] private RopeVerlet ropeVerlet; 

    private Transform attachedObject; 
    private FixedJoint fixedJoint;
    private Rigidbody attachedRb;
    private bool isHookedSomething => attachedObject != null;

    private void Start()
    {
        fixedJoint = GetComponent<FixedJoint>();
        if (fixedJoint == null)
        {
            fixedJoint = gameObject.AddComponent<FixedJoint>();
        }
    }
    private void OnEnable() => inputReader.InteractEvent += HandleInteractEvent;
    private void OnDestroy() => inputReader.InteractEvent -= HandleInteractEvent;
    private void HandleInteractEvent()
    {
        if (isHookedSomething)
        {
            UnhookObject();
        }
    }
    public void HookObject(Transform newObject)
    {
        if (newObject == null) return;

        attachedObject = newObject;
        attachedRb = newObject.GetComponent<Rigidbody>();
        attachedRb.useGravity = false;
        attachedRb.velocity = Vector3.zero; 
        attachedRb.angularVelocity = Vector3.zero; 

        fixedJoint.connectedBody = attachedRb;
        fixedJoint.autoConfigureConnectedAnchor = false;
        fixedJoint.anchor = Vector3.zero; // Gắn vào tâm của vật thể
        fixedJoint.connectedAnchor = Vector3.zero; // Gắn vào tâm của đoạn cuối dây
    }
    public void UnhookObject()
    {
        if (attachedObject == null || fixedJoint == null) return;

        attachedRb.useGravity = true;
        fixedJoint.connectedBody = null; 
        attachedObject = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isHookedSomething) return;

        if (collision.gameObject.CompareTag("Interactable"))
        {
            HookObject(collision.transform);
        }
    }
}