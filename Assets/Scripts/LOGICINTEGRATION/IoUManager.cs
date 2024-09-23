using System.Collections.Generic;
using UnityEngine;

public class IoUManager : MonoBehaviour
{
    private List<GameObject> intersectionObjects = new List<GameObject>();

    private void Update()
    {
        // Check if we should start calculating IoU
        if (CanvasState.Instance.beginCalculatingIoUStatus)
        {
            CalculateIoUForShapes();
            CanvasState.Instance.beginCalculatingIoUStatus = false; // Reset the flag
        }
    }

    // Method to calculate and display IoU between two shapes from ShapeManager
    public void CalculateIoUForShapes()
    {
        List<Shape> allShapes = ShapeManager.Instance.AllShapes;

        if (allShapes.Count < 2)
        {
            Debug.LogWarning("Not enough shapes to calculate IoU.");
            return;
        }

        // Assuming we calculate IoU between the first two shapes
        Shape shape1 = allShapes[0];
        Shape shape2 = allShapes[1];

        Vector2[] poly1 = ConvertShapePointsToVector2Array(shape1.Points);
        Vector2[] poly2 = ConvertShapePointsToVector2Array(shape2.Points);

        float iou = CalculateIoU(poly1, poly2);

        Debug.Log("IoU between shape 1 and shape 2: " + iou);
    }

    private Vector2[] ConvertShapePointsToVector2Array(List<Vector3> points)
    {
        Vector2[] result = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            result[i] = new Vector2(points[i].x, points[i].y);
        }
        return result;
    }
    
    // Method to calculate and display IoU between two polygons
    public float CalculateIoU(Vector2[] poly1, Vector2[] poly2)
    {
        float area1 = CalculatePolygonArea(poly1);
        float area2 = CalculatePolygonArea(poly2);

        List<Vector2> intersectionPoints = GetIntersectionPoints(poly1, poly2);

        if (intersectionPoints.Count < 3)
        {
            return 0f; // No intersection
        }

        float intersectionArea = CalculatePolygonArea(intersectionPoints.ToArray());
        float unionArea = area1 + area2 - intersectionArea;

        // Highlight the intersection after the calculation
        HighlightIntersection(intersectionPoints);

        return intersectionArea / unionArea;
    }

    // Method to get intersection points between two polygons
    private List<Vector2> GetIntersectionPoints(Vector2[] poly1, Vector2[] poly2)
    {
        List<Vector2> intersectionPoints = new List<Vector2>();

        // Check if points from poly1 are inside poly2
        foreach (var point in poly1)
        {
            if (IsPointInPolygon(point, poly2))
            {
                intersectionPoints.Add(point);
            }
        }

        // Check if points from poly2 are inside poly1
        foreach (var point in poly2)
        {
            if (IsPointInPolygon(point, poly1))
            {
                intersectionPoints.Add(point);
            }
        }

        // Find intersection points between the edges of poly1 and poly2
        for (int i = 0; i < poly1.Length; i++)
        {
            Vector2 a1 = poly1[i];
            Vector2 a2 = poly1[(i + 1) % poly1.Length];

            for (int j = 0; j < poly2.Length; j++)
            {
                Vector2 b1 = poly2[j];
                Vector2 b2 = poly2[(j + 1) % poly2.Length];

                Vector2? intersection = GetIntersectionPoint(a1, a2, b1, b2);
                if (intersection.HasValue)
                {
                    intersectionPoints.Add(intersection.Value);
                }
            }
        }

        if (intersectionPoints.Count < 3)
        {
            return new List<Vector2>(); // No valid intersection
        }

        return SortPointsClockwise(intersectionPoints);
    }

    // Method to highlight the intersection area
    private void HighlightIntersection(List<Vector2> intersectionPoints)
    {
        if (intersectionPoints.Count < 3)
            return;

        // Log the intersection points count
        Debug.Log("Highlighting intersection with " + intersectionPoints.Count + " points.");

        GameObject intersectionObject = new GameObject("Intersection");
        MeshFilter meshFilter = intersectionObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = intersectionObject.AddComponent<MeshRenderer>();

        // Ensure a visible shader is used
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = Color.yellow; // Highlight with yellow color

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[intersectionPoints.Count];
        for (int i = 0; i < intersectionPoints.Count; i++)
        {
            vertices[i] = new Vector3(intersectionPoints[i].x, intersectionPoints[i].y, -0.5f); // Use a slight negative z-value to ensure visibility
        }

        int[] triangles = new int[(intersectionPoints.Count - 2) * 3];
        for (int i = 0; i < intersectionPoints.Count - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        intersectionObjects.Add(intersectionObject);  // Track this intersection object for future removal if needed
    }


    // Supporting method to check if a point is inside a polygon
    private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        bool isInside = false;
        int j = polygon.Length - 1;

        for (int i = 0; i < polygon.Length; i++)
        {
            if (polygon[i].y < point.y && polygon[j].y >= point.y || polygon[j].y < point.y && polygon[i].y >= point.y)
            {
                if (polygon[i].x + (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < point.x)
                {
                    isInside = !isInside;
                }
            }
            j = i;
        }

        return isInside;
    }

    // Method to calculate the polygon area
    private float CalculatePolygonArea(Vector2[] polygon)
    {
        float area = 0f;
        int j = polygon.Length - 1;

        for (int i = 0; i < polygon.Length; i++)
        {
            area += (polygon[j].x + polygon[i].x) * (polygon[j].y - polygon[i].y);
            j = i;
        }

        return Mathf.Abs(area / 2f);
    }

    // Sort the intersection points in a clockwise order
    private List<Vector2> SortPointsClockwise(List<Vector2> points)
    {
        Vector2 center = Vector2.zero;
        foreach (var point in points)
        {
            center += point;
        }
        center /= points.Count;

        points.Sort((a, b) => Mathf.Atan2(a.y - center.y, a.x - center.x).CompareTo(Mathf.Atan2(b.y - center.y, b.x - center.x)));

        return points;
    }

    // Get the intersection point between two line segments
    private Vector2? GetIntersectionPoint(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        Vector2 r = a2 - a1;
        Vector2 s = b2 - b1;

        float rxs = r.x * s.y - r.y * s.x;
        float t = ((b1.x - a1.x) * s.y - (b1.y - a1.y) * s.x) / rxs;
        float u = ((b1.x - a1.x) * r.y - (b1.y - a1.y) * r.x) / rxs;

        if (Mathf.Abs(rxs) < Mathf.Epsilon)
        {
            return null;
        }

        if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
        {
            return a1 + t * r;
        }

        return null;
    }
}