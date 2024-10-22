using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Data storage class for shapes
/// Stores both the abstract 'points' and the rendered lines and points
/// </summary>
public class Shape
{
    public GameObject PrefabType;
    public List<Vector3> Points { get; set; } = new List<Vector3>();
    public List<GameObject> RenderedPoints { get; set; } = new List<GameObject>();
    public List<GameObject> Lines { get; set; } = new List<GameObject>();
    public bool IsClosed { get; set; } = false;
    public bool Selected = false;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="isClosed">Whether the shape should be set as closed</param>
    public Shape(bool isClosed = false)
    {
        PrefabType = Materials.GetPrefabType();
        IsClosed = isClosed;
    }

    /// <summary>
    /// Add a point to the shape
    /// </summary>
    /// <param name="point">The point to add</param>
    /// <param name="animated">Whether to play an animation upon adding the point</param>
    public void AddPoint(Vector3 point, bool animated = false)
    {
        Points.Add(point);

        if (animated)
        {
            ShapeRenderer.RenderPoint(this, point, true);
        }
        else
        {
            ShapeRenderer.RenderPoint(this, point);
        }
    }

    /// <summary>
    /// Remove the last point added to a shape
    /// </summary>
    /// <returns>The vertex of the point</returns>
    public Vector3 RemoveLastPoint()
    {
        Vector3 point = Points.Last();
        GameObject prefab = RenderedPoints.Last();

        RenderedPoints.RemoveAt(RenderedPoints.Count - 1);
        Points.RemoveAt(Points.Count - 1);

        prefab.GetComponent<PointAnimation>().Close();
        Object.Destroy(prefab);
        return point;
    }

    /// <summary>
    /// Remove the last line added to a shape
    /// </summary>
    /// <returns>The vertex the line ended with</returns>
    public Vector3 RemoveLastLine()
    {
        Vector3 point = Points.Last();
        GameObject line = Lines.Last();
        Lines.RemoveAt(Lines.Count - 1);
        Object.Destroy(line);
        return point;
    }

    /// <summary>
    /// Check if a set of points returns a convex shape
    /// </summary>
    /// <param name="points">The set of points to testr</param>
    /// <returns>Whether the shape will be convex</returns>
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
                continue;
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

    /// <summary>
    /// Check if a shape is convex with a new point added
    /// </summary>
    /// <param name="newPoint"> The point being added</param>
    /// <returns>Whether the shape will be convex</returns>
    public bool IsConvexWithNewPoint(Vector3 newPoint)
    {
        List<Vector3> testPoints = new List<Vector3>(Points) { newPoint };
        return IsConvex(testPoints);
    }

    /// <summary>
    /// Remove all rendered lines from the current shape, clearing them from the canvas
    /// </summary>
    public void ClearRenderedLines()
    {
        foreach (var line in Lines)
        {
            GameObject.Destroy(line);
        }
        Lines.Clear();
    }

    /// <summary>
    /// Remove all rendered vertices from the current shape, clearing them from the canvas
    /// </summary>
    public void ClearRenderedVertices()
    {
        foreach (GameObject point in RenderedPoints)
        {
            GameObject.Destroy(point);
        }
        RenderedPoints.Clear();
    }

    /// <summary>
    /// Remove all lines and vertices and clear them from the canvas
    /// </summary>
    public void ClearShape()
    {
        ClearRenderedLines();
        ClearRenderedVertices();
    }
}
