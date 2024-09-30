using UnityEngine;
using TMPro;
using System.Collections;

public class NotificationManager : MonoBehaviour
{
    // Singleton instance
    public static NotificationManager Instance { get; private set; }

    private TextMeshProUGUI notificationText;
    private Coroutine currentCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            notificationText = GetComponent<TextMeshProUGUI>();
            if (notificationText == null)
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

        // Start the coroutine to show the message after a slight delay and then clear it after 3 seconds
        currentCoroutine = StartCoroutine(ShowMessageWithDelay(message, 0.1f, 3f));
    }

    private IEnumerator ShowMessageWithDelay(string message, float initialDelay, float displayDuration)
    {
        // Initial invisibility for 0.1 seconds
        notificationText.text = ""; 
        yield return new WaitForSeconds(initialDelay);
        
        // Display the message
        notificationText.text = message;

        // Clear the message after the display duration
        yield return new WaitForSeconds(displayDuration);
        notificationText.text = "";
        currentCoroutine = null;
    }
}
