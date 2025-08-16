using UnityEngine;
using UnityEngine.UI;
using TiltFive;
using JetBrains.Annotations;

public class WandFlashbang : MonoBehaviour
{
    public Image whiteScreen;
    public AudioSource bangSound, whiteNoise;
    public float flashDuration = 2f;
    public float throwForce = 10f;

    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 dragStartPos;
    private bool hasFlashed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        Debug.Log("Wand tracked? " + TiltFive.Wand.IsTracked());
    }

    void Update()
    {
        // Use primary (right-hand) wand by default
        if (!TiltFive.Wand.IsTracked()) return;

        Vector3 wandPos = TiltFive.Wand.GetPosition(ControllerIndex.Right);
        
        float trigger = TiltFive.Input.GetTrigger();

        if (trigger > 0.8f)
        {
            // Begin dragging
            // isDragging = true;
            // dragStartPos = wandPos;
            rb.isKinematic = true;
            transform.position = wandPos;
        }
        else if (trigger < 0.2f)
        {
            rb.isKinematic = false;
        }
        // else if (trigger < 0.2f && isDragging)
        // {
        //     // Release and throw
        //     Vector3 throwDir = (wandPos - dragStartPos).normalized;
        //     rb.isKinematic = false;
        //     rb.AddForce(throwDir * throwForce, ForceMode.Impulse);
        //     isDragging = false;
        // }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasFlashed && collision.gameObject.CompareTag("Table"))
        {
            StartCoroutine(FlashEffect());
            hasFlashed = true;
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
            whiteScreen.color = new Color(1,1,1, Mathf.Lerp(1,0,t));
            yield return null;
        }
    }
}
