using UnityEngine;
using UnityEngine.UI;

public class UndoScript : MonoBehaviour
{
    [SerializeField] private Button _undoBtn;

    // Start is called before the first frame update
    void Start()
    {
        _undoBtn.onClick.AddListener(Undo);
    }

    public void Undo()
    {
        if (CanvasState.Instance.drawState == CanvasState.DrawStates.MODIFY_STATE)
        {
            // Remove Intersection Shape
            IoUCalculator.Reset();
        }

        if (ActionManager.ActionStack.Count == 0)
        {
            NotificationManager.Instance.ShowMessage("There's nothing to Undo");
        }


        ActionManager.UserAction lastAction = ActionManager.ActionStack.Peek();

        Vector3 startPoint;
        Vector3 endPoint;
        Shape shape;


        switch (lastAction)
        {
            case ActionManager.UserAction.DRAW_POINT:
                Debug.Log("Undo Point Draw");

                // A singular point (no line) has been drawn to start a new shape. now undo it
                startPoint = ShapeManager.CurrentShape.RemoveLastPoint();

                ActionManager.PointStack.Push(startPoint);

                if (CanvasState.Instance.shapeCount > 1)
                {
                    // if destroying the first point of shape 2, reassign the current shape to shape 1
                    ShapeManager.CurrentShape = ShapeManager.AllShapes[ShapeManager.AllShapes.Count - 2];
                    ShapeManager.AllShapes.RemoveAt(ShapeManager.AllShapes.Count - 1);
                    CanvasState.Instance.shapeCount--;
                }
                break;

            case ActionManager.UserAction.DRAW_LINE:
                Debug.Log("Undo Line Draw");

                endPoint = ShapeManager.CurrentShape.RemoveLastPoint();
                startPoint = ShapeManager.CurrentShape.RemoveLastLine();

                ActionManager.PointStack.Push(endPoint);
                ActionManager.PointStack.Push(startPoint);
                break;

            case ActionManager.UserAction.CLOSE_SHAPE:
                Debug.Log("Undo Shape Close");

                // the shape was closed and locked. need to undo the locked shape and last line drawn
                if (CanvasState.Instance.shapeCount > 0)
                {
                    // We've closed the first shape and are now undoing it. 
                    // Need to reassign the current shape to the first shape
                    ShapeManager.CurrentShape = ShapeManager.AllShapes[ShapeManager.AllShapes.Count - 1];
                    ShapeManager.AllShapes.RemoveAt(ShapeManager.AllShapes.Count - 1);
                    CanvasState.Instance.shapeCount--;
                }

                ShapeManager.CurrentShape.IsClosed = false;

                startPoint = ShapeManager.CurrentShape.RemoveLastLine();

                ActionManager.PointStack.Push(startPoint);
                break;
            case ActionManager.UserAction.GENERATE_SHAPE:
                Debug.Log("Undo Shape Gen");

                // A shape has been rando generated. Undo the last shape generated
                shape = ShapeManager.AllShapes[ShapeManager.AllShapes.Count - 1];
                ShapeManager.AllShapes.RemoveAt(ShapeManager.AllShapes.Count - 1);

                ActionManager.ShapeSizeStack.Push(shape.Points.Count);
                foreach (var point in shape.Points)
                {
                    ActionManager.PointStack.Push(point);
                }

                ShapeManager.DestroyShape(shape);
                CanvasState.Instance.shapeCount--;

                ShapeManager.CurrentShape = new Shape();
                break;
            case ActionManager.UserAction.DELETE_SHAPE:
                Debug.Log("Undo Delete Partial Shape");

                bool shapeCompleted = ActionManager.DeleteCompletion.Pop();

                shape = ActionManager.BuildShapeFromStack();
                ShapeManager.CurrentShape = shape;

                if (shapeCompleted)
                {
                    ShapeManager.StartNewShape();
                }
                ShapeRenderer.DrawShape(shape);
                break;
        }

        ActionManager.ActionStack.Pop();
        ActionManager.RedoStack.Push(lastAction);
        ActionManager.canRedo = true;
    }
}
