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

    void ClearCanvas()
    {
        ShapeManager.DestroyAllShapes();
        IoUCalculator.Reset();
        ActionManager.Reset();
        CanvasState.Instance.Reset();
    }
}
