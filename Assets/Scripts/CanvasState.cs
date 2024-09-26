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
        uiState =   "Current Shape Count: " + shapeCount + "\n" + 
                    "Current Draw State: " + this.drawState + "\n" +
                    "Should start calc IoU: " + beginCalculatingIoUStatus;

        if (textMeshPro != null) {
            textMeshPro.text = uiState;
        }
        else {
            Debug.LogError("TMP_Text reference not found");
        }

        if (shapeCount >= MAX_SHAPE_COUNT) {
            drawState = DrawStates.LOCK_STATE;
        }

        if (shapeCount == MAX_SHAPE_COUNT && !beginCalculatingIoUStatus)
        {
            beginCalculatingIoU(); // Adds an extra shape (intersection shape)
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
