using UnityEngine;
using UnityEngine.UI;

public class ZoomOutBtnScript : MonoBehaviour
{
    [SerializeField] private Button zoomOutBtn;
    [SerializeField] private CameraController cameraController;

    void Start()
    {
        zoomOutBtn.onClick.AddListener(ZoomOut);
    }

    void ZoomOut()
    {
        cameraController.HandleZoom(-1f); // Zoom out by a fixed amount
    }
}
