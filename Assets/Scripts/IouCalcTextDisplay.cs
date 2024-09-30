using UnityEngine;
using TMPro;

public class IouCalcTextDisplay : MonoBehaviour
{
    // Singleton instance
    public static IouCalcTextDisplay Instance { get; private set; }

    private TextMeshProUGUI displayText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            displayText = GetComponent<TextMeshProUGUI>();
            if (displayText == null)
            {
                Debug.LogError("IouCalcTextDisplay requires a TextMeshProUGUI component on the same GameObject.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to display a message immediately
    public void ShowMessage(string message)
    {
        displayText.text = message;
    }

    // Function to reset the text to an empty string
    public void ResetText()
    {
        displayText.text = "";
    }
}
