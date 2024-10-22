using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script attatched to Load Button
/// </summary>
public class LoadScript : MonoBehaviour
{
    [SerializeField] private Button _loadButton;

    void Start()
    {
        _loadButton.onClick.AddListener(Load);
    }

    void Update()
    {
        // Check for CTRL/CMD + Z key combination
        bool isCtrlOrCmdPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ||
                                  Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand);

        // trigger load with key bind CTRL + O
        if (isCtrlOrCmdPressed && Input.GetKeyDown(KeyCode.O)) Load();
    }

    /// <summary>
    /// Load a canvas state from file
    /// </summary>
    void Load()
    {
        if (File.Exists(CanvasState.savePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CanvasStateData));

            using (FileStream stream = new FileStream(CanvasState.savePath, FileMode.Open))
            {
                CanvasStateData data = (CanvasStateData)serializer.Deserialize(stream);
                CanvasState.Instance.drawState = (CanvasState.DrawStates)System.Enum.Parse(typeof(CanvasState.DrawStates), data.drawState);
                CanvasState.Instance.uiState = data.uiState;
                CanvasState.Instance.hovering = data.hovering;

                ShapeManager.LoadShapes(data.shapes);
                Debug.Log("Canvas state loaded from " + CanvasState.savePath);
            }
            return;
        }

        Debug.LogWarning("Save file not found");
    }
}


