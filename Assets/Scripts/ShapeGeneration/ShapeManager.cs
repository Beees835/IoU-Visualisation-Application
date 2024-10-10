using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public static List<Shape> AllShapes { get; private set; } = new List<Shape>();
    public static Shape CurrentShape;
    public static Shape SelectedShape;

    public const int MAX_SHAPE_COUNT = 2;

    public static int GetShapeCount()
    {
        return AllShapes.Count;
    }

    public static bool CanAddMoreShapes()
    {
        return AllShapes.Count < MAX_SHAPE_COUNT;
    }

    public static void StartNewShape()
    {
        if (CurrentShape.Points.Count > 0)
        {
            CurrentShape.IsClosed = true;
            AllShapes.Add(CurrentShape);
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

    private static void DestroyAllShapes()
    {
        foreach (var shape in AllShapes)
        {
            DestroyShape(shape);
        }
        AllShapes.Clear();
        DestroyShape(CurrentShape);
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


    public static void Reset()
    {
        DestroyAllShapes();
        CurrentShape = new Shape();
        SelectedShape = null;
    }
}