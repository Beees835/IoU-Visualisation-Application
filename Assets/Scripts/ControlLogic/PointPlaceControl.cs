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

    private GameObject prefab;
    private Vector3? lastPosition = null; 
    private bool lockedFirstShape= false;
    
    public void PlacePrefabAtMousePosition()
    {
        // Check if the mouse is over a Button UI element
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
            if (lockedFirstShape == false) {
                lockedFirstShape = true;
                lastPosition = null;
            }
            prefab = prefabShape2;
        }
        else {
            prefab = null;
        }
        if (prefab == null)
        {
            Debug.LogError("Prefab not assigned.");
            return;
        }

        // Get the mouse position in screen space
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        // Convert the screen position to world space
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

        spawnPosition.z = -1f;
        Instantiate(prefab, spawnPosition, Quaternion.identity);

        if (lastPosition.HasValue)
        {
            DrawLine(lastPosition.Value, spawnPosition);
        }

        lastPosition = spawnPosition;
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
