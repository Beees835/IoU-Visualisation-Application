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
        controls.DefaultMapping.PlacePoint.canceled += OnStopPlacePoint; // Handle point release

        controls.DefaultMapping.Enable();
    }

    private void OnDisable()
    {
        controls.DefaultMapping.Zoom.performed -= OnZoom;
        controls.DefaultMapping.PanCameraLock.started -= OnPanLock;
        controls.DefaultMapping.PanCameraLock.canceled -= OnPanUnlock;
        controls.DefaultMapping.PanCameraMove.performed -= OnPanMove;
        controls.DefaultMapping.PlacePoint.performed -= OnPlacePoint;
        controls.DefaultMapping.PlacePoint.canceled -= OnStopPlacePoint;

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
        else if (pointPlaceControl != null)
        {
            // Send the pan movement to the PointPlaceControl if dragging a point
            pointPlaceControl.DragPoint(context.ReadValue<Vector2>());
        }
    }

    private void OnPlacePoint(InputAction.CallbackContext context)
    {
        if (pointPlaceControl != null)
        {
            pointPlaceControl.PlacePrefabAtMousePosition();
        }
    }

    private void OnStopPlacePoint(InputAction.CallbackContext context)
    {
        if (pointPlaceControl != null)
        {
            pointPlaceControl.StopDragging(); // Stop dragging when releasing
        }
    }
}
