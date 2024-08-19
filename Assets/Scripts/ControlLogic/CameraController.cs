using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 1f;
    public float panSpeed = 0.5f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

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
        Vector3 move = new Vector3(-panValue.x * panSpeed, -panValue.y * panSpeed, 0);
        transform.position += move;
    }
}
