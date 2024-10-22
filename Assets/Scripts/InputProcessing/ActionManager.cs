using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the management of actions within undo/redo
/// </summary>
public class ActionManager : MonoBehaviour
{
    /// <summary>
    /// Actions that may be partaken within the app
    /// </summary>
    public enum UserAction
    {
        DRAW_POINT,
        DRAW_LINE,
        CLOSE_SHAPE,
        GENERATE_SHAPE,
        DELETE_SHAPE
    }
    public static Stack<UserAction> ActionStack = new Stack<UserAction>();
    public static Stack<UserAction> RedoStack = new Stack<UserAction>();
    public static bool canRedo = false;

    // Points and Shape Sizes that have been undone
    public static Stack<Vector3> PointStack = new Stack<Vector3>();
    public static Stack<int> ShapeSizeStack = new Stack<int>();

    /// <summary>
    /// Build a shape from the shapestack
    /// </summary>
    /// <returns>The shape that was built</returns>
    public static Shape BuildShapeFromStack()
    {
        Shape shape = new Shape();
        int shapeSize = ShapeSizeStack.Pop();
        Debug.Log(shapeSize);


        for (int i = 0; i < shapeSize; i++)
        {
            Vector3 startPoint = PointStack.Pop();
            shape.AddPoint(startPoint);
        }

        Debug.Log(shape.Points.Count);
        return shape;
    }

    /// <summary>
    /// Reset the manager
    /// </summary>
    public static void Reset()
    {
        ActionStack.Clear();
        RedoStack.Clear();
        PointStack.Clear();
        ShapeSizeStack.Clear();
    }
}