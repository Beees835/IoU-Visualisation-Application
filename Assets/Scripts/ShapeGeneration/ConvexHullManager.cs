using System.Collections.Generic;
using UnityEngine;

public class ConvexHullManager : MonoBehaviour
{
    // Convex hull algorithm using Andrew's monotone chain algorithm
    public static List<Vector3> ConvexHull(List<Vector3> points)
    {
        // Remove duplicate points
        HashSet<Vector3> uniquePoints = new HashSet<Vector3>(points);
        points = new List<Vector3>(uniquePoints);

        // A single point or empty list is already a convex hull so we can return safely
        if (points.Count <= 1)
            return points;

        // Sort points by x, then by y to prepare for hull construction
        points.Sort((a, b) => a.x == b.x ? a.y.CompareTo(b.y) : a.x.CompareTo(b.x));

        // Creating Lower hull
        List<Vector3> lower = new List<Vector3>();
        foreach (var p in points)
        {
            // Remove last point while it creates a non-left turn
            while (lower.Count >= 2 && CrossProduct(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                lower.RemoveAt(lower.Count - 1);
            lower.Add(p);
        }

        // Creating Upper hull
        List<Vector3> upper = new List<Vector3>();
        for (int i = points.Count - 1; i >= 0; i--)
        {
            var p = points[i];
            while (upper.Count >= 2 && CrossProduct(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                upper.RemoveAt(upper.Count - 1);
            upper.Add(p);
        }

        // Remove the last point of each half because they are repeated at the beginning of the other half
        lower.RemoveAt(lower.Count - 1);
        upper.RemoveAt(upper.Count - 1);

        // Concatenate lower and upper to get full hull
        lower.AddRange(upper);

        // Ensure correct vertex count if needed
        if (lower.Count > 2)
        {
            lower = ReduceVertices(lower, points.Count);
        }

        return lower;
    }

    // Calculate the cross product of OA and OB vectors
    // Positive result means counter-clockwise turn
    private static float CrossProduct(Vector2 O, Vector2 A, Vector2 B)
    {
        return (A.x - O.x) * (B.y - O.y) - (A.y - O.y) * (B.x - O.x);
    }

    // Reduce the number of vertices in the convex hull to a desired count
    private static List<Vector3> ReduceVertices(List<Vector3> vertices, int desiredCount)
    {
        if (vertices.Count <= desiredCount) return vertices;

        // Simplify the hull by removing every nth point, trying to keep the shape close
        int removeIndex = 1;
        while (vertices.Count > desiredCount)
        {
            vertices.RemoveAt(removeIndex);
            removeIndex = (removeIndex + 1) % vertices.Count; // Update index cyclically
        }

        return vertices;
    }
}


