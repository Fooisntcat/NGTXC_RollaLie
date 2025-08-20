using UnityEngine;
using UnityEngine.UI;
using TimerCountdown;

public class FlashBang : MonoBehaviour
{
    public float throwForce = 10f;
    public Image whiteScreen;
    public AudioSource bangSound;
    public AudioSource whiteNoise;
    public float flashDuration = 2f;

    private Rigidbody rb;
    private Vector3 dragStartPos;
    private bool isDragging = false;
    private bool hasFlashed = false;

    void Start()
    {
        Debug.Log("WhiteScreen reference: " + (whiteScreen != null));
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Don't move until thrown
        Timer timer = FindFirstObjectByType<Timer>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        dragStartPos = Input.mousePosition;
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            Vector3 dragEndPos = Input.mousePosition;
            Vector3 dragVector = dragStartPos - dragEndPos;
            rb.isKinematic = false;
            rb.AddForce(-dragVector.normalized * throwForce, ForceMode.Impulse);
            isDragging = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasFlashed && collision.gameObject.CompareTag("Table"))
        {
            StartCoroutine(FlashEffect());
            Debug.Log("Flashbang triggered!");
            Timer timer = FindFirstObjectByType<Timer>();
            if (timer.timerIsRunning)
            {
                timer.timeRemaining += 4;
            }
            hasFlashed = true;
        }
    }

    private System.Collections.IEnumerator FlashEffect()
    {
        // Play sounds
        bangSound.Play();
        whiteNoise.Play();

        // Fade in white screen instantly
        whiteScreen.color = new Color(1, 1, 1, 1);

        // Wait flash duration
        yield return new WaitForSeconds(flashDuration);

        // Fade out
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / 1f;
            whiteScreen.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            yield return null;
        }
    }
}
