using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public static ShapeManager Instance { get; private set; }
    public List<Shape> AllShapes { get; private set; } = new List<Shape>();
    public Shape CurrentShape { get; set; } = new Shape();
    public List <GameObject> CurrentLines { get; set; } = new List<GameObject>();

    // storing deleted/undone things in case of redo
    public List <GameObject> PrevLines { get; set; } = new List<GameObject>();
    public Stack<Shape> PrevShapes { get; set; } = new Stack<Shape>();


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

    public void DeleteLastShape()
    {
        CurrentShape = AllShapes[AllShapes.Count - 1];
        foreach (var prefab in CurrentShape.Prefabs)
        {
            // remove points from screen
            prefab.SetActive(false);

        }

        foreach (var line in CurrentLines)
        {
            // remove lines from screen 
            Destroy(line);
        }
        
        // store deleted shape in case of redo
        PrevShapes.Push(CurrentShape);

        // actually remove shape

        AllShapes.Remove(CurrentShape);
        CurrentShape = new Shape();
        CanvasState.Instance.shapeCount--;

        // reassign current lines
        CurrentLines = PrevLines;
    }
}