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
                    
                    Shape newCurrShape = ShapeManager.Instance.PrevShapes.Peek();

                    Vector3 point = newCurrShape.PrevPoints.Pop();
                    newCurrShape.Points.Add(point);

                    GameObject pfType;
                    pfType = CanvasState.Instance.shapeCount == 0 ? PrefabShape1 : PrefabShape2;

                    GameObject newPrefab = Instantiate(pfType, point, Quaternion.identity);
                    ShapeManager.Instance.CurrentShape.Prefabs.Add(newPrefab);

                    ShapeManager.Instance.CurrentShape = newCurrShape;
                    break;

                case ActionManager.UserAction.DRAW_LINE:
                    Debug.Log("Redo Line Draw");
                    break;

                case ActionManager.UserAction.CLOSE_SHAPE:
                    Debug.Log("Redo Shape Close");
                    // Shape 1 was just closed. Its final line was removed. Redraw this line.
                    if (CanvasState.Instance.shapeCount <= 1)
                    {
                        // shape 1 is the current shape
                        GameObject lastLine = ActionManager.Instance.UndoneLines.Pop();
                        lastLine.SetActive(true);
                        ShapeManager.Instance.CurrentLines.Add(lastLine);
                        ShapeManager.Instance.CurrentShape.IsClosed = true;
                        CanvasState.Instance.shapeCount++;
                        ShapeManager.Instance.PrevLines = ShapeManager.Instance.CurrentLines;
                        ShapeManager.Instance.CurrentLines = new List<GameObject>();
                        ShapeManager.Instance.AllShapes.Add(ShapeManager.Instance.CurrentShape);
                        ShapeManager.Instance.CurrentShape = new Shape();
                    }
                    break;

                case ActionManager.UserAction.GENERATE_SHAPE:
                    Debug.Log("Redo Shape Generate");
                    break;
            }
        }
    }
}
