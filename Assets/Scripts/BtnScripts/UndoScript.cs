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
                    ShapeManager.Instance.CurrentShape.Points.Clear();
                    break;

                case ActionManager.UserAction.DRAW_LINE:
                    // undo the last line drawn
                    GameObject lastLine = ShapeManager.Instance.CurrentLines[ShapeManager.Instance.CurrentLines.Count - 1];
                    ShapeManager.Instance.CurrentLines.RemoveAt(ShapeManager.Instance.CurrentLines.Count - 1);
                    Destroy(lastLine);

                    ShapeManager.Instance.CurrentShape.Points.RemoveAt(ShapeManager.Instance.CurrentShape.Points.Count - 1);
                    break;
            }
            GameObject pf = ShapeManager.Instance.CurrentShape.Prefabs[ShapeManager.Instance.CurrentShape.Prefabs.Count - 1];
            ShapeManager.Instance.CurrentShape.Prefabs.RemoveAt(ShapeManager.Instance.CurrentShape.Prefabs.Count - 1);
            Destroy(pf);

            ActionManager.Instance.ActionStack.Pop();
        }
    }

}
