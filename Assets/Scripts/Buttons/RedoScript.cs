using UnityEngine;
using UnityEngine.UI;

public class RedoScript : MonoBehaviour
{

    [SerializeField] private Button _redoBtn;

    // Start is called before the first frame update
    void Start()
    {
        _redoBtn.onClick.AddListener(Redo);
    }

    public void Redo()
    {

        if (ActionManager.RedoStack.Count <= 0 && !ActionManager.canRedo)
        {
            NotificationManager.Instance.ShowMessage("There's nothing to Redo");
            return;
        }

        ActionManager.UserAction Action = ActionManager.RedoStack.Peek();
        GameObject prefabType = CanvasState.Instance.shapeCount == 0 ? Materials.Instance.PrefabShape1 : Materials.Instance.PrefabShape2;

        GameObject prefab;

        Vector3 startPoint;
        Vector3 endPoint;

        switch (Action)
        {
            case ActionManager.UserAction.DRAW_POINT:
                Debug.Log("Redo Point Draw");

                // Came from a full previous shape
                if (ShapeManager.CurrentShape.IsClosed)
                {
                    ShapeManager.CurrentShape = new Shape();
                }

                startPoint = ActionManager.PointStack.Pop();
                ShapeManager.CurrentShape.AddPoint(startPoint);
                break;

            case ActionManager.UserAction.DRAW_LINE:
                Debug.Log("Redo Line Draw");

                startPoint = ActionManager.PointStack.Pop();
                endPoint = ActionManager.PointStack.Pop();

                ShapeManager.AddPointToCurrentShape(endPoint);
                ShapeRenderer.DrawLine(ShapeManager.CurrentShape, startPoint, endPoint);

                break;

            case ActionManager.UserAction.CLOSE_SHAPE:
                Debug.Log("Redo Shape Close");

                startPoint = ActionManager.PointStack.Pop();
                endPoint = ShapeManager.CurrentShape.Points[0];
                ShapeRenderer.DrawLine(ShapeManager.CurrentShape, startPoint, endPoint);

                ShapeManager.StartNewShape();
                break;

            case ActionManager.UserAction.GENERATE_SHAPE:
                Debug.Log("Redo Shape Generate");
                ShapeManager.CurrentShape = ActionManager.BuildShapeFromStack(prefabType);
                ShapeManager.StartNewShape();
                ShapeRenderer.RedrawAllShapes();
                break;
            case ActionManager.UserAction.DELETE_SHAPE:
                DeleteCurrShapeScript.DeleteCurrShape();
                break;
        }

        ActionManager.RedoStack.Pop();
        ActionManager.ActionStack.Push(Action);
    }
}
