using System.Collections;
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
        if (ActionManager.Instance.ActionStack.Count > 0)
        {
            ActionManager.UserAction lastAction = ActionManager.Instance.ActionStack.Peek();

            switch(lastAction)
            {
                case ActionManager.UserAction.DRAW_POINT:
                    // A singular point (no line) has been drawn to start a new shape. now undo it
                    ShapeManager.Instance.CurrentShape.RemoveLastPoint();
                    if (CanvasState.Instance.shapeCount > 1) {
                        // if destroying the first point of shape 2, reassign the current shape to shape 1
                        ShapeManager.Instance.CurrentShape = ShapeManager.Instance.AllShapes[0];
                        ShapeManager.Instance.AllShapes.RemoveAt(ShapeManager.Instance.AllShapes.Count - 1);
                        CanvasState.Instance.shapeCount--;
                        ShapeManager.Instance.CurrentLines = ShapeManager.Instance.PrevLines;
                        ShapeManager.Instance.PrevLines = new List<GameObject>();
                    } 
                    break;

                case ActionManager.UserAction.DRAW_LINE:
                    UndoDrawLine();
                    ShapeManager.Instance.CurrentShape.RemoveLastPoint();
                    break;
                
                case ActionManager.UserAction.CLOSE_SHAPE:
                    // the shape was closed and locked. need to undo the locked shape and last line drawn

                    if (CanvasState.Instance.shapeCount > 0)
                    {
                        // We've closed the first shape and are now undoing it. 
                        // Need to reassign the current shape to the second shape
                        ShapeManager.Instance.CurrentShape = ShapeManager.Instance.AllShapes[0];
                        ShapeManager.Instance.AllShapes.RemoveAt(ShapeManager.Instance.AllShapes.Count - 1);
                        CanvasState.Instance.shapeCount--;
                    }    

                    ShapeManager.Instance.CurrentShape.IsClosed = false;
                    ShapeManager.Instance.CurrentLines = ShapeManager.Instance.PrevLines;

                    UndoDrawLine();
                    break;

                case ActionManager.UserAction.GENERATE_SHAPE:
                    // A shape has been rando generated. Undo the last shape generated
                    ShapeManager.Instance.DeleteLastShape();
                    break;
            }

            ActionManager.Instance.ActionStack.Pop();
        }
    }

    public void UndoDrawLine()
    {
        // undo the last line drawn
        GameObject lastLine = ShapeManager.Instance.CurrentLines[ShapeManager.Instance.CurrentLines.Count - 1];
        ShapeManager.Instance.CurrentLines.RemoveAt(ShapeManager.Instance.CurrentLines.Count - 1);
        Destroy(lastLine);
    }
}
