using UnityEngine;
using UnityEngine.UI;

public class ZoomOutBtnScript : MonoBehaviour
{
    [SerializeField] private Button _zoomOutBtn;
    [SerializeField] private CameraController _cameraController;

    void Start()
    {
        _zoomOutBtn.onClick.AddListener(ZoomOut);
    }

    void ZoomOut()
    {
        _cameraController.HandleZoom(-1f); // Zoom out by a fixed amount
    }
}
