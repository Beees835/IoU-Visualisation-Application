using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public static List<Shape> AllShapes { get; private set; } = new List<Shape>();
    public static Shape CurrentShape { get; set; } = new Shape();
    public static List<GameObject> CurrentLines { get; set; } = new List<GameObject>();

    // storing deleted/undone things in case of redo
    public static List<GameObject> PrevLines { get; set; } = new List<GameObject>();
    public static Stack<Shape> PrevShapes { get; set; } = new Stack<Shape>();


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

    public static void AddPointToCurrentShape(Vector3 point, GameObject prefab)
    {
        CurrentShape.AddPoint(point, prefab);
    }

    public static void DestroyShape(Shape shape)
    {

        foreach (var prefab in shape.Prefabs)
        {
            prefab.GetComponent<PointAnimation>().Close();
            Destroy(prefab);
        }

        foreach (var line in shape.Lines)
        {
            Destroy(line);
        }

        shape.Points.Clear();
        shape.Lines.Clear();
        shape.Prefabs.Clear();
    }

    public static void DestroyShapes()
    {
        foreach (var shape in AllShapes)
        {
            DestroyShape(shape);

        }
        AllShapes.Clear();
        CurrentShape = new Shape();
    }

    public static void ClearLines()
    {
        foreach (var line in CurrentLines)
        {
            // delete the line off the screen
            Destroy(line);
        }
        foreach (var line in PrevLines)
        {
            // delete the line off the screen
            Destroy(line);
        }

        CurrentLines.Clear();
        PrevLines.Clear();
    }
}