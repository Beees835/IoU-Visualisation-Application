using System.Collections.Generic;
using UnityEngine;

public class RandomShapeGenerator : MonoBehaviour
{
    public Material lineMaterial; // Assign this in the Unity Inspector
    public GameObject vertexPrefab; // Assign the prefab for vertex points in the Unity Inspector
    public List<GameObject> allShapes = new List<GameObject>(); // List to store all shapes created
    
    public void GenerateRandomShape()
    {
        int vertexCount = Random.Range(3, 14); // Randomize vertex count between 3 and 14
        GenerateShape(vertexCount, "RandomShape");
    }

    private void GenerateShape(int vertexCount, string shapeName)
    {
        GameObject shapeObject = new GameObject(shapeName + "Object");
        allShapes.Add(shapeObject); 

        List<Vector2> vertices = new List<Vector2>();
        List<GameObject> vertexObjects = new List<GameObject>();

        // Define the vertices for a regular convex polygon with random variations
        float angleStep = 360f / vertexCount;
        float radius = 1f; // Base radius for regular polygons

        for (int i = 0; i < vertexCount; i++)
        {
            // Introduce random variations in the angle to make the shape more irregular but still convex
            float angleInDegrees = angleStep * i + Random.Range(-angleStep / 8, angleStep / 8);
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
            float randomRadius = radius * Random.Range(0.8f, 1.2f); // Randomize radius slightly

            vertices.Add(new Vector2(Mathf.Cos(angleInRadians) * randomRadius, Mathf.Sin(angleInRadians) * randomRadius));
        }

        // Draw lines and create vertex points
        DrawShapeLines(vertices, shapeObject);
        CreateVertexPoints(vertices, vertexObjects, shapeObject);

        // Center the shape on the canvas
        CenterShapeOnCanvas(shapeObject);

        Debug.Log($"Created {shapeName} with {vertexCount} vertices");
    }

    private void DrawShapeLines(List<Vector2> vertices, GameObject shapeObject)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector2 start = vertices[i];
            Vector2 end = vertices[(i + 1) % vertices.Count]; // Ensure the shape is closed by connecting the last vertex to the first
            DrawLine(start, end, shapeObject);
        }
    }

    private void DrawLine(Vector2 start, Vector2 end, GameObject shapeObject)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(shapeObject.transform);
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(start.x, start.y, -1));
        lineRenderer.SetPosition(1, new Vector3(end.x, end.y, -1));
        lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;
    }

    private void CreateVertexPoints(List<Vector2> vertices, List<GameObject> vertexObjects, GameObject shapeObject)
    {
        foreach (Vector2 vertex in vertices)
        {
            GameObject vertexObj = Instantiate(vertexPrefab, new Vector3(vertex.x, vertex.y, -1), Quaternion.identity);
            vertexObj.transform.SetParent(shapeObject.transform);
            vertexObjects.Add(vertexObj);
        }
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
}
