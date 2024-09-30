using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    public void GenerateTriangle()
    {
        GenerateShape(3);
    }

    public void GenerateSquare()
    {
        GenerateShape(4);
    }

    public void GeneratePentagon()
    {
        GenerateShape(5);
    }

    public void GenerateHexagon()
    {
        GenerateShape(6);
    }

    public static void GenerateShape(int vertexCount)
    {
        if (CanvasState.Instance.shapeCount > CanvasState.MAX_SHAPE_COUNT)
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
        List<Vector3> vertices3D = GenerateConvexPolygon(vertexCount);

        Shape shape = new Shape(true);

        // Add points and instantiate prefabs
        foreach (var point in vertices3D)
        {
            GameObject newPrefab = Instantiate(Materials.Instance.PrefabShape1, point, Quaternion.identity);
            shape.AddPoint(point, newPrefab); // Add point and associated prefab to shape
        }

        AddLines(shape, vertices3D);

        Debug.Log($"Created shape with {vertexCount} vertices");

        // Add the generated shape GameObject to the list of all shapes
        ShapeManager.AllShapes.Add(shape);
        CanvasState.Instance.shapeCount++;

        ActionManager.ActionStack.Push(ActionManager.UserAction.GENERATE_SHAPE);
        ActionManager.canRedo = false;
    }

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

    // Add the LineRenderer to the shape object to draw the edges
    private static void AddLines(Shape shape, List<Vector3> vertices)
    {
        for (int i = 1; i < vertices.Count; i++)
        {
            ShapeRenderer.DrawLine(shape, vertices[i - 1], vertices[i]);
        }

        // add the last line to close the shape
        ShapeRenderer.DrawLine(shape, vertices[vertices.Count - 1], vertices[0]);
    }
}
