using System.Linq;
using UnityEngine;

/// <summary>
/// Storage class for instance materials
/// </summary>
public class Materials : MonoBehaviour
{
    public GameObject PrefabShape1;
    public GameObject PrefabShape2;
    public GameObject invalidMarkPrefab;
    public Material LineMaterial;

    // Singleton instance
    private static Materials _instance;
    public const float LINE_WIDTH = 0.05f;

    // Properties
    public static Materials Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Materials>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(Materials).ToString());
                    _instance = singleton.AddComponent<Materials>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// Return a prefab for the current shape
    /// </summary>
    /// <returns>A prefab for vertices</returns>
    public static GameObject GetPrefabType()
    {
        return ShapeManager.GetShapeCount() == 0
            ? Instance.PrefabShape1
            : ShapeManager.AllShapes.Last().PrefabType == Instance.PrefabShape1
                ? Instance.PrefabShape2
                : Instance.PrefabShape1;
    }
}
