using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serialized dataclass for shape information
/// </summary>
[System.Serializable]
public class ShapeData
{
    public List<Vector3> points;
    public bool isClosed;
    public string prefabType;
}
