using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public GameObject PrefabType;
    public List<Vector3> Points { get; set; } = new List<Vector3>();
    public List<GameObject> RenderedPoints { get; set; } = new List<GameObject>();
    public List<GameObject> Lines { get; set; } = new List<GameObject>();
    public Stack<Vector3> PrevPoints { get; set; } = new Stack<Vector3>();
    public bool IsClosed { get; set; } = false;
    public bool Selected = false;

    public Shape(bool isClosed = false)
    {
        PrefabType = Materials.GetPrefabType();
        IsClosed = isClosed;
    }

    public void AddPoint(Vector3 point, bool animated = false)
    {
        Points.Add(point);

        if (animated)
        {
            ShapeRenderer.RenderNewPoint(this, point);
        }
        else
        {
            ShapeRenderer.RenderPoint(this, point);
        }
    }

    public Vector3 RemoveLastPoint()
    {
        Vector3 point = Points[Points.Count - 1];
        GameObject prefab = RenderedPoints[RenderedPoints.Count - 1];

        RenderedPoints.RemoveAt(RenderedPoints.Count - 1);
        Points.RemoveAt(Points.Count - 1);

        prefab.GetComponent<PointAnimation>().Close();
        Object.Destroy(prefab);
        return point;
    }

    public Vector3 RemoveLastLine()
    {
        Vector3 point = Points[Points.Count - 1];
        GameObject line = Lines[Lines.Count - 1];
        Lines.RemoveAt(Lines.Count - 1);
        Object.Destroy(line);
        return point;
    }

    // General convexity check with custom points
    public static bool IsConvex(List<Vector3> points)
    {
        if (points.Count < 3)
        {
            return true;
        }

        bool? isConvex = null;
        int n = points.Count;

        for (int i = 0; i < n; i++)
        {
            Vector3 a = points[i];
            Vector3 b = points[(i + 1) % n];
            Vector3 c = points[(i + 2) % n];

            Vector3 ab = b - a;
            Vector3 bc = c - b;

            float crossProductZ = ab.x * bc.y - ab.y * bc.x;

            if (crossProductZ == 0)
            {
                continue; // Colinear points; skip
            }

            if (isConvex == null)
            {
                isConvex = crossProductZ > 0;
            }
            else if ((crossProductZ > 0) != isConvex.Value)
            {
                return false;
            }
        }

        return true;
    }

    // Update existing methods to use the new IsConvex method
    public bool IsConvex()
    {
        return IsConvex(Points);
    }

    public bool IsConvexWithNewPoint(Vector3 newPoint)
    {
        List<Vector3> testPoints = new List<Vector3>(Points) { newPoint };
        return IsConvex(testPoints);
    }


    public void ClearLines()
    {
        foreach (var line in Lines)
        {
            // delete the line off the screen
            GameObject.Destroy(line);
        }
        Lines.Clear();
    }

    public void ClearVertices()
    {
        foreach (GameObject point in RenderedPoints)
        {
            GameObject.Destroy(point);
        }
        RenderedPoints.Clear();
    }

    public void ClearShape()
    {
        ClearLines();
        ClearVertices();
    }
}