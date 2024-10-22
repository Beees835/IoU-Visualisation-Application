using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing methods for random shape generation
/// </summary>
public class ShapeGenerator : MonoBehaviour
{
    /// <summary>
    /// Generate an irregular triangle
    /// </summary>
    public void GenerateTriangle()
    {
        GenerateShape(3);
    }

    /// <summary>
    /// Generate an irregular square
    /// </summary>
    public void GenerateSquare()
    {
        GenerateShape(4);
    }

    /// <summary>
    /// Generate an irregular pentagon
    /// </summary>
    public void GeneratePentagon()
    {
        GenerateShape(5);
    }

    /// <summary>
    /// Generate an irregular hexagon
    /// </summary>
    public void GenerateHexagon()
    {
        GenerateShape(6);
    }

    /// <summary>
    /// Generate an irregular convex shape with the given number of vertices
    /// </summary>
    /// <param name="vertexCount">The number of vertices the shape should have</param>
    public static void GenerateShape(int vertexCount)
    {
        if (!ShapeManager.CanAddMoreShapes())
        {
            Debug.Log("Too many shapes already");
            NotificationManager.Instance.ShowMessage("Too many shapes already");
            return;
        }

        if (ShapeManager.CurrentShape.Points.Count > 0)
        {
            Debug.Log("Can't generate a shape until the current one is finished");
            NotificationManager.Instance.ShowMessage("Can't generate a shape until the current one is finished");
            return;
        }


        // Generate convex polygon vertices using ConvexHullManager
        List<Vector3> vertices = GenerateConvexPolygon(vertexCount);

        Shape shape = new Shape(true);

        foreach (var point in vertices)
        {
            shape.Points.Add(point);
        }

        ShapeRenderer.DrawShape(shape);

        Debug.Log($"Created shape with {vertexCount} vertices");

        // Add the generated shape GameObject to the list of all shapes
        ShapeManager.AllShapes.Add(shape);

        ActionManager.ActionStack.Push(ActionManager.UserAction.GENERATE_SHAPE);
        ActionManager.canRedo = false;
    }

    /// <summary>
    /// Generate a series of points defining a convex shape of 'n' vertices
    /// </summary>
    /// <param name="vertexCount">The number of vertices the shape should have</param>
    private static List<Vector3> GenerateConvexPolygon(int vertexCount)
    {
        List<Vector3> points = new List<Vector3>();
        int extraPoints = 5; // Adding extra points to ensure sufficient points for a good hull

        // Define the canvas size 
        float canvasWidth = 10.0f;
        float canvasHeight = 10.0f;

        // Calculate 8% margin 
        float marginX = canvasWidth * 0.08f;
        float marginY = canvasHeight * 0.08f;

        // Calculate the canvas limits with margin included
        float xMin = -canvasWidth / 2 + marginX;
        float xMax = canvasWidth / 2 - marginX;
        float yMin = -canvasHeight / 2 + marginY;
        float yMax = canvasHeight / 2 - marginY;

        // Generate random points within the canvas bounds
        for (int i = 0; i < vertexCount + extraPoints; i++)
        {
            points.Add(new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -1f));
        }

        // Use ConvexHullManager to generate the convex hull
        List<Vector3> convexHull = ConvexHullManager.ConvexHull(points);

        // If the convex hull has fewer points than desired, regenerate
        if (convexHull.Count < vertexCount)
        {
            Debug.LogWarning("Regenerating points to match the desired vertex count.");
            return GenerateConvexPolygon(vertexCount);
        }

        // Return only the desired number of points
        return convexHull.GetRange(0, Mathf.Min(vertexCount, convexHull.Count));
    }
}
