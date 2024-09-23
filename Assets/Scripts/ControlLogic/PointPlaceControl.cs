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
    private List<List<Vector3>> allShapes = new List<List<Vector3>>(); // Stores all shapes' points
    private List<Vector3> currentShape = new List<Vector3>(); // Stores points for the current shape
    private bool lockedFirstShape = false;
    private int draggedPointIndex = -1; // Index of the point being dragged
    private bool isDragging = false; // Whether we are currently dragging a point
    private List<List<GameObject>> allPrefabs = new List<List<GameObject>>(); // Stores all prefabs for each shape
    private List<GameObject> currentPrefabs = new List<GameObject>(); // Stores prefabs for the current shape


    // Function to trigger point placement or modification
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
                currentShape = new List<Vector3>();  
            }
            prefab = prefabShape2;
        }

        if (CanvasState.Instance.drawState == CanvasState.DrawStates.MODIFY_STATE)
        {
            HandleModifyState();
        }
        else
        {
            AddNewPoint();
        }
    }


    public void HandleModifyState()
    {
        if (draggedPointIndex == -1) 
        {
            SelectPoint();
        }
    }

    public void DragPoint(Vector2 panDelta)
    {
        if (isDragging && draggedPointIndex != -1)
        {
            Vector3 deltaWorld = cam.ScreenToWorldPoint(new Vector3(panDelta.x, panDelta.y, 0)) - cam.ScreenToWorldPoint(Vector3.zero);

            // Move the dragged point
            currentShape[draggedPointIndex] += new Vector3(deltaWorld.x, deltaWorld.y, 0);

            // Move the associated prefab
            if (draggedPointIndex < currentPrefabs.Count)
            {
                currentPrefabs[draggedPointIndex].transform.position = currentShape[draggedPointIndex];
            }
            
            // Redraw the shape to reflect the new positions of the points
            RedrawShape();
        }
    }


    public void StopDragging()
    {
        if (isDragging)
        {
            draggedPointIndex = -1;
            isDragging = false;
        }
    }

    private void SelectPoint()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        // Iterate through all shapes (allShapes)
        for (int shapeIndex = 0; shapeIndex < allShapes.Count; shapeIndex++)
        {
            List<Vector3> shape = allShapes[shapeIndex];

            // Iterate through each point in the shape
            for (int pointIndex = 0; pointIndex < shape.Count; pointIndex++)
            {
                if (Vector3.Distance(mouseWorldPosition, shape[pointIndex]) <= closeThreshold + 1.0)
                {
                    Debug.Log("Point found for dragging");

                    draggedPointIndex = pointIndex;
                    isDragging = true;

                    // Set the current shape and its corresponding prefab list
                    currentShape = shape;
                    currentPrefabs = allPrefabs[shapeIndex]; // Update currentPrefabs to match the selected shape

                    Debug.Log($"Shape {shapeIndex}, Point {draggedPointIndex}");
                    return;
                }
            }
        }
    }

    private void AddNewPoint()
    {
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

        spawnPosition.z = -1f; // Keeps the prefab visible

        // Check if the point is near the first point to close the shape
        if (currentShape.Count > 2 && Vector3.Distance(spawnPosition, currentShape[0]) <= closeThreshold)
        {
            DrawLine(currentShape[currentShape.Count - 1], currentShape[0]);
            
            allShapes.Add(new List<Vector3>(currentShape));
            allPrefabs.Add(new List<GameObject>(currentPrefabs));

            currentShape.Clear();
            currentPrefabs.Clear(); 
            lockedFirstShape = false;
            CanvasState.Instance.shapeCount++;
            return;
        }

        if (IsConvexWithNewPoint(spawnPosition))
        {
            currentShape.Add(spawnPosition);
            
            // Instantiate the prefab at the point and store it in currentPrefabs
            GameObject newPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);
            currentPrefabs.Add(newPrefab);

            if (currentShape.Count > 1)
            {
                DrawLine(currentShape[currentShape.Count - 2], currentShape[currentShape.Count - 1]);
            }
        }
        else
        {
            Debug.Log("Point not placed: shape would not be convex.");
        }
    }


    private void RedrawShape()
    {
        // Clear existing lines
        foreach (var line in GameObject.FindGameObjectsWithTag("Line"))
        {
            Destroy(line);
        }

        // Draw lines for all the stored shapes
        foreach (var shape in allShapes)
        {
            for (int i = 0; i < shape.Count; i++)
            {
                Vector3 start = shape[i];
                Vector3 end = shape[(i + 1) % shape.Count];
                DrawLine(start, end);
            }
        }

        // Draw lines for the current shape being created/modified
        for (int i = 0; i < currentShape.Count; i++)
        {
            Vector3 start = currentShape[i];
            Vector3 end = currentShape[(i + 1) % currentShape.Count];
            DrawLine(start, end);
        }
    }

    private bool IsConvexWithNewPoint(Vector3 newPoint)
    {
        if (currentShape.Count < 2)
        {
            return true;
        }

        List<Vector3> testPoints = new List<Vector3>(currentShape) { newPoint };
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
        GameObject line = new GameObject("Line");
        line.tag = "Line";

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
