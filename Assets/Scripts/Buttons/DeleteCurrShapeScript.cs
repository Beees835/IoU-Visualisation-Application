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

        if (ShapeManager.AllShapes.Count > -1)
        {
            // set lines as inactive
            ShapeRenderer.ClearCurrentLines();
            foreach (GameObject pf in ShapeManager.CurrentShape.Prefabs)
            {
                // remove points from screen
                if (pf == null) continue;
                pf.GetComponent<PointAnimation>().Close();
                pf.SetActive(false);
            }
            
            // store deleted shape in case of redo
            ShapeManager.PrevShapes.Push(ShapeManager.CurrentShape);

            // reconfigure state
            CanvasState.Instance.shapeCount--;
            ShapeManager.AllShapes.Remove(ShapeManager.CurrentShape);
            CanvasState.Instance.drawState = CanvasState.DrawStates.DRAW_STATE;
            ActionManager.Instance.ActionStack.Push(ActionManager.UserAction.DELETE_SHAPE);
        }
    }
}
