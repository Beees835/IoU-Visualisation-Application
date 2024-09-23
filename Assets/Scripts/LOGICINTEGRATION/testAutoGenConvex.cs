using System.Collections.Generic;
using UnityEngine;

public class RandomConvexPolygon : MonoBehaviour
{
    // Commented out variables related to old rendering logic
    // private List<Vector2> vertices = new List<Vector2>();
    // private List<ushort> triangles = new List<ushort>();
    // private GameObject polygon;
    public int vertexCount = 5; // Number of vertices for the convex polygon
    public GameObject PrefabShape1; // Assign via the Inspector
    // public Material LineMaterial; // Assign via the Inspector (Commented out if not used)

    void Start()
    {
        GenerateAndAddPolygon();
    }

    void GenerateAndAddPolygon()
    {
        // Generate convex polygon vertices
        List<Vector2> vertices2D = GenerateConvexPolygon();

        // Convert to Vector3
        List<Vector3> vertices3D = new List<Vector3>();
        foreach (var v in vertices2D)
        {
            vertices3D.Add(new Vector3(v.x, v.y, -1f));
        }

        // Create a new Shape
        Shape newShape = new Shape();
        newShape.IsClosed = true; // Since it's a complete polygon

        // Add points to the Shape
        foreach (var point in vertices3D)
        {
            // Instantiate a prefab at the point
            GameObject newPrefab = Instantiate(PrefabShape1, point, Quaternion.identity);

            // Add point and prefab to the shape
            newShape.AddPoint(point, newPrefab);
        }

        // Add the new shape to ShapeManager
        ShapeManager.Instance.AllShapes.Add(newShape);

        // Redraw all shapes
        ShapeRenderer.Instance.RedrawAllShapes();
    }

    List<Vector2> GenerateConvexPolygon()
    {
        // Commented out class-level variable; using local variable instead
        // vertices.Clear();
        List<Vector2> points = new List<Vector2>();

        // Generate random points
        for (int i = 0; i < vertexCount; i++)
        {
            points.Add(new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)));
        }

        // Sort vertices to form a convex hull
        // The sorting process allows the algorithm to process points in a systematic order
        // excluding any points that would cause the polygon to be non-convex.

        // vertices.Sort((a, b) => a.x.CompareTo(b.x)); // Commented out old code
        // Vector2[] sortedVertices = ConvexHull(vertices).ToArray();
        List<Vector2> convexHull = ConvexHull(points);

        // Update the vertices list with the convex hull vertices
        // It replaces the original list of vertices with those that form the convex hull
        // vertices = new List<Vector2>(convexHull); // Commented out; we're returning convexHull directly

        return convexHull;
    }

    // Convex hull algorithm using Andrew's monotone chain algorithm
    public static List<Vector2> ConvexHull(List<Vector2> points)
    {
        // A single point or empty list is already a convex hull so we can return safely
        if (points.Count <= 1)
            return points;

        // Sort points by x, then by y to prepare for hull construction
        points.Sort((a, b) => a.x == b.x ? a.y.CompareTo(b.y) : a.x.CompareTo(b.x));

        // Creating Lower hull
        List<Vector2> lower = new List<Vector2>();
        foreach (var p in points)
        {
            // Remove last point while it creates a non-left turn
            while (lower.Count >= 2 && Cross(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                lower.RemoveAt(lower.Count - 1);
            lower.Add(p);
        }

        // Creating Upper hull
        List<Vector2> upper = new List<Vector2>();
        for (int i = points.Count - 1; i >= 0; i--)
        {
            var p = points[i];
            while (upper.Count >= 2 && Cross(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                upper.RemoveAt(upper.Count - 1);
            upper.Add(p);
        }

        // Remove the last point of each half because they are repeated at the beginning of the other half
        lower.RemoveAt(lower.Count - 1);
        upper.RemoveAt(upper.Count - 1);

        // Concatenate lower and upper to get full hull
        lower.AddRange(upper);
        return lower;
    }

    // Calculate the cross product of OA and OB vectors
    // Positive result means counter-clockwise turn
    static float Cross(Vector2 O, Vector2 A, Vector2 B)
    {
        return (A.x - O.x) * (B.y - O.y) - (A.y - O.y) * (B.x - O.x);
    }

    // Commented out methods related to old rendering logic

    // Triangulation for convex polygon (fan triangulation)
    /*
    void Triangulate()
    {
        triangles.Clear();
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add((ushort)i);
            triangles.Add((ushort)(i + 1));
        }
    }
    */

    // Update the polygon's visual representation
    /*
    void UpdatePolygon()
    {
        // Ensure we have enough vertices to form a polygon
        if (vertices.Count < 3) return;

        // Allows component to be displayed
        SpriteRenderer sr = polygon.GetComponent<SpriteRenderer>();

        Texture2D texture = new Texture2D(1024, 1024);

        // Colouring for texture
        FillTexture(texture, Color.green);

        // Create a sprite from the texture. This sprite will be drawn at coordinates (0,0) with a size of 1024x1024
        // and a pivot point at the center (Vector2.zero), scaled at a 1:1 ratio
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, 1024, 1024), Vector2.zero, 1);

        // Array to hold the transformed vertices that are adjusted based on the polygon's position
        Vector2[] localVertices = new Vector2[vertices.Count];

        // Calculate the position offset for the polygon based on its vertices.
        // This method adjusts the vertices to be relative to the lower left corner of the bounding box containing
        // all vertices and returns the offset to be applied to the polygon
        Vector2 positionOffset = CalculateLocalVerticesAndOffset(localVertices);

        // Apply the local vertices to the sprite using the sprite's OverrideGeometry method. It updates the sprite to
        // match the shape defined by the vertices and triangles arrays
        sr.sprite.OverrideGeometry(localVertices, triangles.ToArray());

        // Adjust the position of the entire polygon GameObject to account for the calculated offset.
        polygon.transform.position = (Vector2)transform.position + positionOffset;
    }
    */

    // Fill the texture with a color
    /*
    void FillTexture(Texture2D texture, Color color)
    {
        Color[] colors = new Color[texture.width * texture.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        texture.SetPixels(colors);
        texture.Apply();
    }
    */

    // Calculate local vertices and position offset
    /*
    Vector2 CalculateLocalVerticesAndOffset(Vector2[] localVertices)
    {
        float lx = Mathf.Infinity, ly = Mathf.Infinity;
        // Find the lower left boundary of the vertices
        foreach (Vector2 vertex in vertices)
        {
            if (vertex.x < lx) lx = vertex.x;
            if (vertex.y < ly) ly = vertex.y;
        }
        // Adjust all vertices to be relative to the lower left corner
        for (int i = 0; i < vertices.Count; i++)
        {
            localVertices[i] = vertices[i] - new Vector2(lx, ly);
        }
        return new Vector2(lx, ly);
    }
    */
}
