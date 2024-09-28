using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ClearCanvasScript : MonoBehaviour
{

    [SerializeField] private Button _clearCanvasBtn;

    // Start is called before the first frame update
    void Start()
    {
        _clearCanvasBtn.onClick.AddListener(ClearCanvas);
        
    }

    void ClearCanvas() 
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        ShapeManager.Instance.AllShapes.Clear();
        ShapeManager.Instance.CurrentShape.Points.Clear();
        ShapeRenderer.Instance.ClearAllLines();
        CanvasState.Instance.shapeCount = 1;
        CanvasState.Instance.drawState = CanvasState.DrawStates.DRAW_STATE;
        Debug.Log("num shapes: " + ShapeManager.Instance.AllShapes.Count);
    }
        
}
