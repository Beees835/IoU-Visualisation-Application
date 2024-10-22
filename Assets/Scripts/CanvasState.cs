using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Enum for different states
    public enum DrawStates
    {
        DRAW_STATE,
        MODIFY_STATE,
        LOCK_STATE
    };

    // Variables
    public DrawStates drawState = DrawStates.DRAW_STATE;
    public string uiState = "";
    public bool hovering;

    // Singleton instance
    private static CanvasState _instance;

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

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        ShapeManager.CurrentShape = new Shape();
    }

    void Update()
    {
        // Check for CTRL/CMD key combination
        bool isCtrlOrCmdPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                                  Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

        // trigger save with key bind CTRL + S
        if (isCtrlOrCmdPressed && Input.GetKeyDown(KeyCode.S))
        {
            SaveState();

            // trigger load with key bind CTRL + O
        }
        else if (isCtrlOrCmdPressed && Input.GetKeyDown(KeyCode.O))
        {
            LoadState();
        }

        if (ShapeManager.CanAddMoreShapes())
        {
            drawState = hovering ? DrawStates.DRAW_STATE : DrawStates.LOCK_STATE;
            return;
        }
        IoUCalculator.CalculateIoUForShapes();
        drawState = DrawStates.MODIFY_STATE;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter Canvas");
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit Canvas");
        hovering = false;
    }

    public void Reset()
    {
        drawState = DrawStates.DRAW_STATE;
    }

    public void ClearCanvas()
    {
        ShapeManager.Reset();
        IoUCalculator.Reset();
        ActionManager.Reset();
        NotificationManager.Instance.ClearMessage();
        CanvasState.Instance.Reset();
    }

    // Save canvas state to XML
    public void SaveState()
    {
        SaveLoadManager.SaveCanvasState(this);
    }

    // Load canvas state from XML
    public void LoadState()
    {
        SaveLoadManager.LoadCanvasState();
    }
}
