using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider _zoomSlider;
    [SerializeField] private CameraController _cameraController;

    void Start()
    {
        _zoomSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float normalizedVal)
    {
        if (_cameraController.isUpdating) return;

        // Calculation and adjust to invert the slider mapping
        float targetZoom = _cameraController.maxZoom - normalizedVal * (_cameraController.maxZoom - _cameraController.minZoom);
        _cameraController._cam.orthographicSize = targetZoom;

        // Update the zoom UI
        _cameraController.UpdateZoomUI();
    }
}
