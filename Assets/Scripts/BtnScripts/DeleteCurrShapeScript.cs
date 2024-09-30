using UnityEngine;
using UnityEngine.UI;

public class DeleteCurrShapeScript : MonoBehaviour
{

    [SerializeField] private Button _deleteCurrShapeBtn;

    // Start is called before the first frame update
    void Start()
    {
        _deleteCurrShapeBtn.onClick.AddListener(DeleteCurrShape);
    }

    public void DeleteCurrShape()
    {
        if (CanvasState.Instance.drawState == CanvasState.DrawStates.MODIFY_STATE)
        {
            Debug.Log("Can't delete shape after intersection");
            return;
        }

        if (ShapeManager.Instance.AllShapes.Count > -1)
        {
            ShapeManager.Instance.CurrentShape.Points.Clear();
            foreach (GameObject pf in ShapeManager.Instance.CurrentShape.Prefabs)
            {
                pf.GetComponent<PointAnimation>().Close();
            }
            ShapeManager.Instance.CurrentShape.Prefabs.Clear();
            ShapeRenderer.Instance.ClearCurrentLines();
            CanvasState.Instance.drawState = CanvasState.DrawStates.DRAW_STATE;
        }
    }
}
