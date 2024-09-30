using UnityEngine;
using UnityEngine.SceneManagement;
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
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        ShapeManager.AllShapes.Clear();
        ShapeManager.CurrentShape.Points.Clear();
        ShapeRenderer.ClearAllLines();
        IoUManager.resetInfo();
        CanvasState.Instance.shapeCount = 0;
        CanvasState.Instance.drawState = CanvasState.DrawStates.DRAW_STATE;
        Debug.Log("num shapes: " + ShapeManager.AllShapes.Count);
    }

}
