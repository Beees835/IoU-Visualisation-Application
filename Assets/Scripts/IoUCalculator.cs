using System.Collections.Generic;
using UnityEngine;

public class IoUCalculator : MonoBehaviour
{
    public static GameObject intersectionObject;
    private static string defaultInfo = "Not enough shapes to calculate Intersection over Union";
    public static string IoUInfo = defaultInfo;

    // Method to calculate and display IoU between two shapes from ShapeManager
    public static void CalculateIoUForShapes()
    {
        List<Shape> allShapes = ShapeManager.AllShapes;

        if (allShapes.Count < CanvasState.MAX_SHAPE_COUNT)
        {
            Debug.LogWarning("Not enough shapes to calculate IoU.");
            return;
        }

        // Assuming we calculate IoU between the first two shapes
        Shape shape1 = allShapes[0];
        Shape shape2 = allShapes[1];

        Vector2[] poly1 = ConvertShapePointsToVector2Array(shape1.Points);
        Vector2[] poly2 = ConvertShapePointsToVector2Array(shape2.Points);

        Vector2[] intersectionPoints = GetIntersectionPoints(poly1, poly2);

        float area1 = CalculatePolygonArea(poly1);
        float area2 = CalculatePolygonArea(poly2);

        float[] iouValues = CalculateIoU(area1, area2, intersectionPoints);

        // Highlight the intersection after the calculation
        HighlightIntersection(intersectionPoints);
        CanvasState.Instance.shapeCount += 1; // Add a shape for the intersection
        Debug.Log("IoU between shape 1 and shape 2: " + iouValues[2]);

        Debug.Log("IoU between shape 1 and shape 2: " + iouValues[2]);
        string msg = "Shape 1 Area:  {0} \nShape 2 Area: {1} \n Area of Union: {2} \n Area of Intersection: {3} \n IoU: {4}";
        IoUInfo = string.Format(msg, area1, area2, iouValues[0], iouValues[1], iouValues[2]);
        IouCalcTextDisplay.Instance.ShowMessage(IoUInfo);
    }

    public static void ResetInfo()
    {
        IoUInfo = defaultInfo;
        IouCalcTextDisplay.Instance.ResetText();
    }

    private static Vector2[] ConvertShapePointsToVector2Array(List<Vector3> points)
    {
        Vector2[] result = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            result[i] = new Vector2(points[i].x, points[i].y);
        }
        return result;
    }

    // Method to calculate and display IoU between two polygons
    public static float[] CalculateIoU(float area1, float area2, Vector2[] intersectionPoints)
    {
        float[] iouValues = new float[3];
        if (intersectionPoints.Length < 3)
        {
            iouValues[0] = area1 + area2;
            iouValues[1] = 0f;
            iouValues[2] = 0f;
            return iouValues;
        }

        float intersectionArea = CalculatePolygonArea(intersectionPoints);
        float unionArea = area1 + area2 - intersectionArea;

        iouValues[0] = unionArea;
        iouValues[1] = intersectionArea;
        iouValues[2] = intersectionArea / unionArea;
        return iouValues;
    }

    // Method to get intersection points between two polygons
    public static Vector2[] GetIntersectionPoints(Vector2[] poly1, Vector2[] poly2)
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
            return new List<Vector2>().ToArray(); // No valid intersection
        }

        return SortPointsClockwise(intersectionPoints).ToArray();
    }

    // Method to highlight the intersection area
    private static void HighlightIntersection(Vector2[] intersectionPoints)
    {
        if (intersectionPoints.Length < 3)
            return;

        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < intersectionPoints.Length; i++)
        {
            // Use a slight negative z-value to ensure visibility
            vertices.Add(new Vector3(intersectionPoints[i].x, intersectionPoints[i].y, -0.5f));
        }

        // Log the intersection points count
        Debug.Log("Highlighting intersection with " + intersectionPoints.Length + " points.");

        GameObject newIntersectionObject = new GameObject("Intersection");
        MeshFilter meshFilter = newIntersectionObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = newIntersectionObject.AddComponent<MeshRenderer>();

        // Ensure a visible shader is used
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = Color.yellow; // Highlight with yellow color

        Mesh mesh = new Mesh();

        int[] triangles = new int[(intersectionPoints.Length - 2) * 3];
        for (int i = 0; i < intersectionPoints.Length - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        if (intersectionObject != null)
        {
            Destroy(intersectionObject);
        }
        // Track this intersection object for future removal if needed
        intersectionObject = newIntersectionObject;
    }


    // Supporting method to check if a point is inside a polygon
    public static bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
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
    public static float CalculatePolygonArea(Vector2[] polygon)
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
    private static List<Vector2> SortPointsClockwise(List<Vector2> points)
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
    public static Vector2? GetIntersectionPoint(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
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

    public static void Reset()
    {
        Destroy(intersectionObject);
        intersectionObject = null;
        ResetInfo();
    }
}