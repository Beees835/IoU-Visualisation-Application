using System.Collections.Generic;
using System.Linq;
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

    public static ShapeData[] GetShapesData()
    {
        List<ShapeData> shapesData = new List<ShapeData>();

        // Save all finished shapes
        foreach (Shape shape in AllShapes)
        {
            ShapeData data = new ShapeData
            {
                points = shape.Points,
                isClosed = shape.IsClosed,
                prefabType = shape.PrefabType.name
            };
            shapesData.Add(data);
        }

        // Save the current unfinished shape separately if it exists
        if (CurrentShape != null && CurrentShape.Points.Count > 0 && !CurrentShape.IsClosed)
        {
            ShapeData currentShapeData = new ShapeData
            {
                points = CurrentShape.Points,
                isClosed = false,  // Mark as unfinished
                prefabType = CurrentShape.PrefabType.name
            };
            shapesData.Add(currentShapeData);
        }

        return shapesData.ToArray();
    }

    public static void LoadShapes(ShapeData[] shapesData)
    {
        Reset();
        IoUCalculator.Reset();

        for (int i = 0; i < shapesData.Length; i++)
        {
            ShapeData data = shapesData[i];
            Shape newShape = new Shape(data.isClosed)
            {
                PrefabType = Materials.GetPrefabType()
            };

            // Recreate the shape by adding each point
            foreach (var point in data.points)
            {
                newShape.AddPoint(point, false);
            }

            ShapeRenderer.DrawLines(newShape);

            // If the shape is closed, add it to the list of finished shapes
            if (newShape.IsClosed)
            {
                AllShapes.Add(newShape);
            }
            else if (i == shapesData.Length - 1 && !data.isClosed)
            {
                CurrentShape = newShape;
            }
        }
    }
}
