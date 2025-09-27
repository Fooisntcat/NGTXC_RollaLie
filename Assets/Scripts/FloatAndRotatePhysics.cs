using UnityEngine;

public class FloatAndRotatePhysics : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);

    private Rigidbody rb;
    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate; // smooth movement
    }

    void FixedUpdate()
    {
        // target float position
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 targetPos = new Vector3(startPos.x, newY, startPos.z);

        // smooth move instead of snapping
        Vector3 smoothPos = Vector3.Lerp(rb.position, targetPos, 0.1f);

        rb.MovePosition(smoothPos);

        // smooth rotation
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime));
    }
}
