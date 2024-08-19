using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasState : MonoBehaviour
{
    // Enum for different states
    public enum DrawStates
    {
        DRAW_STATE,
        MODIFY_STATE,
        LOCK_STATE
    };

    private const int MAX_SHAPE_COUNT = 2;

    // Singleton instance
    private static CanvasState _instance;

    // Properties
    public static CanvasState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CanvasState>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(CanvasState).ToString());
                    _instance = singleton.AddComponent<CanvasState>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    // Variables
    public int shapeCount;
    public DrawStates drawState;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.drawState = DrawStates.DRAW_STATE;
        this.shapeCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Example code for using drawState
        // if (drawState == DrawStates.DRAW_STATE) 
        // {
        //     //code here
        // }
        // else if (drawState == DrawStates.MODIFY_STATE) 
        // {
        //     //code here
        // }
        // else if (drawState == DrawStates.LOCK_STATE) 
        // {
        //     //code here
        // }
    }
}
