using System.Collections.Generic;
using UnityEngine;

public class ShapeRenderer : MonoBehaviour
{
    public static float LineWidth = 0.05f;

    public static void DrawShape(Shape shape)
    {
        int pointCount = shape.Points.Count;

        if (pointCount < 2)
            return;

        for (int i = 0; i < pointCount - 1; i++)
        {
            Vector3 start = shape.Points[i];
            Vector3 end = shape.Points[i + 1];
            DrawLine(start, end);
        }

        if (shape.IsClosed)
        {
            // Draw closing line from last point to first point
            Vector3 start = shape.Points[pointCount - 1];
            Vector3 end = shape.Points[0];
            DrawLine(start, end);
            ShapeManager.PrevLines = ShapeManager.CurrentLines;
            ShapeManager.CurrentLines = new List<GameObject>();
        }
    }


    public static void RedrawAllShapes()
    {
        ClearAllLines();
        foreach (var shape in ShapeManager.AllShapes)
        {
            DrawShape(shape);
        }
        DrawShape(ShapeManager.CurrentShape);
    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
        //Debug.Log("IS DRAWING LINE");
        GameObject line = new GameObject("Line");
        line.tag = "Line";

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.material = Materials.Instance.LineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.startWidth = LineWidth;
        lineRenderer.endWidth = LineWidth;

        ShapeManager.CurrentLines.Add(line);
    }

    public static void ClearAllLines()
    {
        foreach (var line in GameObject.FindGameObjectsWithTag("Line"))
        {
            Destroy(line);
        }
    }

    public static void ClearCurrentLines()
    {
        foreach (var line in ShapeManager.CurrentLines)
        {
            // delete the line off the screen
            if (line != null) {
                line.SetActive(false);
            }
        }
        ShapeManager.CurrentLines.Clear();
    }
}
