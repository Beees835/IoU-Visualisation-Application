using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteCurrShape()
    {
        if (ShapeManager.Instance.AllShapes.Count > 0)
        {
            //ShapeManager.Instance.AllShapes.RemoveAt(ShapeManager.Instance.AllShapes.Count - 1);
            ShapeManager.Instance.CurrentShape.Points.Clear();
            ShapeRenderer.Instance.ClearCurrentLines();
            CanvasState.Instance.drawState = CanvasState.DrawStates.DRAW_STATE;
            //CanvasState.Instance.shapeCount -= 1;
            //ShapeManager.Instance.CurrentShape = ShapeManager.Instance.AllShapes[ShapeManager.Instance.AllShapes.Count - 1];
        }
    }
}
