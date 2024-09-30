using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float panSpeed = 0.3f;
    [SerializeField] public float minZoom = 5f;
    [SerializeField] public float maxZoom = 15f;

    [Header("UI Elements")]
    [SerializeField] private Slider zoomSlider;
    [SerializeField] private TextMeshProUGUI zoomPercentageText;

    [HideInInspector] public Camera cam;

    [HideInInspector] public bool isUpdating = false;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        // Initialize slider values
        zoomSlider.minValue = 0f;
        zoomSlider.maxValue = 1f;

        // Set initial slider value and zoom percentage text
        UpdateZoomUI();
    }

    private void Update()
    {
        // Handle mouse wheel zoom
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (scrollData != 0.0f)
        {
            HandleZoom(-scrollData * zoomSpeed);
        }
    }

    public void HandleZoom(float zoomAmount)
    {
        float newZoom = Mathf.Clamp(cam.orthographicSize - zoomAmount, minZoom, maxZoom);
        cam.orthographicSize = newZoom;

        // Update the Zoom UI
        UpdateZoomUI();
    }

    public void HandlePan(Vector2 panValue)
    {
        transform.position += new Vector3(-panValue.x * panSpeed, -panValue.y * panSpeed, 0);
    }

    public void UpdateZoomUI()
    {
        isUpdating = true;

        float val = cam.orthographicSize;

        // Calculate the zoom percentage
        float zoomPercentage = (maxZoom - val) / (maxZoom - minZoom) * 100f;
        zoomPercentageText.text = Mathf.RoundToInt(zoomPercentage).ToString() + "%";

        // Invert the slider value calculation
        zoomSlider.value = (maxZoom - val) / (maxZoom - minZoom);

        isUpdating = false;
    }
}
