using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class containing all methods related to rendering shapes to canvas
/// </summary>
public class ShapeRenderer : MonoBehaviour
{
    /// <summary>
    /// Draw a shape to the canvas 
    /// </summary>
    /// <param name="shape">The shape to draw</param>
    public static void DrawShape(Shape shape)
    {
        DrawLines(shape);

        foreach (Vector3 point in shape.Points)
        {
            RenderPoint(shape, point);
        }
    }

    /// <summary>
    /// Remove all shapes from the canvas and redraw them
    /// </summary>
    public static void RedrawAllShapes()
    {
        ShapeManager.ClearAllShapesFromCanvas();
        foreach (var shape in ShapeManager.AllShapes)
        {
            DrawShape(shape);
        }
        DrawShape(ShapeManager.CurrentShape);
    }

    /// <summary>
    /// Render a point to canvas
    /// </summary>
    /// <param name="shape">The shape to assign the point to</param>
    /// <param name="point">The point to draw</param>
    /// <param name="fullAnimation">Whether the full animation should display</param>
    public static void RenderPoint(Shape shape, Vector3 point, bool fullAnimation = false)
    {
        GameObject vertex = Instantiate(shape.PrefabType, point, Quaternion.identity);

        if (fullAnimation)
        {
            vertex.GetComponent<PointAnimation>().Scale();
        }
        else
        {
            vertex.GetComponent<PointAnimation>().Instant();
        }
        shape.RenderedPoints.Add(vertex);
    }

    /// <summary>
    /// Draw a line to the canvas
    /// </summary>
    /// <param name="shape">Shape to assign the line to</param>
    /// <param name="start">Start point for the line</param>
    /// <param name="end">End point for the line</param>
    public static void DrawLine(Shape shape, Vector3 start, Vector3 end)
    {
        GameObject line = new GameObject("Line");
        line.tag = "Line";

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.material = Materials.Instance.LineMaterial;

        UnityEngine.Color color = shape.Selected ? new UnityEngine.Color(1.0f, 0.64f, 0.0f) : UnityEngine.Color.white;

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.startWidth = Materials.LINE_WIDTH;
        lineRenderer.endWidth = Materials.LINE_WIDTH;

        shape.Lines.Add(line);
    }

    /// <summary>
    /// Draw all lines for a shape
    /// </summary>
    /// <param name="shape">The shape to draw from</param>
    private static void DrawLines(Shape shape)
    {
        // Clear any existing lines before drawing new ones
        if (shape.Lines.Count > 0)
        {
            shape.ClearRenderedLines();
        }

        List<Vector3> points = shape.Points;

        // Loop through the points and draw a line between each consecutive pair of points
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 start = points[i];
            Vector3 end = points[i + 1];
            DrawLine(shape, start, end);  // Draw line between consecutive points
        }

        // If the shape is closed, connect the last point to the first point
        if (shape.IsClosed && points.Count > 1)
        {
            Vector3 start = points.Last();
            Vector3 end = points[0];
            DrawLine(shape, start, end);  // Draw the closing line
        }
    }

    /// <summary>
    /// Toggle the coordinate display
    /// </summary>
    public static void ToggleCoordinates()
    {
        CoordinateDisplay.ToggleAllCoordinates();
    }
}
