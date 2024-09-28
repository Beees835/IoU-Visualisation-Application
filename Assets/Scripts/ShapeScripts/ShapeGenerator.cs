using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShapeGenerator : MonoBehaviour
{
    public GameObject PrefabShape1;
    public Material lineMaterial;
    public TMP_InputField verticesInput;
    public Button generateButton;
    private const int MaxVertices = 10;

    private void Start()
    {
        generateButton.onClick.RemoveAllListeners(); // Remove any existing listeners to prevent duplicates
        generateButton.onClick.AddListener(GenerateShapeFromInput);
        UpdateUIState();
    }
    
    public void UpdateUIState()
    {
        bool canCreateShape = ShapeManager.Instance.CanCreateShape();
        // Disable both the button and input field if the maximum number of shapes has been reached
        generateButton.interactable = canCreateShape;
        verticesInput.interactable = canCreateShape;
    }

    public void GenerateTriangle()
    {
        GenerateShape(3, "Triangle");
    }

    public void GenerateSquare()
    {
        GenerateShape(4, "Square");
    }

    public void GeneratePentagon()
    {
        GenerateShape(5, "Pentagon");
    }

    public void GenerateHexagon()
    {
        GenerateShape(6, "Hexagon");
    }

    public void GenerateShapeFromInput()
    {
        if (!ShapeManager.Instance.CanCreateShape())
        {
            Debug.LogWarning("Maximum number of shapes reached. Cannot generate more shapes.");
            UpdateUIState();
            return;
        }

        int vertexCount;
        bool isValid = int.TryParse(verticesInput.text, out vertexCount);

        if (!isValid || vertexCount < 3 || vertexCount > MaxVertices)
        {
            Debug.LogWarning("Invalid input: Please enter a number between 3 and " + MaxVertices);
            return;
        }

        GenerateShape(vertexCount, "InputShape");
    }

    private void GenerateShape(int vertexCount, string shapeName)
    {
        if (!ShapeManager.Instance.CanCreateShape())
        {
            Debug.LogWarning("Maximum number of shapes reached. Cannot generate more shapes.");
            UpdateUIState();
            return;
        }

        List<Vector2> vertices2D = GenerateConvexPolygon(vertexCount);

        List<Vector3> vertices3D = new List<Vector3>();
        foreach (var v in vertices2D)
        {
            vertices3D.Add(new Vector3(v.x, v.y, -1f));
        }

        Shape newShape = new Shape
        {
            IsClosed = true
        };

        GameObject shapeObject = new GameObject(shapeName + "Object");

        foreach (var point in vertices3D)
        {
            GameObject newPrefab = Instantiate(PrefabShape1, point, Quaternion.identity);
            newPrefab.transform.SetParent(shapeObject.transform);

            newShape.AddPoint(point, newPrefab);
        }

        AddLineRenderer(shapeObject, vertices3D);

        ShapeManager.Instance.AddShape(newShape);

        CenterShapeOnCanvas(shapeObject);

        Debug.Log($"Created {shapeName} with {vertexCount} vertices");

        UpdateUIState();

        // Highlight intersections with other shapes
        HighlightIntersectionsWithExistingShapes(newShape);
    }

    private List<Vector2> GenerateConvexPolygon(int vertexCount)
    {
        List<Vector2> points = new List<Vector2>();
        int extraPoints = 5;

        float canvasWidth = 10.0f;
        float canvasHeight = 10.0f;

        float marginX = canvasWidth * 0.08f;
        float marginY = canvasHeight * 0.08f;

        float xMin = -canvasWidth / 2 + marginX;
        float xMax = canvasWidth / 2 - marginX;
        float yMin = -canvasHeight / 2 + marginY;
        float yMax = canvasHeight / 2 - marginY;

        for (int i = 0; i < vertexCount + extraPoints; i++)
        {
            points.Add(new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax)));
        }

        List<Vector2> convexHull = ConvexHullManager.ConvexHull(points);

        if (convexHull.Count < vertexCount)
        {
            Debug.LogWarning("Regenerating points to match the desired vertex count.");
            return GenerateConvexPolygon(vertexCount);
        }

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
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = vertices.Count + 1;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.loop = true;

        for (int i = 0; i < vertices.Count; i++)
        {
            lineRenderer.SetPosition(i, vertices[i]);
        }

        lineRenderer.SetPosition(vertices.Count, vertices[0]);
    }

    private void Update()
    {
        if (ShapeManager.Instance.IsDrawingAllowed)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = -1f;

                // Ensure a point is only added after the cooldown period
                if (Time.time - ShapeManager.Instance.lastShapeEndTime > ShapeManager.Instance.newShapeCooldown)
                {
                    GameObject newPrefab = Instantiate(PrefabShape1, mousePos, Quaternion.identity);
                    ShapeManager.Instance.AddPointToCurrentShape(mousePos, newPrefab);

                    UpdateUIState();
                }
            }
        }
    }
    
    private static Vector2[] ConvertShapePointsToVector(List<Vector3> points)
    {
        Vector2[] result = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            result[i] = new Vector2(points[i].x, points[i].y);
        }
        return result;
    }
    
    private void HighlightIntersectionsWithExistingShapes(Shape newShape)
    {
        List<Shape> allShapes = ShapeManager.Instance.AllShapes;

        foreach (var existingShape in allShapes)
        {
            if (existingShape == newShape) continue;

            Vector2[] newShapePoints = ConvertShapePointsToVector(newShape.Points);
            Vector2[] existingShapePoints = ConvertShapePointsToVector(existingShape.Points);

            Vector2[] intersectionPoints = IoUManager.GetIntersectionPoints(newShapePoints, existingShapePoints);

            if (intersectionPoints.Length >= 3)
            {
                IoUManager.HighlightIntersection(intersectionPoints);
            }
        }
    }
}
