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
            NotificationManager.Instance.ShowMessage("Can't delete shape after intersection has been calculated.");
            Debug.Log("Can't delete shape after intersection");
            return;
        }

        if (ShapeManager.AllShapes.Count > -1)
        {
            ShapeManager.CurrentShape.Points.Clear();
            foreach (GameObject pf in ShapeManager.CurrentShape.Prefabs)
            {
                pf.GetComponent<PointAnimation>().Close();
            }
            ShapeManager.CurrentShape.Prefabs.Clear();
            ShapeRenderer.ClearCurrentLines();
            CanvasState.Instance.drawState = CanvasState.DrawStates.DRAW_STATE;
        }
    }
}
