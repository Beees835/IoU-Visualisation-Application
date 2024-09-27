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
               
            }
        }
    }

}
