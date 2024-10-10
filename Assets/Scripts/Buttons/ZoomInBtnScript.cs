using UnityEngine;
using UnityEngine.UI;

public class ZoomInBtnScript : MonoBehaviour
{
    [SerializeField] private Button _zoomInBtn;
    [SerializeField] private CameraController _cameraController;

    void Start()
    {
        _zoomInBtn.onClick.AddListener(ZoomIn);
    }

    void ZoomIn()
    {
        _cameraController.HandleZoom(1f); // Zoom in by a fixed amount
    }
}
