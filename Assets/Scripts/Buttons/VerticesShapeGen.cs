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
        _generateButton.onClick.AddListener(GenerateShapeFromInput);
    }

    /// <summary>
    /// Generate a shape with n vertices based on input field
    /// </summary>
    void GenerateShapeFromInput()
    {
        // Get and validate user input
        int vertexCount;
        bool isValid = int.TryParse(_verticesInput.text, out vertexCount);

        _verticesInput.text = "";

        if (!isValid || vertexCount < MIN_VERTICES || vertexCount > MAX_VERTICES)
        {
            NotificationManager.Instance.ShowMessage("Invalid input: Please enter a number between " + MIN_VERTICES + " and " + MAX_VERTICES);
            Debug.LogWarning("Invalid input: Please enter a number between " + MIN_VERTICES + " and " + MAX_VERTICES);
            return;
        }

        ShapeGenerator.GenerateShape(vertexCount);
        _generateButton.interactable = true;
    }
}