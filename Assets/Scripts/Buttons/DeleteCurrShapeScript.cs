using UnityEngine;
using UnityEngine.UI;

public class DeleteCurrShapeScript : MonoBehaviour
{

    [SerializeField] private Button _deleteShapeBtn;

    // Start is called before the first frame update
    void Start()
    {
        _deleteShapeBtn.onClick.AddListener(DeleteShape);
    }

    public static void DeleteShape()
    {
        if (ShapeManager.AllShapes.Count < ShapeManager.MAX_SHAPE_COUNT)
        {
            NotificationManager.Instance.ShowMessage("Define all shapes first before attempting deletion");
            return;
        }


        if (ShapeManager.SelectedShape == null)
        {
            NotificationManager.Instance.ShowMessage("Select a shape to delete. Try double clicking a vertex!");
            return;
        }

        IoUCalculator.Reset();

        Shape deletionShape = ShapeManager.SelectedShape;
        ShapeManager.SelectedShape = null;

        // Data for Undo/Redo
        ActionManager.ShapeSizeStack.Push(deletionShape.Points.Count);

        // Reverse list to ensure points come out in right order when popped
        deletionShape.Points.Reverse();
        foreach (var point in deletionShape.Points)
        {
            ActionManager.PointStack.Push(point);
        }


        // Remove from AllShapes
        ShapeManager.AllShapes.Remove(deletionShape);


        ShapeManager.DestroyShape(deletionShape);
        ShapeManager.CurrentShape = new Shape();

        ActionManager.ActionStack.Push(ActionManager.UserAction.DELETE_SHAPE);
        ActionManager.canRedo = false;
    }
}
