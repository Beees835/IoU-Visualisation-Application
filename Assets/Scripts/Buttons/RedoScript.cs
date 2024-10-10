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

        if (ActionManager.RedoStack.Count <= 0 || !ActionManager.canRedo)
        {
            NotificationManager.Instance.ShowMessage("There's nothing to Redo");
            return;
        }

        ActionManager.UserAction Action = ActionManager.RedoStack.Peek();

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

                // Another point has since been placed
                if (startPoint != ShapeManager.CurrentShape.Points[ShapeManager.CurrentShape.Points.Count - 1])
                {
                    ActionManager.RedoStack.Clear();
                    ActionManager.PointStack.Clear();
                    return;
                }

                ShapeRenderer.DrawLine(ShapeManager.CurrentShape, startPoint, endPoint);

                if (!ShapeManager.CanAddMoreShapes())
                {
                    IoUCalculator.CalculateIoUForShapes();
                    break;
                }

                ShapeManager.StartNewShape();
                break;

            case ActionManager.UserAction.GENERATE_SHAPE:
                Debug.Log("Redo Shape Generate");
                ShapeManager.CurrentShape = ActionManager.BuildShapeFromStack();
                ShapeManager.StartNewShape();
                ShapeRenderer.RedrawAllShapes();
                break;
            case ActionManager.UserAction.DELETE_SHAPE:
                // Not Implemented
                break;
        }

        ActionManager.RedoStack.Pop();
        ActionManager.ActionStack.Push(Action);
    }
}
