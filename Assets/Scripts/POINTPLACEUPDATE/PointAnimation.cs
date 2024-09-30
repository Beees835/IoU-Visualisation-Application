using System.Collections;
using UnityEngine;

public class PointAnimation : MonoBehaviour
{
    public float duration = 0.5f; 
    private Vector3 targetScale;
    private Vector3 initialScale = Vector3.zero;
    private Vector3 originalScale;

    private Coroutine scaleCoroutine;
    private bool shouldScaleDownAfterScaleUp = false;

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void OnEnable()
    {
        transform.localScale = initialScale;
        scaleCoroutine = StartCoroutine(ScaleUp());
    }

    public void Close()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null;
        }

        scaleCoroutine = StartCoroutine(ScaleDown());
    }

    public void quickLife()
    {
        shouldScaleDownAfterScaleUp = true;

        if (scaleCoroutine == null)
        {
            scaleCoroutine = StartCoroutine(ScaleUp());
        }
    }

    IEnumerator ScaleUp()
    {
        float time = 0f;
        while (time < duration)
        {
            // Smoothly interpolate the scale from zero to the original scale
            transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;

        // Coroutine is complete
        scaleCoroutine = null;

        // Check if we should scale down after scaling up
        if (shouldScaleDownAfterScaleUp)
        {
            shouldScaleDownAfterScaleUp = false; // Reset the flag
            scaleCoroutine = StartCoroutine(ScaleDown());
        }
    }

    IEnumerator ScaleDown()
    {
        float time = 0f;
        Vector3 currentScale = transform.localScale;
        while (time < duration)
        {
            // Smoothly interpolate the scale from current to zero
            transform.localScale = Vector3.Lerp(currentScale, initialScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = initialScale;

        // Coroutine is complete
        scaleCoroutine = null;

        Destroy(gameObject); // Destroy after scaling down
    }
}
