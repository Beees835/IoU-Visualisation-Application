using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public List<Vector3> Points { get; set; } = new List<Vector3>();
    public List<GameObject> Prefabs { get; set; } = new List<GameObject>();
    public Stack<Vector3> PrevPoints { get; set; } = new Stack<Vector3>();
    public bool IsClosed { get; set; } = false;

    public Shape() { }

    public void AddPoint(Vector3 point, GameObject prefab)
    {
        Points.Add(point);
        Prefabs.Add(prefab);
    }

    public void RemoveLastPoint()
    {
        if (Points.Count > 0)
        {
            // store last point in case of redo
            Vector3 lastPoint = Points[Points.Count - 1];
            PrevPoints.Push(lastPoint);

            // actually remove last point 
            Points.RemoveAt(Points.Count - 1);
            GameObject lastPrefab = Prefabs[Prefabs.Count - 1];
            Prefabs.RemoveAt(Prefabs.Count - 1);
            GameObject.Destroy(lastPrefab);
        }
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
}