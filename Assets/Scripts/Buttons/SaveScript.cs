using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class SaveScript : MonoBehaviour
{
    [SerializeField] private Button _saveButton;

    void Start()
    {
        _saveButton.onClick.AddListener(Save);
    }
    void Update()
    {
        bool isCtrlOrCmdPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                                  Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

        if (isCtrlOrCmdPressed && Input.GetKeyDown(KeyCode.S)) Save();
    }

    /// <summary>
    /// Save a canvas state to file
    /// </summary>
    void Save()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(CanvasStateData));

        using (FileStream stream = new FileStream(CanvasState.savePath, FileMode.Create))
        {
            CanvasStateData data = new CanvasStateData
            {
                drawState = CanvasState.Instance.drawState.ToString(),
                uiState = CanvasState.Instance.uiState,
                hovering = CanvasState.Instance.hovering,
                shapes = ShapeManager.GetShapesData()
            };

            serializer.Serialize(stream, data);
            Debug.Log("Canvas state saved to " + CanvasState.savePath);
        }
    }
}


