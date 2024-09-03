using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class PointPlaceControl : MonoBehaviour
{
    public Camera cam;
    public GameObject prefabShape1;
    public GameObject prefabShape2;
    public Material lineMaterial;
    public float closeThreshold = 0.1f; // Threshold distance to close the shape

    private GameObject prefab;
    private List<Vector3> points = new List<Vector3>();
    private bool lockedFirstShape = false;

    public void PlacePrefabAtMousePosition()
    {
        if (IsPointerOverUIButton())
        {
            return;
        }
        if (CanvasState.Instance.shapeCount == 1)
        {
            prefab = prefabShape1;
        }
        else if (CanvasState.Instance.shapeCount == 2)
        {
            if (!lockedFirstShape)
            {
                lockedFirstShape = true;
                points.Clear();  // Reset the points list when switching to shape2
            }
            prefab = prefabShape2;
        }
        else
        {
            Debug.LogError("Prefab not assigned.");
            return;
        }

        // Get the mouse position in screen space and convert to world space
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mouseScreenPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        Vector3 spawnPosition;

        if (hit.collider != null)
        {
            spawnPosition = hit.point;
        }
        else
        {
            spawnPosition = cam.ScreenToWorldPoint(mouseScreenPosition);
            spawnPosition.z = 0f;
        }

        spawnPosition.z = -1f; //keeps the prefab visible

        // Check if the point is near the first point to close the shape
        if (points.Count > 2 && Vector3.Distance(spawnPosition, points[0]) <= closeThreshold)
        {
            // Close the shape by connecting the last point to the first point
            DrawLine(points[points.Count - 1], points[0]);
            // Instantiate the prefab at the first point position to complete the shape
            Instantiate(prefab, points[0], Quaternion.identity);

            // To next shape
            points.Clear();
            lockedFirstShape = false;
            CanvasState.Instance.shapeCount++;
            return;
        }

        if (IsConvexWithNewPoint(spawnPosition))
        {
            points.Add(spawnPosition);
            Instantiate(prefab, spawnPosition, Quaternion.identity);

            // Draw lines between the stored points
            if (points.Count > 1)
            {
                DrawLine(points[points.Count - 2], points[points.Count - 1]);
            }
        }
        else
        {
            Debug.Log("Point not placed: shape would not be convex.");
        }
    }

    private bool IsConvexWithNewPoint(Vector3 newPoint)
    {
        // If there are fewer than 2 points, any new point will maintain convexity
        if (points.Count < 2)
        {
            return true;
        }

        // Clone the list and add the new point
        // probably not optimal? will have to implement a better way to do this later
        List<Vector3> testPoints = new List<Vector3>(points) { newPoint };

        // Check convexity by examining the cross product signs
        //Checks if it is continously turning in a consistent direciton
        bool isConvex = true;
        int n = testPoints.Count;

        for (int i = 0; i < n; i++)
        {
            Vector3 a = testPoints[i];
            Vector3 b = testPoints[(i + 1) % n];
            Vector3 c = testPoints[(i + 2) % n];

            Vector3 ab = b - a;
            Vector3 bc = c - b;

            float crossProductZ = ab.x * bc.y - ab.y * bc.x;

            if (i == 0)
            {
                isConvex = crossProductZ > 0; 
            }
            else if ((crossProductZ > 0) != isConvex)
            {
                return false; 
            }
        }

        return true;
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        // Create a new GameObject for the line
        GameObject line = new GameObject("Line");
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    private bool IsPointerOverUIButton()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                return true;
            }
        }

        return false;
    }
}
