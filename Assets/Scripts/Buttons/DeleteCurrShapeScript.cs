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

    public void DeleteCurrShape()
    {
        if (CanvasState.Instance.drawState == CanvasState.DrawStates.MODIFY_STATE)
        {
            Debug.Log("Can't delete shape after intersection");
            return;
        }

        if (ShapeManager.CurrentShape.Points.Count == 0)
        {
            ShapeManager.CurrentShape = ShapeManager.AllShapes[ShapeManager.AllShapes.Count - 1];
            ShapeManager.AllShapes.RemoveAt(ShapeManager.AllShapes.Count - 1);
            CanvasState.Instance.shapeCount--;
        }

        if (ShapeManager.AllShapes.Count > -1)
        {
            ShapeManager.DestroyShape(ShapeManager.CurrentShape);
        }
    }
}
