using UnityEngine;
using UnityEngine.UI;
using TiltFive;

public class WandFlashbang : MonoBehaviour
{
    public Image whiteScreen;
    public AudioSource bangSound, whiteNoise;
    public float flashDuration = 2f;
    // public float throwForce = 10f;
    // public float grabDistance = 2f;
    // public float positionLerpSpeed = 20f;
    // public float rotationLerpSpeed = 15f;

    private Rigidbody rb;
    // private bool isDragging = false;
    private bool hasFlashed = false;
    // private bool isGrabbed = false;
    // private Vector3 grabOffset;
    // private Quaternion grabRotationOffset;
    // public Transform wandLocation;
    // private Vector3 lastPosition;
    // private Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    #region Wrongly put script (raycast should be in WandRaycast not in the Flashbang)
    /*
    void Update()
    {
        /*
        if (!TiltFive.Wand.IsTracked()) return;

        Vector3 wandPos = wandLocation.position;
        Quaternion wandRot = wandLocation.rotation;
        float trigger = TiltFive.Input.GetTrigger();

        // Grab logic
        // if (trigger > 0.8f && !isDragging)
        if (trigger > 0.8f)
        {
            // if (IsThisGrenadeBeingPointedAt(wandPos, wandRot))
            // {
            // StartGrab(wandPos, wandRot);
            // }

            // Doesnt work with rigidbody .iskinematic = false
            // transform.position = wandPos;

            // Works the Rigidbody
            // rb.MovePosition(wandLocation.position);
            // rb.MoveRotation(wandLocation.rotation);

            // Raycast method
            /* 
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 20f))
            {
                
                if (hit.collider.CompareTag("Grenade"))
                {
                    StartGrab(wandPos, wandRot);
                }
                
            if (hit.collider.CompareTag("Flashbang"))
            {
                rb.MovePosition(wandLocation.position);
                rb.MoveRotation(wandLocation.rotation);
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * 20f, Color.red);
            }
            


            // Raycast
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2000f))
            {
                if (hit.collider.CompareTag("Flashbang"))
                {
                    Debug.DrawRay(transform.position, transform.forward * 2000f, Color.green);
                    rb.MovePosition(wandLocation.position);
                    rb.MoveRotation(wandLocation.rotation);
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.forward * 2000f, Color.red);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * 2000f, Color.red);
            }

        }
        // Release logic
        // else if (trigger < 0.2f && isDragging && isGrabbed)
        // else if (trigger < 0.2f && isDragging)
        else if (trigger < 0.2f)
        {
            ReleaseGrenade(wandPos);
        }

        // Smooth follow when grabbed
        // if (isGrabbed)
        // {
            // velocity = (transform.position - lastPosition) / Time.deltaTime;
            // lastPosition = transform.position;
            // SmoothFollow(wandPos, wandRot);
        // }
        
    }

       private void StartGrab(Vector3 wandPos, Quaternion wandRot)
   {
       isDragging = true;
       isGrabbed = true;

       // Calculate initial offset
       // grabOffset = wandRot * (wandPos - transform.position);
       // grabRotationOffset = Quaternion.Inverse(wandRot) * transform.rotation;

       rb.isKinematic = true;
       rb.linearVelocity = Vector3.zero;
       rb.angularVelocity = Vector3.zero;
       lastPosition = transform.position;
       transform.position = wandPos;
   }

       private void ReleaseGrenade(Vector3 currentWandPos)
       {
           isDragging = false;
           isGrabbed = false;
           rb.isKinematic = false;

           // Calculate throw direction based on recent movement
           // Vector3 throwDir = (currentWandPos - (currentWandPos - grabOffset)).normalized;
           Vector3 throwDir = (currentWandPos - currentWandPos).normalized;
           // rb.AddForce(throwDir * throwForce, ForceMode.Impulse);
           rb.AddForce(velocity, ForceMode.Impulse);
       }

   /*
       private void SmoothFollow(Vector3 targetWandPos, Quaternion targetWandRot)
       {
           // Calculate target position with offset
           // Vector3 targetPos = targetWandPos - (targetWandRot * grabOffset);
           Vector3 targetPos = targetWandPos;
           Quaternion targetRot = targetWandRot;

           // Smooth interpolation
           transform.position = Vector3.Lerp(transform.position, targetPos, positionLerpSpeed * Time.deltaTime);
           transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationLerpSpeed * Time.deltaTime);
       }

       private bool IsThisGrenadeBeingPointedAt(Vector3 wandPos, Quaternion wandRot)
       {
           RaycastHit hit;
           Vector3 rayDirection = wandRot * Vector3.forward;

           if (Physics.Raycast(wandPos, rayDirection, out hit, grabDistance))
           // if (Physics.SphereCast(wandPos, 50f, rayDirection, out hit, grabDistance))
           // if (Physics.SphereCast(wandPos, 50f, rayDirection, out hit, grabDistance))
           {
               return hit.collider.gameObject == this.gameObject;
           }
           return false;
       }
       */
       #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasFlashed && collision.gameObject.CompareTag("Table"))
        {
            StartCoroutine(FlashEffect());
            hasFlashed = true;
            Destroy(gameObject, flashDuration + 1f); // Destroy after flash duration + 1 second
        }
    }

    private System.Collections.IEnumerator FlashEffect()
    {
        bangSound.Play();
        whiteNoise.Play();
        whiteScreen.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 1f;
            whiteScreen.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            yield return null;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1Wand"))
        {
            transform.position = other.transform.position;
        }
    }
    */
}
