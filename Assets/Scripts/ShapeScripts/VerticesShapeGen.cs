using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class VerticesShapeGen : MonoBehaviour
{
    public GameObject PrefabShape1; 
    public Material lineMaterial; 
    public TMP_InputField verticesInput; 
    public Button generateButton;

    private GameObject currentShapeObject; // Reference to the currently generated shape
    private List<GameObject> allShapes = new List<GameObject>(); // List to keep track of shapes
    private const int MaxVertices = 10; // Maximum vertices allowed

    private void Start()
    {
        // Add listener to the button to call GenerateShapeFromInput on click
        generateButton.onClick.AddListener(GenerateShapeFromInput);
    }

    public void GenerateShapeFromInput()
    {
        // Get and validate user input
        int vertexCount;
        bool isValid = int.TryParse(verticesInput.text, out vertexCount);

        if (!isValid || vertexCount < 3 || vertexCount > MaxVertices)
        {
            Debug.LogWarning("Invalid input: Please enter a number between 3 and " + MaxVertices);
            return;
        }

        // Clear previous shape if it exists
        if (currentShapeObject != null)
        {
            Destroy(currentShapeObject); // Destroy the existing shape GameObject
            allShapes.Clear(); // Clear the shapes list in case it's being used elsewhere
            ShapeManager.Instance.AllShapes.Clear(); // Clear the ShapeManager's shape list
        }

        // Generate shape based on user input
        GenerateShape(vertexCount, "InputShape");
    }

    private void GenerateShape(int vertexCount, string shapeName)
    {
        // Generate convex polygon vertices using the ConvexHullManager
        List<Vector3> vertices2D = GenerateConvexPolygon(vertexCount);

        // Convert to Vector3 for shape instantiation
        List<Vector3> vertices3D = new List<Vector3>();
        foreach (var v in vertices2D)
        {
            vertices3D.Add(new Vector3(v.x, v.y, -1f));
        }

        // Create a new Shape and set its properties
        Shape newShape = new Shape
        {
            IsClosed = true
        };

        // Create a new GameObject for this shape
        GameObject shapeObject = new GameObject(shapeName + "Object");
        currentShapeObject = shapeObject; // Store the reference to the current shape object

        // Add points and instantiate prefabs
        foreach (var point in vertices3D)
        {
            GameObject newPrefab = Instantiate(PrefabShape1, point, Quaternion.identity);
            newPrefab.transform.SetParent(shapeObject.transform); // Parent to the shape object

            newShape.AddPoint(point, newPrefab); // Add point and associated prefab to shape
        }

        // Add the LineRenderer to the shape object to draw the edges
        AddLineRenderer(shapeObject, vertices3D);

        // Add the new shape to ShapeManager
        ShapeManager.Instance.AllShapes.Add(newShape);

        // Center the shape on the canvas
        CenterShapeOnCanvas(shapeObject);

        Debug.Log($"Created {shapeName} with {vertexCount} vertices");

        // Add the generated shape GameObject to the list of all shapes
        allShapes.Add(shapeObject);
    }

    private List<Vector3> GenerateConvexPolygon(int vertexCount)
    {
        List<Vector3> points = new List<Vector3>();
        int extraPoints = 5; // Adding extra points to ensure sufficient points for a good hull

        // Define the canvas size 
        float canvasWidth = 10.0f;
        float canvasHeight = 10.0f;

        // Calculate 8% margin 
        float marginX = canvasWidth * 0.08f; 
        float marginY = canvasHeight * 0.08f; 

        // Calculate the canvas limits with margin included
        float xMin = -canvasWidth / 2 + marginX; 
        float xMax = canvasWidth / 2 - marginX;  
        float yMin = -canvasHeight / 2 + marginY; 
        float yMax = canvasHeight / 2 - marginY;  

        // Generate random points within the canvas bounds
        for (int i = 0; i < vertexCount + extraPoints; i++)
        {
            points.Add(new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax)));
        }

        // Use ConvexHullManager to generate the convex hull
        List<Vector3> convexHull = ConvexHullManager.ConvexHull(points);

        // If the convex hull has fewer points than desired, regenerate
        if (convexHull.Count < vertexCount)
        {
            Debug.LogWarning("Regenerating points to match the desired vertex count.");
            return GenerateConvexPolygon(vertexCount);
        }

        // Return only the desired number of points
        return convexHull.GetRange(0, Mathf.Min(vertexCount, convexHull.Count));
    }

    private void CenterShapeOnCanvas(GameObject shapeObject)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            shapeObject.transform.position = canvasRect.transform.position;
        }
    }

    private void AddLineRenderer(GameObject shapeObject, List<Vector3> vertices)
    {
        LineRenderer lineRenderer = shapeObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial; // Assign the material from the Inspector
        lineRenderer.positionCount = vertices.Count + 1; // One extra to close the loop
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.loop = true; // To close the shape

        // Set positions for the LineRenderer
        for (int i = 0; i < vertices.Count; i++)
        {
            lineRenderer.SetPosition(i, vertices[i]);
        }

        // Close the loop
        lineRenderer.SetPosition(vertices.Count, vertices[0]);
    }
}
