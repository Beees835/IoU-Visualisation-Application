using UnityEngine;
using UnityEngine.UI;

public class ZoomInBtnScript : MonoBehaviour
{
    [SerializeField] private Button zoomInBtn;
    [SerializeField] private CameraController cameraController;

    void Start()
    {
        zoomInBtn.onClick.AddListener(ZoomIn);
    }

    void ZoomIn()
    {
        cameraController.HandleZoom(1f); // Zoom in by a fixed amount
    }
}
