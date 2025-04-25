using System.Collections.Generic;
using UnityEngine;

public class RopeVerlet : MonoBehaviour
{
    [Header("Rope")]
    [SerializeField] private int numberOfRopeSegments = 15;
    [SerializeField] private float ropeSegmentLength = 0.2f;
    [SerializeField] private float ropeWidth = 0.05f;
    [SerializeField] private Material ropeMaterial;

    [Header("Physics")]
    [SerializeField] private Vector3 gravityForce = new Vector3(0, -2f, 0);
    [SerializeField] private float dampingFactor = 0.95f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.1f;
    [SerializeField] private float bounceFactor = 0f;

    [Header("Constraints")]
    [SerializeField] private int numberOfConstraintRuns = 30;

    [Header("Optimizations")]
    [SerializeField] private int collisionSegmentInterval = 2;

    [Header("Transform")]
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private Transform endPoint;

    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private LineRenderer lineRenderer;
    private Vector3 ropeStartPoint;
    private Vector3 lastAnchorPosition;

    public struct RopeSegment
    {
        public Vector3 CurrentPosition;
        public Vector3 OldPosition;
        public RopeSegment(Vector3 pos)
        {
            CurrentPosition = pos;
            OldPosition = pos;
        }
    }

    private void Awake()
    {
        SetupLineRenderer();
        lineRenderer.positionCount = numberOfRopeSegments;
        lineRenderer.useWorldSpace = true;

        ropeStartPoint = anchorPoint.position;
        lastAnchorPosition = ropeStartPoint;
        for (int i = 0; i < numberOfRopeSegments; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegmentLength;
        }
    }
    void SetupLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = numberOfRopeSegments;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        lineRenderer.material = ropeMaterial != null ? ropeMaterial : new Material(Shader.Find("Sprites/Default"));
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
    }
    private void FixedUpdate()
    {
        ropeStartPoint = anchorPoint.position;
        Vector3 anchorVelocity = (ropeStartPoint - lastAnchorPosition) / Time.fixedDeltaTime;
        lastAnchorPosition = ropeStartPoint;

        Simulate(anchorVelocity);

        for (int i = 0; i < numberOfConstraintRuns; i++)
        {
            ApplyConstraints();
            if (i % collisionSegmentInterval == 0)
            {
                HandleCollision();
            }
        }
    }

    private void LateUpdate()
    {
        float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        Vector3[] interpolatedPositions = new Vector3[numberOfRopeSegments];
        for (int i = 0; i < numberOfRopeSegments; i++)
        {
            interpolatedPositions[i] = Vector3.Lerp(
                ropeSegments[i].OldPosition,
                ropeSegments[i].CurrentPosition,
                t
            );
        }
        lineRenderer.SetPositions(interpolatedPositions);

        endPoint.position = GetRopeEndInterpolatedPosition();
    }

    // Phương thức mới để truy cập điểm cuối của dây
    public Vector3 GetRopeEndPosition()
    {
        if (ropeSegments.Count == 0) return transform.position; // Dự phòng nếu danh sách rỗng
        return ropeSegments[ropeSegments.Count - 1].CurrentPosition;
    }

    // Phương thức mới để truy cập vị trí nội suy của điểm cuối
    public Vector3 GetRopeEndInterpolatedPosition()
    {
        if (ropeSegments.Count == 0) return transform.position;
        float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        return Vector3.Lerp(
            ropeSegments[ropeSegments.Count - 1].OldPosition,
            ropeSegments[ropeSegments.Count - 1].CurrentPosition,
            t
        );
    }

    private void Simulate(Vector3 anchorVelocity)
    {
        for (int i = 0; i < ropeSegments.Count; i++)
        {
            RopeSegment segment = ropeSegments[i];
            Vector3 velocity = (segment.CurrentPosition - segment.OldPosition) * dampingFactor;

            if (i == 0)
            {
                velocity = anchorVelocity * dampingFactor;
            }

            segment.OldPosition = segment.CurrentPosition;
            segment.CurrentPosition += velocity;
            segment.CurrentPosition += gravityForce * Time.fixedDeltaTime;
            ropeSegments[i] = segment;
        }
    }

    private void ApplyConstraints()
    {
        RopeSegment firstSegment = ropeSegments[0];
        firstSegment.CurrentPosition = ropeStartPoint;
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < numberOfRopeSegments - 1; i++)
        {
            RopeSegment currentSegment = ropeSegments[i];
            RopeSegment nextSegment = ropeSegments[i + 1];

            Vector3 delta = nextSegment.CurrentPosition - currentSegment.CurrentPosition;
            float currentDistance = delta.magnitude;
            float error = currentDistance - ropeSegmentLength;

            if (currentDistance > 0.0001f)
            {
                Vector3 correction = delta / currentDistance * error * 0.3f;

                if (i == 0)
                {
                    nextSegment.CurrentPosition -= correction * 2f;
                }
                else
                {
                    currentSegment.CurrentPosition += correction;
                    nextSegment.CurrentPosition -= correction;
                }
            }

            ropeSegments[i] = currentSegment;
            ropeSegments[i + 1] = nextSegment;
        }
    }

    private void HandleCollision()
    {
        for (int i = 1; i < ropeSegments.Count; i++)
        {
            RopeSegment segment = ropeSegments[i];
            Vector3 velocity = segment.CurrentPosition - segment.OldPosition;

            Collider[] colliders = Physics.OverlapSphere(segment.CurrentPosition, collisionRadius, collisionMask);
            foreach (Collider collider in colliders)
            {
                bool isValidCollider = collider is BoxCollider || collider is SphereCollider || collider is CapsuleCollider || (collider is MeshCollider meshCollider && meshCollider.convex);
                if (!isValidCollider) return;

                Vector3 closestPoint = collider.ClosestPoint(segment.CurrentPosition);
                float distance = Vector3.Distance(segment.CurrentPosition, closestPoint);

                if (distance < collisionRadius && distance > 0.0001f)
                {
                    Vector3 normal = (segment.CurrentPosition - closestPoint).normalized;
                    float depth = collisionRadius - distance;

                    segment.CurrentPosition += normal * depth * 0.5f;
                    velocity = Vector3.Reflect(velocity, normal) * bounceFactor;
                }
            }

            segment.OldPosition = segment.CurrentPosition - velocity;
            ropeSegments[i] = segment;
        }
    }
}