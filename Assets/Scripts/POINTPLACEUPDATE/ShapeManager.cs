using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public static ShapeManager Instance { get; private set; }
    public List<Shape> AllShapes { get; private set; } = new List<Shape>();
    public Shape CurrentShape { get; set; } = new Shape();
    public List<GameObject> CurrentLines { get; set; } = new List<GameObject>();
    public List<GameObject> PrevLines { get; set; } = new List<GameObject>();

    private const int MaxShapes = 2; // Global maximum number of shapes

    // Add the IsDrawingAllowed property
    public bool IsDrawingAllowed { get; set; } = true;

    // New variable to prevent immediate new point registration
    public float newShapeCooldown = 0.2f; // 200ms cooldown
    public float lastShapeEndTime;

    private void Awake()
    {
        Instance = this;
    }
    
    public bool CanCreateShape()
    {
        return AllShapes.Count < MaxShapes;
    }

    public void AddShape(Shape newShape)
    {
        if (newShape != null && CanCreateShape())
        {
            AllShapes.Add(newShape);
        }
        else
        {
            Debug.LogWarning("Cannot add more shapes. Maximum limit reached.");
        }
    }

    public void StartNewShape()
    {
        if (CurrentShape.Points.Count > 0 && CanCreateShape())
        {
            CurrentShape.IsClosed = true;
            AddShape(CurrentShape);
        }

        // Reset current shape and clear lines
        CurrentShape = new Shape();
        CurrentLines.Clear();
        PrevLines.Clear();

        // Record the time when the shape was closed
        lastShapeEndTime = Time.time;
    }

    public void AddPointToCurrentShape(Vector3 point, GameObject prefab)
    {
        // Check if the cooldown has passed since the last shape was closed
        if (Time.time - lastShapeEndTime > newShapeCooldown)
        {
            CurrentShape.AddPoint(point, prefab);
        }
    }
}