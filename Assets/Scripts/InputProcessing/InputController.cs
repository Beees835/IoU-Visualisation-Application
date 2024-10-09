using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Camera Cam;
    public float CloseThreshold = 0.1f;
    private int draggedPointIndex = -1;
    private bool isDragging = false;

    private GameObject currentPrefab;
    private Shape selectedShape; // Keeps track of the shape being modified

    public void PlacePrefabAtMousePosition()
    {
        // Logic to select the correct prefab
        if (CanvasState.Instance.shapeCount == 0)
        {
            currentPrefab = Materials.Instance.PrefabShape1;
        }
        else if (CanvasState.Instance.shapeCount == 1)
        {
            currentPrefab = Materials.Instance.PrefabShape2;
        }

        switch (CanvasState.Instance.drawState)
        {
            case CanvasState.DrawStates.DRAW_STATE:
                AddNewPoint();
                break;
            case CanvasState.DrawStates.MODIFY_STATE:
                HandleModifyState();
                break;
            case CanvasState.DrawStates.LOCK_STATE:
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
        if (ShapeManager.CurrentShape.Points.Count > 2 &&
            Vector3.Distance(spawnPosition, ShapeManager.CurrentShape.Points[0]) <= CloseThreshold)
        {
            // Close the current shape
            ShapeManager.StartNewShape();
            ShapeRenderer.RedrawAllShapes();
            ActionManager.ActionStack.Push(ActionManager.UserAction.CLOSE_SHAPE);
            ActionManager.canRedo = false;
            return;
        }

        // Add the new point to the current shape
        if (ShapeManager.CurrentShape.IsConvexWithNewPoint(spawnPosition))
        {
            NotificationManager.Instance.ClearMessage();
            // current shape hasn't been set yet, this point will be the first point
            if (ShapeManager.CurrentShape.RenderedPoints.Count == 0)
            {
                ActionManager.ActionStack.Push(ActionManager.UserAction.DRAW_POINT);
                ActionManager.canRedo = false;
            }
            else
            {
                ActionManager.ActionStack.Push(ActionManager.UserAction.DRAW_LINE);
                ActionManager.canRedo = false;
            }
            ShapeManager.AddPointToCurrentShape(spawnPosition);
            ShapeRenderer.RedrawAllShapes();
        }
        else
        {
            // new point results in an invalid shape
            GameObject invalidClickMark = Instantiate(Materials.Instance.invalidMarkPrefab, spawnPosition, Quaternion.identity);
            PointAnimation pointAnimation = invalidClickMark.GetComponent<PointAnimation>();
            pointAnimation.quickLife();
            NotificationManager.Instance.ShowMessage("Cannot place point here. Shape would not be Convex");
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
                if (draggedPointIndex < selectedShape.RenderedPoints.Count)
                {
                    selectedShape.RenderedPoints[draggedPointIndex].transform.position = selectedShape.Points[draggedPointIndex];
                }

                IoUCalculator.CalculateIoUForShapes();
                ShapeRenderer.RedrawAllShapes();
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

        bool dragPointFlag = false;
        // Iterate through all shapes
        foreach (Shape shape in ShapeManager.AllShapes)
        {
            for (int pointIndex = 0; pointIndex < shape.Points.Count; pointIndex++)
            {
                if (Vector3.Distance(mouseWorldPosition, shape.Points[pointIndex]) <= CloseThreshold + 1.0f)
                {
                    dragPointFlag = true;
                    Debug.Log("Point found for dragging");
                    NotificationManager.Instance.ClearMessage();

                    draggedPointIndex = pointIndex;
                    isDragging = true;

                    // Set the selected shape
                    selectedShape = shape;

                    Debug.Log($"Point {draggedPointIndex} selected in shape.");
                    return;
                }
            }
        }
        if (!dragPointFlag && CanvasState.Instance.hovering)
        {
            NotificationManager.Instance.ShowMessage("Cannot add new points when the two shapes are defined");
        }
    }
}
