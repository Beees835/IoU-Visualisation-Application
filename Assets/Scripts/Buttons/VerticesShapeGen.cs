using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VerticesShapeGen : MonoBehaviour
{
    [SerializeField] private TMP_InputField _verticesInput;
    [SerializeField] private Button _generateButton;


    private const int MIN_VERTICES = 3; // Minimum vertices allowed
    private const int MAX_VERTICES = 10; // Maximum vertices allowed

    private void Start()
    {
        // Add listener to the button to call GenerateShapeFromInput on click
        _generateButton.onClick.AddListener(GenerateShapeFromInput);
    }

    void GenerateShapeFromInput()
    {
        // Get and validate user input
        int vertexCount;
        bool isValid = int.TryParse(_verticesInput.text, out vertexCount);

        if (!isValid || vertexCount < MIN_VERTICES || vertexCount > MAX_VERTICES)
        {
            NotificationManager.Instance.ShowMessage("Invalid input: Please enter a number between " + MIN_VERTICES + " and " + MAX_VERTICES);
            Debug.LogWarning("Invalid input: Please enter a number between " + MIN_VERTICES + " and " + MAX_VERTICES);
            // Clear input field
            _verticesInput.text = "";
            return;
        }

        // Generate shape based on user input
        ShapeGenerator.GenerateShape(vertexCount);

        // Clear input field
        _verticesInput.text = "";

        // Allows the colour tint to show on the btn when you hover over it, even after clicking 
        _generateButton.interactable = true;
    }
}