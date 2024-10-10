using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoordinateDisplay : MonoBehaviour
{
    public static bool showCoordinates = true; // Static flag to toggle coordinates display for all instances
    public Vector3 offset = new Vector3(0, 2, 0); 
    private TextMeshProUGUI textMesh; 

    // Static list to hold all instances
    private static List<CoordinateDisplay> allInstances = new List<CoordinateDisplay>();

    void Start()
    {
        // Register this instance
        allInstances.Add(this);

        Canvas canvas = GetComponentInChildren<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas component is missing. Make sure there is a child Canvas attached.");
            return;
        }

        // Set the render camera for the canvas to the Main Camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            canvas.worldCamera = mainCamera; // Assign the Main Camera to the Canvas
        }
        else
        {
            Debug.LogError("Main Camera is missing. Make sure there is a Camera tagged as MainCamera in the scene.");
        }

        textMesh = canvas.GetComponentInChildren<TextMeshProUGUI>();

        if (textMesh == null)
        {
            Debug.LogError("TextMeshProUGUI component is missing. Make sure the Canvas has a Text (TMP) child.");
            return;
        }

        textMesh.color = Color.white; 
        textMesh.text = ""; 

        canvas.transform.localPosition = offset;

        UpdateCoordinates(); 
    }

    void OnDestroy()
    {
        // Unregister this instance
        allInstances.Remove(this);
    }

    void Update()
    {
        if (showCoordinates)
        {
            UpdateCoordinates();
        }
        else
        {
            textMesh.text = ""; 
        }
    }

    void UpdateCoordinates()
    {
        Vector3 position = transform.position;
        textMesh.text = $"X: {position.x:F2}, Y: {position.y:F2}"; // Display X and Y with 2 decimal precision
    }

    // Static method to toggle showCoordinates for all instances
    public static void ToggleAllCoordinates()
    {
        showCoordinates = !showCoordinates;

        // Update each instance to reflect the change
        foreach (CoordinateDisplay instance in allInstances)
        {
            if (instance != null && instance.textMesh != null)
            {
                if (showCoordinates)
                {
                    instance.UpdateCoordinates();
                }
                else
                {
                    instance.textMesh.text = "";
                }
            }
        }
    }
}
