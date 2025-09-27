using UnityEngine;
using System.Collections;

public class HandFlash : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    private Color originalColor;
    private Coroutine flashRoutine;

    void Awake()
    {
        // rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    public void TriggerFlash(Color targetColor, float duration)
    {
        // Stop old flash if still running
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine(targetColor, duration));
    }

    private IEnumerator FlashRoutine(Color targetColor, float duration)
    {
        if (rend == null) yield break;

        rend.material.color = targetColor;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            rend.material.color = Color.Lerp(targetColor, originalColor, t / duration);
            yield return null;
        }

        rend.material.color = originalColor;
        flashRoutine = null;
    }

    public void SetOriginalColor(Color c)
    {
        originalColor = c;
        if (rend != null) rend.material.color = c;
    }
}
