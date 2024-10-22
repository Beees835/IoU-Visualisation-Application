using System.Linq;
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

    // Trigger Undo if CTRL/CMD + Z key combination is pressed
    void Update()
    {
        // Check for CTRL/CMD + Z key combination
        bool isCtrlOrCmdPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                                  Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

        // Not condition to check for Shift key is pressed or not 
        if (isCtrlOrCmdPressed && Input.GetKeyDown(KeyCode.Z) && !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            Undo();
        }
    }

    void Undo()
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

                if (!ShapeManager.CanAddMoreShapes())
                {
                    IoUCalculator.Reset();
                }

                // the shape was closed and locked. need to undo the locked shape and last line drawn
                if (ShapeManager.GetShapeCount() > 0)
                {
                    // We've closed the first shape and are now undoing it. 
                    // Need to reassign the current shape to the first shape
                    ShapeManager.CurrentShape = ShapeManager.AllShapes.Last();
                    ShapeManager.AllShapes.RemoveAt(ShapeManager.GetShapeCount() - 1);
                }

                ShapeManager.CurrentShape.IsClosed = false;

                startPoint = ShapeManager.CurrentShape.RemoveLastLine();

                ActionManager.PointStack.Push(startPoint);
                break;
            case ActionManager.UserAction.GENERATE_SHAPE:
                Debug.Log("Undo Shape Gen");

                // A shape has been rando generated. Undo the last shape generated
                shape = ShapeManager.AllShapes.Last();
                ShapeManager.AllShapes.RemoveAt(ShapeManager.GetShapeCount() - 1);

                ActionManager.ShapeSizeStack.Push(shape.Points.Count);
                foreach (var point in shape.Points)
                {
                    ActionManager.PointStack.Push(point);
                }

                shape.ClearShape();

                ShapeManager.CurrentShape = new Shape();
                break;
            case ActionManager.UserAction.DELETE_SHAPE:
                Debug.Log("Undo Delete Shape");


                shape = ActionManager.BuildShapeFromStack();
                ShapeManager.CurrentShape = shape;
                ShapeManager.StartNewShape();
                ShapeRenderer.DrawShape(shape);
                break;
        }

        ActionManager.ActionStack.Pop();
        ActionManager.RedoStack.Push(lastAction);
        ActionManager.canRedo = true;
    }
}
