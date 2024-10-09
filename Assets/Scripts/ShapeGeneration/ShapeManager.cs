using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public static List<Shape> AllShapes { get; private set; } = new List<Shape>();
    public static Shape CurrentShape;
    public static Shape SelectedShape;
    public static List<GameObject> CurrentLines { get; set; } = new List<GameObject>();

    // storing deleted/undone things in case of redo
    public static List<GameObject> PrevLines { get; set; } = new List<GameObject>();


    public static void StartNewShape()
    {
        if (CurrentShape.Points.Count > 0)
        {
            CurrentShape.IsClosed = true;
            AllShapes.Add(CurrentShape);
            CanvasState.Instance.shapeCount++;
            CurrentShape = new Shape();
        }
    }

    public static void AddPointToCurrentShape(Vector3 point)
    {
        CurrentShape.AddPoint(point, true);
    }

    public static void DestroyShape(Shape shape)
    {

        foreach (var prefab in shape.RenderedPoints)
        {
            prefab.GetComponent<PointAnimation>().Close();
            Destroy(prefab);
        }
        shape.Points.Clear();
        shape.RenderedPoints.Clear();

        shape.ClearLines();
    }

    public static void DestroyAllShapes()
    {
        foreach (var shape in AllShapes)
        {
            DestroyShape(shape);
        }
        AllShapes.Clear();
        DestroyShape(CurrentShape);
        CurrentShape = new Shape();
    }

    public static void ClearLines()
    {
        foreach (Shape shape in AllShapes)
        {
            shape.ClearLines();
        }
        CurrentShape.ClearLines();
    }

    public static void ClearVertices()
    {
        foreach (Shape shape in AllShapes)
        {
            shape.ClearVertices();
        }
        CurrentShape.ClearVertices();
    }
}