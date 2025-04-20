using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Fire(Vector3 direction, float speed)
    {
        rb.velocity = direction * speed;
    }
}
