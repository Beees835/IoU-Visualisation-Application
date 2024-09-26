using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeRenderer : MonoBehaviour
{
    public static ShapeRenderer Instance { get; private set; }

    public Material LineMaterial;
    public float LineWidth = 0.05f;

    private void Awake()
    {
        Instance = this;
    }

    public void DrawShape(Shape shape)
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
        }
    }


    public void RedrawAllShapes()
    {
        ClearAllLines();
        foreach (var shape in ShapeManager.Instance.AllShapes)
        {
            DrawShape(shape);
        }
        DrawShape(ShapeManager.Instance.CurrentShape);
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        Debug.Log("IS DRAWING LINE");
        GameObject line = new GameObject("Line");
        line.tag = "Line";

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();

        lineRenderer.material = LineMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.startWidth = LineWidth;
        lineRenderer.endWidth = LineWidth;
    }

    public void ClearAllLines()
    {
        foreach (var line in GameObject.FindGameObjectsWithTag("Line"))
        {
            Destroy(line);
        }
    }

    public void ClearAllPoints()
    {
        foreach (var point in GameObject.FindGameObjectsWithTag("Point"))
        {
            Destroy(point);
        }
    }   
}
