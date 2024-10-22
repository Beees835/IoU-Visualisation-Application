using UnityEngine;
using UnityEngine.UI;

public class DeleteCurrShapeScript : MonoBehaviour
{
    [SerializeField] private Button _deleteShapeBtn;

    void Start()
    {
        _deleteShapeBtn.onClick.AddListener(DeleteShape);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace)) DeleteShape();
    }

    /// <summary>
    /// Delete the currently selected shape
    /// </summary>
    void DeleteShape()
    {
        if (ShapeManager.CanAddMoreShapes())
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

        // Remove shape from canvas
        ShapeManager.AllShapes.Remove(deletionShape);
        deletionShape.ClearShape();

        ShapeManager.CurrentShape = new Shape();

        ActionManager.ActionStack.Push(ActionManager.UserAction.DELETE_SHAPE);
        ActionManager.canRedo = false;
    }
}
