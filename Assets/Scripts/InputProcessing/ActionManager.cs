using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    public enum UserAction
    {
        DRAW_POINT,
        DRAW_LINE,
        CLOSE_SHAPE,
        GENERATE_SHAPE
    }
    public static Stack<UserAction> ActionStack = new Stack<UserAction>();
    public static Stack<UserAction> RedoStack = new Stack<UserAction>();
    public static bool canRedo = false;

    // Points and Shape Sizes that have been undone
    public static Stack<Vector3> PointStack = new Stack<Vector3>();
    public static Stack<int> ShapeSizeStack = new Stack<int>();


    public int GetNumberOfActions()
    {
        return ActionStack.Count;
    }

    public static void Reset()
    {
        ActionStack.Clear();
        RedoStack.Clear();
    }
}