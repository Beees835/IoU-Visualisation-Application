using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnFunctions : MonoBehaviour
{
    
    //Structure goes something like this
    public void LockShape()
    {
        if (CanvasState.Instance.shapeCount == 1) {
            CanvasState.Instance.shapeCount = 2;
        }
        else if (CanvasState.Instance.shapeCount == 2) {
            CanvasState.Instance.shapeCount = 0;
            CanvasState.Instance.beginCalculatingIoU();
        } 
    }

    public void Reset() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}