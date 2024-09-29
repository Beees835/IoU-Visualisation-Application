using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    public enum UserAction 
    {
        DRAW_POINT,
        DRAW_LINE,
        CLOSE_SHAPE,
        GENERATE_SHAPE
    }
    public static ActionManager Instance { get; private set; }
    public Stack<UserAction> ActionStack { get; set; } = new Stack<UserAction>();
    public Stack<UserAction> RedoStack { get; set; } = new Stack<UserAction>();

    private void Awake()
    {
    Instance = this;
    }


    public int GetNumberOfActions()
    {
        return ActionStack.Count;
    }

}