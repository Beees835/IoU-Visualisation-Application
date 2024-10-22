using UnityEngine;
using UnityEngine.UI;

public class ClearCanvasScript : MonoBehaviour
{
    [SerializeField] private Button _clearCanvasBtn;

    void Start()
    {
        _clearCanvasBtn.onClick.AddListener(CanvasState.Instance.ClearCanvas);
    }

    void Update()
    {
        bool isCtrlOrCmdPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                                  Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

        if (isCtrlOrCmdPressed && Input.GetKeyDown(KeyCode.R)) CanvasState.Instance.ClearCanvas();
    }
}
