using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public static ShapeManager Instance { get; private set; }
    public List<Shape> AllShapes { get; private set; } = new List<Shape>();
    public Shape CurrentShape { get; set; } = new Shape();
    public List <GameObject> CurrentLines { get; set; } = new List<GameObject>();
    public List <GameObject> PrevLines { get; set; } = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void StartNewShape()
    {
        if (CurrentShape.Points.Count > 0)
        {
            CurrentShape.IsClosed = true; 
            AllShapes.Add(CurrentShape);
            CurrentShape = new Shape();
        }
    }

    public void AddPointToCurrentShape(Vector3 point, GameObject prefab)
    {
        CurrentShape.AddPoint(point, prefab);
    }

    public void DeleteLastShape() {
        foreach (var prefab in CurrentShape.Prefabs)
        {
            // remove points from screen
            Destroy(prefab);
        }
        {
        foreach (var line in CurrentLines)
        {
            // remove lines from screen
            Destroy(line);
        }

        // reassign current shape
        if (CanvasState.Instance.shapeCount > 0)
        {
            CurrentShape = AllShapes[0];
            AllShapes.RemoveAt(AllShapes.Count - 1);
        } 
        else 
        {
            CurrentShape = new Shape();
        }
        CanvasState.Instance.shapeCount--;

        // reassign current lines
        CurrentLines = PrevLines;
        
    }
    }
}