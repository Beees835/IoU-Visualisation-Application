using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedoScript : MonoBehaviour
{

    [SerializeField] private Button _redoBtn;

    // Start is called before the first frame update
    void Start()
    {
        _redoBtn.onClick.AddListener(Redo);
    }

    public void Redo()
    {
        if (ActionManager.Instance.RedoStack.Count > 0)
        {
            ActionManager.UserAction Action = ActionManager.Instance.RedoStack.Peek();
            GameObject pfType;
            pfType = CanvasState.Instance.shapeCount == 0 ? Materials.Instance.PrefabShape1 : Materials.Instance.PrefabShape2;

            switch (Action)
            {
                case ActionManager.UserAction.DRAW_POINT:
                    Debug.Log("Redo Point Draw");

                    Shape newCurrShape = ShapeManager.PrevShapes.Peek();

                    Vector3 point = newCurrShape.PrevPoints.Pop();
                    newCurrShape.Points.Add(point);


                    GameObject newPrefab = Instantiate(pfType, point, Quaternion.identity);
                    ShapeManager.CurrentShape.Prefabs.Add(newPrefab);

                    ShapeManager.CurrentShape = newCurrShape;
                    break;

                case ActionManager.UserAction.DRAW_LINE:
                    Debug.Log("Redo Line Draw");
                    // A line was just drawn and then removed. Redraw this line.
                    GameObject line = ActionManager.Instance.UndoneLines.Pop();
                    line.SetActive(true);
                    ShapeManager.CurrentLines.Add(line);

                    // add the point back
                    Vector3 point1 = ShapeManager.CurrentShape.PrevPoints.Pop();
                    ShapeManager.CurrentShape.Points.Add(point1);
                    GameObject newPf = Instantiate(pfType, point1, Quaternion.identity);
                    ShapeManager.CurrentShape.Prefabs.Add(newPf);

                    break;

                case ActionManager.UserAction.CLOSE_SHAPE:
                    Debug.Log("Redo Shape Close");
                    // Shape 1 was just closed. Its final line was removed. Redraw this line.
                    if (CanvasState.Instance.shapeCount <= 1)
                    {
                        // shape 1 is the current shape
                        GameObject lastLine = ActionManager.Instance.UndoneLines.Pop();
                        lastLine.SetActive(true);
                        ShapeManager.CurrentLines.Add(lastLine);
                        ShapeManager.CurrentShape.IsClosed = true;
                        CanvasState.Instance.shapeCount++;
                        ShapeManager.PrevLines = ShapeManager.CurrentLines;
                        ShapeManager.CurrentLines = new List<GameObject>();
                        ShapeManager.AllShapes.Add(ShapeManager.CurrentShape);
                        ShapeManager.CurrentShape = new Shape();
                    }
                    break;

                case ActionManager.UserAction.GENERATE_SHAPE:
                    Debug.Log("Redo Shape Generate");
                    // put the randomly generated shape back on the screen
                    Shape deletedShape = ShapeManager.PrevShapes.Pop();
                    ShapeManager.AllShapes.Add(deletedShape);
                    CanvasState.Instance.shapeCount++;

                    // redraw the points 
                    foreach (var prefab in deletedShape.Prefabs)
                    {
                        prefab.SetActive(true);
                    }
                    // redraw lines
                    ShapeRenderer.RedrawAllShapes();
                    break;
            }
            ActionManager.Instance.RedoStack.Pop();
            ActionManager.Instance.ActionStack.Push(Action);
        }
    }
}
