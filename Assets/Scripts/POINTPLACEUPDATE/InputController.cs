using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Camera Cam;
    public GameObject PrefabShape1;
    public GameObject PrefabShape2;
    public float CloseThreshold = 0.1f;

    private GameObject currentPrefab;
    private int draggedPointIndex = -1;
    private bool isDragging = false;

    private Shape selectedShape; // Keeps track of the shape being modified

    public void PlacePrefabAtMousePosition()
    {
        if (IsPointerOverUIButton())
        {
            return;
        }

        
        // Logic to select the correct prefab
        if (CanvasState.Instance.shapeCount == 1)
        {
            currentPrefab = PrefabShape1;
        }
        else if (CanvasState.Instance.shapeCount >= 2)
        {
            currentPrefab = PrefabShape2;
            //ShapeManager.Instance.StartNewShape();
        }

        switch (CanvasState.Instance.drawState)
        {
            case CanvasState.DrawStates.LOCK_STATE:
                break;
            case CanvasState.DrawStates.MODIFY_STATE:
                HandleModifyState();
                break;
            case CanvasState.DrawStates.DRAW_STATE:
                AddNewPoint();
                break;
        }
    }

    private void HandleModifyState()
    {
        if (draggedPointIndex == -1)
        {
            SelectPoint();
        }
    }

    private void AddNewPoint()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 spawnPosition = GetWorldPosition(mouseScreenPosition);

        // Check if the point is near the first point to close the shape
        if (ShapeManager.Instance.CurrentShape.Points.Count > 2 &&
            Vector3.Distance(spawnPosition, ShapeManager.Instance.CurrentShape.Points[0]) <= CloseThreshold)
        {
            // Close the current shape
            ShapeManager.Instance.CurrentShape.IsClosed = true;
            ShapeManager.Instance.StartNewShape();
            ShapeRenderer.Instance.RedrawAllShapes();
            CanvasState.Instance.shapeCount++;
            ActionManager.Instance.ActionStack.Push(ActionManager.UserAction.CLOSE_SHAPE);
            return;
        }

        // Add the new point to the current shape
        if (ShapeManager.Instance.CurrentShape.IsConvexWithNewPoint(spawnPosition))
        {
             // current shape hasn't been set yet, this point will be the first point
            if (ShapeManager.Instance.CurrentShape.Prefabs.Count == 0)
            {
                ActionManager.Instance.ActionStack.Push(ActionManager.UserAction.DRAW_POINT);
            }
            else 
            {
                ActionManager.Instance.ActionStack.Push(ActionManager.UserAction.DRAW_LINE);
            }
            GameObject newPrefab = Instantiate(currentPrefab, spawnPosition, Quaternion.identity);
            ShapeManager.Instance.AddPointToCurrentShape(spawnPosition, newPrefab);
            ShapeRenderer.Instance.RedrawAllShapes();
        }
        else
        {
            Debug.Log("Point not placed: shape would not be convex.");
        }
    }


    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Ray ray = Cam.ScreenPointToRay(screenPosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        Vector3 worldPosition;

        if (hit.collider != null)
        {
            worldPosition = hit.point;
        }
        else
        {
            worldPosition = Cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -Cam.transform.position.z));
        }

        worldPosition.z = -1f; // Keeps the prefab visible
        return worldPosition;
    }

    public void DragPoint(Vector2 panDelta)
    {
        if (isDragging && draggedPointIndex != -1 && selectedShape != null)
        {
            Vector3 deltaWorld = Cam.ScreenToWorldPoint(new Vector3(panDelta.x, panDelta.y, 0)) - Cam.ScreenToWorldPoint(Vector3.zero);

            // Create a copy of the points to simulate the movement
            List<Vector3> testPoints = new List<Vector3>(selectedShape.Points);

            // Simulate moving the point
            testPoints[draggedPointIndex] += new Vector3(deltaWorld.x, deltaWorld.y, 0);

            // Check if the shape remains convex after moving the point
            if (Shape.IsConvex(testPoints))
            {
                // Move the dragged point
                selectedShape.Points[draggedPointIndex] = testPoints[draggedPointIndex];

                // Move the associated prefab
                if (draggedPointIndex < selectedShape.Prefabs.Count)
                {
                    selectedShape.Prefabs[draggedPointIndex].transform.position = selectedShape.Points[draggedPointIndex];
                }
                
                ShapeRenderer.Instance.RedrawAllShapes();
            }
            else
            {
                Debug.Log("Point movement rejected: shape would not be convex.");
            }
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
        Vector3 mouseWorldPosition = Cam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, -Cam.transform.position.z));
        mouseWorldPosition.z = 0f;

        // Iterate through all shapes
        foreach (Shape shape in ShapeManager.Instance.AllShapes)
        {
            for (int pointIndex = 0; pointIndex < shape.Points.Count; pointIndex++)
            {
                if (Vector3.Distance(mouseWorldPosition, shape.Points[pointIndex]) <= CloseThreshold + 1.0f)
                {
                    Debug.Log("Point found for dragging");

                    draggedPointIndex = pointIndex;
                    isDragging = true;

                    // Set the selected shape
                    selectedShape = shape;

                    Debug.Log($"Point {draggedPointIndex} selected in shape.");
                    return;
                }
            }
        }
    }

    private bool IsPointerOverUIButton()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<UnityEngine.UI.Button>() != null)
            {
                return true;
            }
        }

        return false;
    }
}
