using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PolygonDrawer : MonoBehaviour
{
    public Button finishButton;
    public Button calculateIoUButton;
    public Button resetButton;
    public Text ioUText;
    public GameObject dotPrefab;
    private List<Vector2> currentPolygonPoints = new List<Vector2>();
    private List<GameObject> polygons = new List<GameObject>();
    private List<GameObject> dots = new List<GameObject>();
    private LineRenderer currentLineRenderer;
    private bool isDrawingPolygon = true;

    private List<Vector2[]> allPolygons = new List<Vector2[]>();
    private List<GameObject> intersectionObjects = new List<GameObject>();

    private GameObject selectedPolygon = null;
    private Vector2 offset;

    void Start()
    {
        finishButton.onClick.AddListener(FinishPolygon);
        calculateIoUButton.onClick.AddListener(CalculateAndDisplayIoU);
        resetButton.onClick.AddListener(ResetCanvas);
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (selectedPolygon != null)
        {
            DragPolygon();
            return;
        }

        if (!isDrawingPolygon && Input.GetMouseButtonDown(0))
        {
            isDrawingPolygon = true;
        }

        if (isDrawingPolygon && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject clickedDot = DetectClickedDot(mousePos);
            if (clickedDot != null)
            {
                currentPolygonPoints.Add(clickedDot.transform.position);
                FinishPolygon();
                return;
            }

            currentPolygonPoints.Add(mousePos);
            InstantiateDot(mousePos);
            UpdateLineRenderer();
        }
        else if (!isDrawingPolygon && Input.GetMouseButtonDown(0))
        {
            TrySelectPolygon();
        }

        if (Input.GetMouseButtonUp(0) && selectedPolygon != null)
        {
            selectedPolygon = null;
        }
    }

    GameObject DetectClickedDot(Vector2 mousePos)
    {
        foreach (GameObject dot in dots)
        {
            Collider2D collider = dot.GetComponent<Collider2D>();
            if (collider.OverlapPoint(mousePos))
                return dot;
        }
        return null;
    }

    void InstantiateDot(Vector2 position)
    {
        GameObject dot = Instantiate(dotPrefab, position, Quaternion.identity);
        dot.AddComponent<CircleCollider2D>().isTrigger = true;
        dots.Add(dot);
    }

    void UpdateLineRenderer()
    {
        if (currentLineRenderer == null)
        {
            GameObject lineObject = new GameObject("PolygonLine");
            currentLineRenderer = lineObject.AddComponent<LineRenderer>();
            currentLineRenderer.startWidth = 0.05f;
            currentLineRenderer.endWidth = 0.05f;
            currentLineRenderer.loop = false;
            currentLineRenderer.positionCount = 0;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.positionCount = currentPolygonPoints.Count + 1;
        for (int i = 0; i < currentPolygonPoints.Count; i++)
        {
            currentLineRenderer.SetPosition(i, currentPolygonPoints[i]);
        }
        currentLineRenderer.SetPosition(currentPolygonPoints.Count, mousePos);
    }

    public void FinishPolygon()
    {
        if (currentPolygonPoints.Count > 2)
        {
            CreatePolygon();
            isDrawingPolygon = false;
            ClearDots();
        }
    }

    void CreatePolygon()
    {
        currentPolygonPoints.Add(currentPolygonPoints[0]);

        GameObject polygonObject = new GameObject("Polygon");
        polygons.Add(polygonObject);

        PolygonCollider2D polygonCollider = polygonObject.AddComponent<PolygonCollider2D>();
        polygonCollider.points = currentPolygonPoints.ToArray();

        allPolygons.Add(polygonCollider.points);

        LineRenderer lineRenderer = polygonObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = currentPolygonPoints.Count;
        for (int i = 0; i < currentPolygonPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, currentPolygonPoints[i]);
        }

        Destroy(currentLineRenderer.gameObject);
        currentLineRenderer = null;
        currentPolygonPoints.Clear();
    }

    void ClearDots()
    {
        foreach (GameObject dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
    }

    void CalculateAndDisplayIoU()
    {
        if (allPolygons.Count >= 2)
        {
            // Clear any existing intersection highlights
            ClearIntersectionHighlights();

            // Compute the intersection across all polygons
            List<Vector2> intersectionPoints = new List<Vector2>(allPolygons[0]);

            for (int i = 1; i < allPolygons.Count; i++)
            {
                intersectionPoints = GetIntersectionPoints(intersectionPoints.ToArray(), allPolygons[i]);

                // If at any point the intersection is empty, IoU is zero
                if (intersectionPoints.Count < 3)
                {
                    ioUText.text = "IoU: 0";
                    return;
                }
            }

            // Calculate the intersection area
            float intersectionArea = CalculatePolygonArea(intersectionPoints);

            // Calculate the union area: the sum of individual polygon areas minus the intersection area
            float unionArea = CalculateUnionArea(allPolygons) - (allPolygons.Count - 1) * intersectionArea;

            // Highlight the new intersection area
            HighlightIntersection(intersectionPoints);

            float iou = intersectionArea / unionArea;
            ioUText.text = "IoU: " + iou.ToString("F8");
        }
        else
        {
            ioUText.text = "Please draw at least two polygons.";
        }
    }

    void ClearIntersectionHighlights()
    {
        // Destroy all existing intersection highlight objects
        foreach (GameObject intersectionObject in intersectionObjects)
        {
            Destroy(intersectionObject);
        }
        intersectionObjects.Clear();
    }

    List<Vector2> GetIntersectionPoints(Vector2[] poly1, Vector2[] poly2)
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

    float CalculateUnionArea(List<Vector2[]> polygons)
    {
        float totalArea = 0;
        foreach (var poly in polygons)
        {
            totalArea += CalculatePolygonArea(poly);
        }
        return totalArea;
    }


    void TrySelectPolygon()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (GameObject polygon in polygons)
        {
            PolygonCollider2D collider = polygon.GetComponent<PolygonCollider2D>();
            if (collider.OverlapPoint(mousePos))
            {
                selectedPolygon = polygon;
                offset = (Vector2)polygon.transform.position - mousePos;
                break;
            }
        }
    }

    void DragPolygon()
    {
        if (selectedPolygon != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedPolygon.transform.position = mousePos + offset;

            // Update the collider and line renderer positions
            PolygonCollider2D collider = selectedPolygon.GetComponent<PolygonCollider2D>();
            LineRenderer lineRenderer = selectedPolygon.GetComponent<LineRenderer>();

            Vector2[] newPositions = new Vector2[collider.points.Length];
            for (int i = 0; i < collider.points.Length; i++)
            {
                newPositions[i] = collider.points[i] + (Vector2)selectedPolygon.transform.position;
            }
            collider.points = newPositions;

            for (int i = 0; i < newPositions.Length; i++)
            {
                lineRenderer.SetPosition(i, newPositions[i]);
            }

            // Optional: Recalculate IoU and highlight intersection if needed
            if (allPolygons.Count >= 2)
            {
                CalculateAndDisplayIoU();
            }
        }
    }

    void ResetCanvas()
    {
        foreach (GameObject polygon in polygons)
        {
            Destroy(polygon);
        }
        polygons.Clear();

        ClearDots();

        foreach (GameObject intersectionObject in intersectionObjects)
        {
            Destroy(intersectionObject);
        }
        intersectionObjects.Clear();

        allPolygons.Clear();

        ioUText.text = "";

        isDrawingPolygon = true;
        currentLineRenderer = null;
        currentPolygonPoints.Clear();
    }

    float CalculateIoU(Vector2[] poly1, Vector2[] poly2)
    {
        float area1 = CalculatePolygonArea(poly1);
        float area2 = CalculatePolygonArea(poly2);

        List<Vector2> intersectionPoints = new List<Vector2>();

        foreach (var point in poly1)
        {
            if (IsPointInPolygon(point, poly2))
            {
                intersectionPoints.Add(point);
            }
        }

        foreach (var point in poly2)
        {
            if (IsPointInPolygon(point, poly1))
            {
                intersectionPoints.Add(point);
            }
        }

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
            return 0;
        }

        intersectionPoints = SortPointsClockwise(intersectionPoints);
        float intersectionArea = CalculatePolygonArea(intersectionPoints);

        float unionArea = area1 + area2 - intersectionArea;

        HighlightIntersection(intersectionPoints);

        return intersectionArea / unionArea;
    }

    void HighlightIntersection(List<Vector2> intersectionPoints)
    {
        if (intersectionPoints.Count < 3)
            return;

        GameObject intersectionObject = new GameObject("Intersection");
        MeshFilter meshFilter = intersectionObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = intersectionObject.AddComponent<MeshRenderer>();

        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material.color = Color.yellow;

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[intersectionPoints.Count];
        for (int i = 0; i < intersectionPoints.Count; i++)
        {
            vertices[i] = new Vector3(intersectionPoints[i].x, intersectionPoints[i].y, 0);
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

        intersectionObjects.Add(intersectionObject);
    }

    Vector2? GetIntersectionPoint(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
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

    float CalculatePolygonArea(Vector2[] polygon)
    {
        float area = 0;
        int j = polygon.Length - 1;

        for (int i = 0; i < polygon.Length; i++)
        {
            area += (polygon[j].x + polygon[i].x) * (polygon[j].y - polygon[i].y);
            j = i;
        }

        return Mathf.Abs(area / 2);
    }

    float CalculatePolygonArea(List<Vector2> polygon)
    {
        float area = 0;
        int j = polygon.Count - 1;

        for (int i = 0; i < polygon.Count; i++)
        {
            area += (polygon[j].x + polygon[i].x) * (polygon[j].y - polygon[i].y);
            j = i;
        }

        return Mathf.Abs(area / 2);
    }

    List<Vector2> SortPointsClockwise(List<Vector2> points)
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

    bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
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
}
