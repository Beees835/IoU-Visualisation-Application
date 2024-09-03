using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public CameraController cameraController;
    public PointPlaceControl pointPlaceControl;

    private GeneralControls controls;
    private bool isPanning = false;

    private void Awake()
    {
        controls = new GeneralControls();
    }

    private void OnEnable()
    {
        controls.DefaultMapping.Zoom.performed += OnZoom;
        controls.DefaultMapping.PanCameraLock.started += OnPanLock;
        controls.DefaultMapping.PanCameraLock.canceled += OnPanUnlock;
        controls.DefaultMapping.PanCameraMove.performed += OnPanMove;
        controls.DefaultMapping.PlacePoint.performed += OnPlacePoint;

        controls.DefaultMapping.Enable();
    }

    private void OnDisable()
    {
        controls.DefaultMapping.Zoom.performed -= OnZoom;
        controls.DefaultMapping.PanCameraLock.started -= OnPanLock;
        controls.DefaultMapping.PanCameraLock.canceled -= OnPanUnlock;
        controls.DefaultMapping.PanCameraMove.performed -= OnPanMove;
        controls.DefaultMapping.PlacePoint.performed -= OnPlacePoint;

        controls.DefaultMapping.Disable();
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();
        cameraController.HandleZoom(scrollValue);
    }

    private void OnPanLock(InputAction.CallbackContext context)
    {
        isPanning = true;
    }

    private void OnPanUnlock(InputAction.CallbackContext context)
    {
        isPanning = false;
    }

    private void OnPanMove(InputAction.CallbackContext context)
    {
        if (isPanning)
        {
            Vector2 panValue = context.ReadValue<Vector2>();
            cameraController.HandlePan(panValue);
        }
    }

    private void OnPlacePoint(InputAction.CallbackContext context)
    {
        if (pointPlaceControl != null)
        {
            pointPlaceControl.PlacePrefabAtMousePosition();
        }
    }
}
