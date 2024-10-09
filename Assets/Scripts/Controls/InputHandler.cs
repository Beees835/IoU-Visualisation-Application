using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public CameraController cameraController;
    public InputController inputController;

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
        controls.DefaultMapping.PlacePoint.canceled += OnStopPlacePoint;

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
        inputController?.StopDragging();
    }

    private void OnPanMove(InputAction.CallbackContext context)
    {
        Vector2 panValue = context.ReadValue<Vector2>();
        if (isPanning)
        {
            cameraController.HandlePan(panValue);
        }
        else
        {
            inputController?.Drag(panValue);
        }
    }

    private void OnPlacePoint(InputAction.CallbackContext context)
    {
        inputController?.HandleClick(context);
    }

    private void OnStopPlacePoint(InputAction.CallbackContext context)
    {
        inputController?.StopDragging();
    }
}
