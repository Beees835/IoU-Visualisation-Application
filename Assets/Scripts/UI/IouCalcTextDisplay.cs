using TMPro;
using UnityEngine;

public class IouCalcTextDisplay : MonoBehaviour
{
    // Singleton instance
    public static IouCalcTextDisplay Instance { get; private set; }

    private TextMeshProUGUI _displayText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _displayText = GetComponent<TextMeshProUGUI>();
            _displayText.color = Color.black;
            if (_displayText == null)
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
        _displayText.text = message;
    }

    // Function to reset the text to an empty string
    public void ResetText()
    {
        _displayText.text = "";
    }
}
