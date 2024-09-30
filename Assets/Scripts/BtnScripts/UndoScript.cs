using System.Collections.Generic;
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
            Debug.Log("Can't undo Canvas has been locked");
            return;
        }


        if (ActionManager.Instance.ActionStack.Count > 0)
        {
            ActionManager.UserAction lastAction = ActionManager.Instance.ActionStack.Peek();

            switch (lastAction)
            {
                case ActionManager.UserAction.DRAW_POINT:
                    Debug.Log("Undo Point Draw");

                    // A singular point (no line) has been drawn to start a new shape. now undo it
                    ShapeManager.CurrentShape.RemoveLastPoint();

                    // store shape in case of redo
                    ShapeManager.PrevShapes.Push(ShapeManager.CurrentShape);

                    if (CanvasState.Instance.shapeCount > 1)
                    {
                        // if destroying the first point of shape 2, reassign the current shape to shape 1
                        ShapeManager.CurrentShape = ShapeManager.AllShapes[0];
                        ShapeManager.AllShapes.RemoveAt(ShapeManager.AllShapes.Count - 1);
                        CanvasState.Instance.shapeCount--;
                        ShapeManager.CurrentLines = ShapeManager.PrevLines;
                        ShapeManager.PrevLines = new List<GameObject>();
                    }
                    break;

                case ActionManager.UserAction.DRAW_LINE:
                    Debug.Log("Undo Line Draw");

                    UndoDrawLine();
                    ShapeManager.CurrentShape.RemoveLastPoint();
                    break;

                case ActionManager.UserAction.CLOSE_SHAPE:
                    Debug.Log("Undo Shape Close");

                    // the shape was closed and locked. need to undo the locked shape and last line drawn
                    if (CanvasState.Instance.shapeCount > 0)
                    {
                        // We've closed the first shape and are now undoing it. 
                        // Need to reassign the current shape to the first shape
                        ShapeManager.CurrentShape = ShapeManager.AllShapes[0];
                        ShapeManager.AllShapes.RemoveAt(ShapeManager.AllShapes.Count - 1);
                        CanvasState.Instance.shapeCount--;
                    }

                    ShapeManager.CurrentShape.IsClosed = false;
                    ShapeManager.CurrentLines = ShapeManager.PrevLines;

                    UndoDrawLine();
                    break;

                case ActionManager.UserAction.GENERATE_SHAPE:
                    Debug.Log("Undo Shape Gen");

                    // A shape has been rando generated. Undo the last shape generated
                    ShapeManager.DeleteLastShape();
                    ShapeRenderer.RedrawAllShapes();
                    break;
            }

            ActionManager.Instance.ActionStack.Pop();
            ActionManager.Instance.RedoStack.Push(lastAction);
        }
    }

    public void UndoDrawLine()
    {
        // undo the last line drawn
        GameObject lastLine = ShapeManager.CurrentLines[ShapeManager.CurrentLines.Count - 1];
        ShapeManager.CurrentLines.RemoveAt(ShapeManager.CurrentLines.Count - 1);
        lastLine.SetActive(false);
        ActionManager.Instance.UndoneLines.Push(lastLine);
    }
}
