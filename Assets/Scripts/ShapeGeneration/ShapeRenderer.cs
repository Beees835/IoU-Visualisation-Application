using UnityEngine;

public class ShapeRenderer : MonoBehaviour
{
    public static float LineWidth = 0.05f;

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
        //Debug.Log("IS DRAWING LINE");
        GameObject line = new GameObject("Line");
        line.tag = "Line";

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.material = Materials.Instance.LineMaterial;

        Color color = shape.Selected ? new Color(1.0f, 0.64f, 0.0f) : Color.white;

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.startWidth = LineWidth;
        lineRenderer.endWidth = LineWidth;

        ShapeManager.CurrentLines.Add(line);
        shape.Lines.Add(line);
    }

    public static void DrawLines(Shape shape)
    {
        if (shape.Lines.Count > 0)
        {
            shape.ClearLines();
        }


        int pointCount = shape.Points.Count;

        for (int i = 0; i < pointCount - 1; i++)
        {
            Vector3 start = shape.Points[i];
            Vector3 end = shape.Points[i + 1];
            DrawLine(shape, start, end);
        }

        if (shape.IsClosed)
        {
            // Draw closing line from last point to first point
            Vector3 start = shape.Points[pointCount - 1];
            Vector3 end = shape.Points[0];
            DrawLine(shape, start, end);
        }
    }

    public static void RenderPoint(Shape shape, Vector3 point)
    {
        GameObject vertex = Instantiate(shape.prefabType, point, Quaternion.identity);
        vertex.GetComponent<PointAnimation>().Instant();
        shape.RenderedPoints.Add(vertex);
    }

    public static void RenderNewPoint(Shape shape, Vector3 point)
    {
        GameObject vertex = Instantiate(shape.prefabType, point, Quaternion.identity);
        vertex.GetComponent<PointAnimation>().Scale();
        shape.RenderedPoints.Add(vertex);
    }
}
