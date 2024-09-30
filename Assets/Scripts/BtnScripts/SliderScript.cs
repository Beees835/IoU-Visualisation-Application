using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider zoomSlider;
    [SerializeField] private CameraController cameraController;

    void Start()
    {
        zoomSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float normalizedVal)
    {
        if (cameraController.isUpdating) return;

        // Calculation and adjust to invert the slider mapping
        float targetZoom = cameraController.maxZoom - normalizedVal * (cameraController.maxZoom - cameraController.minZoom);
        cameraController.cam.orthographicSize = targetZoom;

        // Update the zoom UI
        cameraController.UpdateZoomUI();
    }
}
