using UnityEngine;


public class FloatingPower : MonoBehaviour
{
    [SerializeField] float buoyancyForce = 10f;
    [SerializeField] float waterHeight = 0f;
    [SerializeField] float dampingFactor = 0.5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (transform.position.y < waterHeight)
        {
            float depth = waterHeight - transform.position.y;
            Vector3 upward = Vector3.up * buoyancyForce * depth;
            Vector3 damping = -rb.velocity * dampingFactor;

            rb.AddForce(upward + damping, ForceMode.Force);
        }
    }
}
