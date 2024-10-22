using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script attatched to save button
/// </summary>
public class SaveScript : MonoBehaviour
{
    [SerializeField] private Button _saveButton;

    void Start()
    {
        _saveButton.onClick.AddListener(Save);
    }
    void Update()
    {
        // Check for CTRL/CMD + Z key combination
        bool isCtrlOrCmdPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                                  Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

        // trigger save with key bind CTRL + S
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


