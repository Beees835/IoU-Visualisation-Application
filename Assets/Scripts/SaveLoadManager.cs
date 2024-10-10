using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private static string savePath = Application.persistentDataPath + "/canvasSave.xml";

    public static void SaveCanvasState(CanvasState canvasState)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(CanvasStateData));

        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            CanvasStateData data = new CanvasStateData
            {
                drawState = canvasState.drawState.ToString(),
                uiState = canvasState.uiState,
                hovering = canvasState.hovering,
                shapes = ShapeManager.GetShapesData()
            };

            serializer.Serialize(stream, data);
            Debug.Log("Canvas state saved to " + savePath);
        }
    }

    public static void LoadCanvasState()
    {
        if (File.Exists(savePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CanvasStateData));

            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                CanvasStateData data = (CanvasStateData)serializer.Deserialize(stream);
                CanvasState.Instance.drawState = (CanvasState.DrawStates)System.Enum.Parse(typeof(CanvasState.DrawStates), data.drawState);
                CanvasState.Instance.uiState = data.uiState;
                CanvasState.Instance.hovering = data.hovering;

                ShapeManager.LoadShapes(data.shapes);
                Debug.Log("Canvas state loaded from " + savePath);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
    }
}

[System.Serializable]
public class CanvasStateData
{
    public string drawState;
    public string uiState;
    public bool hovering;
    public ShapeData[] shapes;
}
