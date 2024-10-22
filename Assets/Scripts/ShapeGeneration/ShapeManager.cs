using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for managing shape related details
/// </summary>
public class ShapeManager : MonoBehaviour
{
    public static List<Shape> AllShapes { get; private set; } = new List<Shape>();
    public static Shape CurrentShape;
    public static Shape SelectedShape;

    public const int MAX_SHAPE_COUNT = 2;

    /// <summary>
    /// Return the number of shapes on the canvas
    /// </summary>
    /// <returns>The number of shapes on the canvas</returns>
    public static int GetShapeCount()
    {
        return AllShapes.Count;
    }

    /// <summary>
    /// Determine whether more shapes may be drawn
    /// </summary>
    /// <returns>Whether more shapes can be drawn</returns>
    public static bool CanAddMoreShapes()
    {
        return AllShapes.Count < MAX_SHAPE_COUNT;
    }

    /// <summary>
    /// Start a new shape. Adding the previous to the shape list
    /// </summary>
    public static void StartNewShape()
    {
        if (CurrentShape.Points.Count > 0)
        {
            CurrentShape.IsClosed = true;
            AllShapes.Add(CurrentShape);
            CurrentShape = new Shape();
        }
    }

    /// <summary>
    /// Add a point to the current shape
    /// </summary>
    /// <param name="point">The point to add</param>
    public static void AddPointToCurrentShape(Vector3 point)
    {
        CurrentShape.AddPoint(point, true);
    }

    /// <summary>
    /// Clear all shapes from the canvas
    /// </summary>
    public static void ClearAllShapesFromCanvas()
    {
        foreach (var shape in AllShapes)
        {
            shape.ClearShape();
        }
        CurrentShape.ClearShape();
    }

    /// <summary>
    /// Reset the shape manager
    /// </summary>
    public static void Reset()
    {
        ClearAllShapesFromCanvas();
        AllShapes.Clear();
        CurrentShape = new Shape();
        SelectedShape = null;
    }

    /// <summary>
    /// Get the shape data from the shape manager
    /// </summary>
    /// <returns>All shape data</returns>
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

    /// <summary>
    /// Load shapes from shape data
    /// </summary>
    /// <param name="shapesData">The shape data to load</param>
    public static void LoadShapes(ShapeData[] shapesData)
    {
        CanvasState.Instance.ClearCanvas();

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
                newShape.Points.Add(point);
            }

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
        ShapeRenderer.RedrawAllShapes();
    }
}
