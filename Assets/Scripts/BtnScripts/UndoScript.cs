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
        if (numActions > 0)
        {
            if (ActionManager.Instance.ActionStack.Peek() == ActionManager.UserAction.DRAW_POINT)
            {
                // A singular point (no line) has been drawn to start a new shape. now undo it
                ShapeManager.Instance.CurrentShape.Points.Clear();
                foreach (GameObject pf in ShapeManager.Instance.CurrentShape.Prefabs)
                {
                    Destroy(pf);
                }
            }
            else if (ActionManager.Instance.ActionStack.Peek() == ActionManager.UserAction.DRAW_LINE)
            {
                GameObject lastLine = ShapeManager.Instance.CurrentLines[ShapeManager.Instance.CurrentLines.Count - 1];
                ShapeManager.Instance.CurrentLines.RemoveAt(ShapeManager.Instance.CurrentLines.Count - 1);
                Destroy(lastLine);

                GameObject pf = ShapeManager.Instance.CurrentShape.Prefabs[ShapeManager.Instance.CurrentShape.Prefabs.Count - 1];
                ShapeManager.Instance.CurrentShape.Prefabs.RemoveAt(ShapeManager.Instance.CurrentShape.Prefabs.Count - 1);
                Destroy(pf);

                ShapeManager.Instance.CurrentShape.Points.RemoveAt(ShapeManager.Instance.CurrentShape.Points.Count - 1);

            }
        }
    }

}
