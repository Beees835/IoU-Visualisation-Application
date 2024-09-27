using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasState : MonoBehaviour
{
    // Enum for different states
    public enum DrawStates
    {
        DRAW_STATE,
        MODIFY_STATE,
        LOCK_STATE
    };

    public const int MAX_SHAPE_COUNT = 2;

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
    public bool beginCalculatingIoUStatus = false;
    public string uiState = "";
    public TMP_Text textMeshPro;

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

        if (this.shapeCount >= 3) {
            this.drawState = DrawStates.MODIFY_STATE;
        }
        
        if (shapeCount >= 2 && !beginCalculatingIoUStatus)
        {
            beginCalculatingIoU(); // Adds an extra shape (intersection shape)
        }

        if (shapeCount < MAX_SHAPE_COUNT)
        {
            this.drawState = DrawStates.DRAW_STATE;
        }


        // Example code for using drawState
        // if (drawState == DrawStates.DRAW_STATE) sss
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

    public void beginCalculatingIoU() {
        beginCalculatingIoUStatus = true;
        this.drawState = DrawStates.LOCK_STATE;
    }
}
