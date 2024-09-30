using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
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
    public static Stack<bool> DeleteCompletion = new Stack<bool>();


    public int GetNumberOfActions()
    {
        return ActionStack.Count;
    }

    public static Shape BuildShapeFromStack(GameObject prefabType)
    {
        Shape shape = new Shape();
        int shapeSize = ActionManager.ShapeSizeStack.Pop();

        for (int i = 0; i < shapeSize; i++)
        {
            Vector3 startPoint = PointStack.Pop();
            GameObject prefab = Instantiate(prefabType, startPoint, Quaternion.identity);
            shape.AddPoint(startPoint, prefab);
        }

        return shape;
    }

    public static void Reset()
    {
        ActionStack.Clear();
        RedoStack.Clear();
    }
}