using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    // Singleton instance
    public static NotificationManager Instance { get; private set; }

    private TextMeshProUGUI _notificationText;
    private Coroutine currentCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _notificationText = GetComponent<TextMeshProUGUI>();
            if (_notificationText == null)
            {
                Debug.LogError("NotificationManager requires a TextMeshProUGUI component on the same GameObject.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMessage(string message)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Start the coroutine to show the message after a slight delay and then clear it after 2 seconds
        currentCoroutine = StartCoroutine(ShowMessageWithDelay(message, 0.1f, 2f));
    }

    private IEnumerator ShowMessageWithDelay(string message, float initialDelay, float displayDuration)
    {
        // Initial invisibility for 0.1 seconds
        _notificationText.text = "";
        yield return new WaitForSeconds(initialDelay);

        // Display the message
        _notificationText.text = message;

        // Clear the message after the display duration
        yield return new WaitForSeconds(displayDuration);
        _notificationText.text = "";
        currentCoroutine = null;
    }

    public void ClearMessage()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        _notificationText.text = "";
    }
}
