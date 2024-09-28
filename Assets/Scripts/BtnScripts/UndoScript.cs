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
        int numActions = ActionManager.Instance.GetNumberOfActions();
        Debug.Log("Number of actions: " + numActions);
        ActionManager.UserAction lastAction = ActionManager.Instance.ActionStack.Peek();
        if (numActions > 0)
        {
            

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
                    }
                    break;

                case ActionManager.UserAction.DRAW_LINE:
                    UndoDrawLine();
                    ShapeManager.Instance.CurrentShape.RemoveLastPoint();
                    break;
                
                case ActionManager.UserAction.CLOSE_SHAPE:
                    // the shape was closed and locked. need to undo the locked shape and last line drawn
                    // ShapeManager.Instance.AllShapes.RemoveAt(ShapeManager.Instance.AllShapes.Count - 1);
                    // ShapeManager.Instance.CurrentShape = ShapeManager.Instance.AllShapes[ShapeManager.];
                    // if (CanvasState.Instance.shapeCount == 1) {
                    //     ShapeManager.Instance.CurrentShape = ShapeManager.Instance.AllShapes[0];
                    //     ShapeManager.Instance.AllShapes.RemoveAt(ShapeManager.Instance.AllShapes.Count - 1);
                    // }
                    ShapeManager.Instance.CurrentShape.IsClosed = false;
                    ShapeManager.Instance.CurrentLines = ShapeManager.Instance.PrevLines;
                    UndoDrawLine();
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

    public void DestroyLastPrefab() {
        GameObject pf = ShapeManager.Instance.CurrentShape.Prefabs[ShapeManager.Instance.CurrentShape.Prefabs.Count - 1];
        ShapeManager.Instance.CurrentShape.Prefabs.RemoveAt(ShapeManager.Instance.CurrentShape.Prefabs.Count - 1);
        Destroy(pf);
    }
}
