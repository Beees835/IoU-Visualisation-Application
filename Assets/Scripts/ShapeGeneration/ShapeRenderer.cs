using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeRenderer : MonoBehaviour
{

    public static void DrawShape(Shape shape)
    {
        DrawLines(shape);

        foreach (Vector3 point in shape.Points)
        {
            RenderPoint(shape, point);
        }
    }

    public static void RedrawAllShapes()
    {
        ShapeManager.ClearLines();
        ShapeManager.ClearVertices();
        foreach (var shape in ShapeManager.AllShapes)
        {
            DrawShape(shape);
        }
        DrawShape(ShapeManager.CurrentShape);
    }

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

    public static void DrawLines(Shape shape)
    {
        if (shape.Lines.Count > 0)
        {
            shape.ClearLines();
        }


        List<Vector3> points = shape.Points;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 start = points[i];
            Vector3 end = points[i + 1];
            DrawLine(shape, start, end);
        }

        if (shape.IsClosed)
        {
            // Draw closing line from last point to first point
            Vector3 start = points.Last();
            Vector3 end = points[0];
            DrawLine(shape, start, end);
        }
    }

    public static void RenderPoint(Shape shape, Vector3 point)
    {
        GameObject vertex = Instantiate(shape.PrefabType, point, Quaternion.identity);
        vertex.GetComponent<PointAnimation>().Instant();
        shape.RenderedPoints.Add(vertex);
    }

    public static void RenderNewPoint(Shape shape, Vector3 point)
    {
        GameObject vertex = Instantiate(shape.PrefabType, point, Quaternion.identity);
        vertex.GetComponent<PointAnimation>().Scale();
        shape.RenderedPoints.Add(vertex);
    }

    public static void ToggleCoordinates() 
    {
        CoordinateDisplay.ToggleAllCoordinates();
    }
}
