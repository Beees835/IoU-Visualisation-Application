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

    // Trigger DeleteShape if Delete or Backspace key is pressed 
    void Update()
    {
        if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace))
        {
            DeleteShape();
        }
    }


    public static void DeleteShape()
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


        // Remove from AllShapes
        ShapeManager.AllShapes.Remove(deletionShape);

        deletionShape.ClearShape();
        ShapeManager.CurrentShape = new Shape();

        ActionManager.ActionStack.Push(ActionManager.UserAction.DELETE_SHAPE);
        ActionManager.canRedo = false;
    }
}
