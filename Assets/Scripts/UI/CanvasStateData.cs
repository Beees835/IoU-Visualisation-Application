using System.Collections.Generic;

/// <summary>
/// Serialized dataclass for canvas state information
/// </summary>
[System.Serializable]
public class CanvasStateData
{
    public string drawState;
    public string uiState;
    public bool hovering;
    public List<ShapeData> shapes;
}