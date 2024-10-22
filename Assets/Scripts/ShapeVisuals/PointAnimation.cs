using System.Collections;
using UnityEngine;

/// <summary>
/// Class managing the animation of points
/// </summary>
public class PointAnimation : MonoBehaviour
{
    public float duration = 0.5f;
    private Vector3 targetScale;
    private Vector3 initialScale = Vector3.zero;
    private Vector3 originalScale;

    private Coroutine scaleCoroutine;
    private bool shouldScaleDownAfterScaleUp = false;
    private bool noWait = false;

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    /// <summary>
    /// Scale a point into existence
    /// </summary>
    public void Scale()
    {
        noWait = false;
        transform.localScale = initialScale;
        scaleCoroutine = StartCoroutine(ScaleUp());
    }

    // Destory a point
    public void Close()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
            scaleCoroutine = null;
        }

        scaleCoroutine = StartCoroutine(ScaleDown());
    }

    /// <summary>
    /// Display a point without animation
    /// </summary>
    public void Instant()
    {
        noWait = true;

        if (scaleCoroutine == null)
        {
            scaleCoroutine = StartCoroutine(ScaleUp());
        }
    }

    // Scale a point back down after scaling up
    public void QuickLife()
    {
        shouldScaleDownAfterScaleUp = true;

        if (scaleCoroutine == null)
        {
            scaleCoroutine = StartCoroutine(ScaleUp());
        }
    }

    /// <summary>
    /// Actual scale animation
    /// </summary>
    /// <returns>Animation enumerator</returns>
    IEnumerator ScaleUp()
    {
        if (!noWait)
        {
            float time = 0f;
            while (time < duration)
            {
                // Smoothly interpolate the scale from zero to the original scale
                transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
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

    /// <summary>
    /// Actual scale down animation
    /// </summary>
    /// <returns>Animation enumerator</returns>
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
