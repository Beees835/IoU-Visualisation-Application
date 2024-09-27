using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0; // Ensure the shape stays in the 2D plane
            transform.position = newPosition;
        }
    }

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}