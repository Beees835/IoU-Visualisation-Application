using UnityEngine;
using UnityEngine.UI;

public class ClearCanvasScript : MonoBehaviour
{

    [SerializeField] private Button _clearCanvasBtn;

    // Start is called before the first frame update
    void Start()
    {
        _clearCanvasBtn.onClick.AddListener(ClearCanvas);

    }

    // Trigger Redo if CTRL/CMD + R key combination is pressed 
    void Update()
    {

        // Check for CTRL/CMD + R key combination
        bool isCtrlOrCmdPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                                  Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

        if (isCtrlOrCmdPressed && Input.GetKeyDown(KeyCode.R))
        {
            ClearCanvas();
        }
    }

    void ClearCanvas()
    {
        ShapeManager.Reset();
        IoUCalculator.Reset();
        ActionManager.Reset();
        NotificationManager.Instance.ClearMessage();
        CanvasState.Instance.Reset();
    }
}
