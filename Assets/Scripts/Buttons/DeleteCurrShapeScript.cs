using UnityEngine;
using UnityEngine.UI;

public class DeleteCurrShapeScript : MonoBehaviour
{

    [SerializeField] private Button _deleteCurrShapeBtn;

    // Start is called before the first frame update
    void Start()
    {
        _deleteCurrShapeBtn.onClick.AddListener(DeleteCurrShape);
    }

    public static void DeleteCurrShape()
    {
        if (CanvasState.Instance.drawState == CanvasState.DrawStates.MODIFY_STATE)
        {
            //NotificationManager.Instance.ShowMessage("Can't delete shape after intersection has been calculated.");
            //Debug.Log("Can't delete shape after intersection");

            // Remove Intersection Shape
            IoUCalculator.Reset();
        }

        if (ShapeManager.CurrentShape.Points.Count == 0)
        {
            ShapeManager.CurrentShape = ShapeManager.AllShapes[ShapeManager.AllShapes.Count - 1];
            ShapeManager.AllShapes.RemoveAt(ShapeManager.AllShapes.Count - 1);
            CanvasState.Instance.shapeCount--;
        }

        // Data for Undo/Redo
        ActionManager.DeleteCompletion.Push(ShapeManager.CurrentShape.IsClosed);
        ActionManager.ShapeSizeStack.Push(ShapeManager.CurrentShape.Points.Count);

        // Reverse list to ensure points come out in right order when popped
        ShapeManager.CurrentShape.Points.Reverse();
        foreach (var point in ShapeManager.CurrentShape.Points)
        {
            ActionManager.PointStack.Push(point);
        }

        if (ShapeManager.AllShapes.Count > -1)
        {
            ShapeManager.DestroyShape(ShapeManager.CurrentShape);
        }

        ShapeManager.CurrentShape.IsClosed = false;

        ActionManager.ActionStack.Push(ActionManager.UserAction.DELETE_SHAPE);
        ActionManager.canRedo = false;
    }
}
