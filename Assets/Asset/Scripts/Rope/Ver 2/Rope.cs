using System.Collections.Generic;
using UnityEngine;

public class PhysicalRope : MonoBehaviour
{
    [Header("Rope Settings")]
    public int segmentCount = 20;
    public float segmentLength = 0.3f;
    public float ropeWidth = 0.05f;
    public float segmentMass = 1f;
    public float segmentDrag = 4f;
    public float segmentAngularDrag = 4f;

    [Header("Tension Settings")]
    public float springStrength = 3000f;
    public float damperStrength = 200f;

    [Header("Visuals")]
    public Material ropeMaterial;

    private List<Transform> segmentTransforms = new List<Transform>();
    private LineRenderer lineRenderer;
    private Rigidbody lastSegmentRb;

    /// <summary> Trả về Rigidbody của đoạn cuối dây, để hệ thống khác gắn object. </summary>
    public Rigidbody GetEndRigidbody() => lastSegmentRb;
    /// <summary> Trả về Transform của đoạn cuối dây. </summary>
    public Transform GetEndTransform() => segmentTransforms.Count > 0 ? segmentTransforms[^1] : null;
    public float GetSegmentLength() => segmentLength;
    public float GetSpringStrength() => springStrength;
    public float GetDamperStrength() => damperStrength;

    void Awake()
    {
        SetupLineRenderer();
        CreateRopeSegments();
    }

    void Update()
    {
        UpdateLineRenderer();
    }

    void SetupLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = segmentCount;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        lineRenderer.material = ropeMaterial != null ? ropeMaterial : new Material(Shader.Find("Sprites/Default"));
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
    }

    void CreateRopeSegments()
    {
        Rigidbody previousRb = GetComponent<Rigidbody>();
        if (previousRb == null)
        {
            previousRb = gameObject.AddComponent<Rigidbody>();
            previousRb.isKinematic = true;
        }

        Vector3 currentPosition = transform.position;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            segment.layer = LayerMask.NameToLayer("RopeSegment");
            segment.transform.SetParent(transform);
            Destroy(segment.GetComponent<MeshRenderer>());
            segment.name = $"RopeSegment_{i}";
            segment.transform.localScale = new Vector3(0.05f, segmentLength / 2f, 0.05f);
            currentPosition -= Vector3.up * segmentLength;
            segment.transform.position = currentPosition;

            Rigidbody rb = segment.AddComponent<Rigidbody>();
            rb.mass = segmentMass;
            rb.drag = segmentDrag;
            rb.angularDrag = segmentAngularDrag;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.useGravity = true;

            CapsuleCollider col = segment.GetComponent<CapsuleCollider>();
            col.direction = 1;

            ConfigurableJoint joint = segment.AddComponent<ConfigurableJoint>();
            joint.connectedBody = previousRb;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector3.up * segmentLength / 2f;
            joint.connectedAnchor = Vector3.down * segmentLength / 2f;

            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit limit = new SoftJointLimit { limit = segmentLength };
            joint.linearLimit = limit;

            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Limited;

            JointDrive angularDrive = new JointDrive
            {
                positionSpring = springStrength,
                positionDamper = damperStrength,
                maximumForce = Mathf.Infinity
            };
            joint.angularXDrive = angularDrive;
            joint.angularYZDrive = angularDrive;

            previousRb = rb;
            lastSegmentRb = rb;
            segmentTransforms.Add(segment.transform);
        }
    }

    void UpdateLineRenderer()
    {
        for (int i = 0; i < segmentTransforms.Count; i++)
        {
            lineRenderer.SetPosition(i, segmentTransforms[i].position);
        }
    }
}
