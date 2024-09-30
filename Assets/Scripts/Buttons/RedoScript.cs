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
        if (ActionManager.RedoStack.Count > 0 && ActionManager.canRedo)
        {
            ActionManager.UserAction Action = ActionManager.RedoStack.Peek();
            GameObject prefabType = CanvasState.Instance.shapeCount == 0 ? Materials.Instance.PrefabShape1 : Materials.Instance.PrefabShape2;

            GameObject prefab;

            Vector3 startPoint;
            Vector3 endPoint;

            switch (Action)
            {
                case ActionManager.UserAction.DRAW_POINT:
                    Debug.Log("Redo Point Draw");

                    // Came from a full previous shape
                    if (ShapeManager.CurrentShape.IsClosed)
                    {
                        ShapeManager.CurrentShape = new Shape();
                    }

                    startPoint = ActionManager.PointStack.Pop();
                    prefab = Instantiate(prefabType, startPoint, Quaternion.identity);
                    ShapeManager.CurrentShape.AddPoint(startPoint, prefab);
                    break;

                case ActionManager.UserAction.DRAW_LINE:
                    Debug.Log("Redo Line Draw");

                    startPoint = ActionManager.PointStack.Pop();
                    endPoint = ActionManager.PointStack.Pop();

                    prefab = Instantiate(prefabType, endPoint, Quaternion.identity);
                    ShapeManager.AddPointToCurrentShape(endPoint, prefab);
                    ShapeRenderer.DrawLine(ShapeManager.CurrentShape, startPoint, endPoint);

                    break;

                case ActionManager.UserAction.CLOSE_SHAPE:
                    Debug.Log("Redo Shape Close");

                    startPoint = ActionManager.PointStack.Pop();
                    endPoint = ShapeManager.CurrentShape.Points[0];
                    ShapeRenderer.DrawLine(ShapeManager.CurrentShape, startPoint, endPoint);

                    ShapeManager.StartNewShape();
                    break;

                case ActionManager.UserAction.GENERATE_SHAPE:
                    Debug.Log("Redo Shape Generate");

                    int shapeSize = ActionManager.ShapeSizeStack.Pop();

                    ShapeManager.CurrentShape = new Shape();
                    for (int i = 0; i < shapeSize; i++)
                    {
                        startPoint = ActionManager.PointStack.Pop();
                        prefab = Instantiate(prefabType, startPoint, Quaternion.identity);
                        ShapeManager.AddPointToCurrentShape(startPoint, prefab);
                    }
                    ShapeManager.StartNewShape();
                    ShapeRenderer.RedrawAllShapes();
                    break;
            }

            ActionManager.RedoStack.Pop();
            ActionManager.ActionStack.Push(Action);
        }
    }
}
