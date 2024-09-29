using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float zoomSpeed = 1f;
    private float panSpeed = 0.3f;
    private float minZoom = 5f;
    private float maxZoom = 15f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void HandleZoom(float scrollValue)
    {
        float newZoom = cam.orthographicSize - scrollValue * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
    }

    public void HandlePan(Vector2 panValue)
    {
        // Pan values are negative so that scroll feels correct
        transform.position += new Vector3(-panValue.x * panSpeed, -panValue.y * panSpeed, 0);
    }
}
