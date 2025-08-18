#region Raycast in game
/* Raycast in game (very buggy)
using UnityEngine;
using TiltFive;

[RequireComponent(typeof(LineRenderer))]
public class WandLaser : MonoBehaviour
{
    private Rigidbody grabbedRb;             // current flashbang held
    public Transform wandHoldPoint;          // empty child where object snaps
    public float rayLength = 2000f;

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2; // start + end
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = Color.red;
    }

    void Update()
    {
        if (!TiltFive.Wand.IsTracked())
        {
            lr.enabled = false;
            return;
        }

        float trigger = TiltFive.Input.GetTrigger();

        // Always draw the laser
        lr.enabled = true;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Vector3 endPos = transform.position + transform.forward * rayLength;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            endPos = hit.point;

            // Laser turns green if pointing at flashbang
            lr.material.color = hit.collider.CompareTag("Flashbang") ? Color.green : Color.red;

            // Grab object when trigger pressed
            if (trigger > 0.8f)
            {
                if (grabbedRb == null && hit.collider.CompareTag("Flashbang"))
                {
                    grabbedRb = hit.collider.attachedRigidbody;
                    if (grabbedRb != null)
                    {
                        grabbedRb.isKinematic = true;
                        grabbedRb.transform.position = wandHoldPoint.position;
                        grabbedRb.transform.rotation = wandHoldPoint.rotation;
                        grabbedRb.transform.SetParent(wandHoldPoint);
                    }
                }
            }
        }

        // If holding one, keep it stuck
        if (grabbedRb != null && trigger > 0.8f)
        {
            grabbedRb.transform.position = wandHoldPoint.position;
            grabbedRb.transform.rotation = wandHoldPoint.rotation;
        }
        else if (grabbedRb != null && trigger <= 0.8f)
        {
            // Release
            grabbedRb.isKinematic = false;
            grabbedRb.transform.SetParent(null);
            grabbedRb = null;
        }

        // Update line renderer positions
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, endPos);
    }
}
*/
#endregion
#region Raycast only scene preview
/* Raycast Old Method (no ray in game)
using UnityEngine;
using TiltFive;

public class WandRaycast : MonoBehaviour
{
    private Rigidbody grabbedRb; // the flashbang currently held
    public Transform wandHoldPoint; // empty child object where the flashbang attaches

    void Update()
    {
        if (!TiltFive.Wand.IsTracked()) return;

        float trigger = TiltFive.Input.GetTrigger();

        // If holding trigger strongly
        if (trigger > 0.8f)
        {
            if (grabbedRb == null)
            {
                // Raycast forward
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2000f))
                {
                    if (hit.collider.CompareTag("Flashbang"))
                    {
                        // Grab the flashbang
                        grabbedRb = hit.collider.attachedRigidbody;
                        if (grabbedRb != null)
                        {
                            // Disable physics so it follows cleanly
                            grabbedRb.isKinematic = true;

                            // Snap to hold point
                            grabbedRb.transform.position = wandHoldPoint.position;
                            grabbedRb.transform.rotation = wandHoldPoint.rotation;

                            // Parent it to the wand
                            grabbedRb.transform.SetParent(wandHoldPoint);
                        }
                    }
                }
            }
            // If already holding one, keep it stuck to hold point
            else
            {
                grabbedRb.transform.position = wandHoldPoint.position;
                grabbedRb.transform.rotation = wandHoldPoint.rotation;
            }
        }
        else
        {
            // Release if holding one
            if (grabbedRb != null)
            {
                grabbedRb.isKinematic = false; // physics on again
                grabbedRb.transform.SetParent(null);
                grabbedRb = null;
            }
        }
    }
}
*/
#endregion

#region Using collision
using UnityEngine;
using TiltFive; // don’t forget namespace

public class WandCollisionPickup : MonoBehaviour
{
    private Transform objectToDrag;   // current flashbang being held
    public Transform wandHoldPoint;   // empty child object on wand (where flashbang sticks)
    private bool isHolding = false;

    void Update()
    {
        float trigger = TiltFive.Input.GetTrigger();

        // If holding, keep the flashbang stuck to wand
        if (isHolding && objectToDrag != null)
        {
            objectToDrag.position = wandHoldPoint.position;
            objectToDrag.rotation = wandHoldPoint.rotation;

            // Release when trigger is let go
            if (trigger < 0.2f)
            {
                Rigidbody rb = objectToDrag.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;

                objectToDrag = null;
                isHolding = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float trigger = TiltFive.Input.GetTrigger();

        // Grab when trigger pressed while colliding with a flashbang
        if (other.CompareTag("Flashbang") && !isHolding && trigger > 0.8f)
        {
            objectToDrag = other.transform;
            isHolding = true;

            Rigidbody rb = other.attachedRigidbody;
            if (rb != null) rb.isKinematic = true;
        }
    }
}

#endregion