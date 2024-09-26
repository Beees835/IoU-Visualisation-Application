using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public static ShapeManager Instance { get; private set; }

    public List<Shape> AllShapes { get; private set; } = new List<Shape>();
    public Shape CurrentShape { get; private set; } = new Shape();

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
}