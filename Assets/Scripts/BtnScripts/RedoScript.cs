using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedoScript : MonoBehaviour
{

    [SerializeField] private Button _redoBtn;
    public GameObject PrefabShape1;
    public GameObject PrefabShape2;

    // Start is called before the first frame update
    void Start()
    {
        _redoBtn.onClick.AddListener(Redo);
    }

    public void Redo() {
        if (ActionManager.Instance.RedoStack.Count > 0)
        {
            ActionManager.UserAction Action = ActionManager.Instance.RedoStack.Peek();

            switch(Action) {
                case ActionManager.UserAction.DRAW_POINT:
                    Debug.Log("Redo Point Draw");
                    if (CanvasState.Instance.shapeCount == 0)
                    {

                    }
                    else 
                    {
                        // undid the first point of shape 2. need to redraw it.
                        Shape newCurrShape = ShapeManager.Instance.PrevShapes.Peek();
                        Vector3 point = newCurrShape.PrevPoints.Pop();
                        newCurrShape.Points.Add(point);
                        GameObject newPrefab = Instantiate(PrefabShape2, point, Quaternion.identity);
                        ShapeManager.Instance.CurrentShape.Prefabs.Add(newPrefab);

                        ShapeManager.Instance.CurrentShape = newCurrShape;
                        CanvasState.Instance.shapeCount++;
                    }
                    break;

                case ActionManager.UserAction.DRAW_LINE:
                    Debug.Log("Redo Line Draw");
                    break;

                case ActionManager.UserAction.CLOSE_SHAPE:
                    Debug.Log("Redo Shape Close");
                    break;

                case ActionManager.UserAction.GENERATE_SHAPE:
                    Debug.Log("Redo Shape Generate");
                    break;
            }
        }
    }
}
