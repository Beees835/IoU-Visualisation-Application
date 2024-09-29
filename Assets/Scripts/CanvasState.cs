using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CanvasState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Enum for different states
    public enum DrawStates
    {
        DRAW_STATE,
        MODIFY_STATE,
        LOCK_STATE
    };

    public const int MAX_SHAPE_COUNT = 2;

    public bool hovering;

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

    // Start is called before the first frame update
    void Start()
    {
        this.drawState = DrawStates.DRAW_STATE;
        this.shapeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if ( shapeCount < MAX_SHAPE_COUNT)
        {
            drawState = hovering ? DrawStates.DRAW_STATE : DrawStates.LOCK_STATE;
            return;
        } else if (shapeCount == MAX_SHAPE_COUNT)
        {
            this.drawState = DrawStates.LOCK_STATE;
            IoUManager.CalculateIoUForShapes();
        } else if (this.shapeCount > MAX_SHAPE_COUNT) {
            this.drawState = DrawStates.MODIFY_STATE;
        }
    }
}
