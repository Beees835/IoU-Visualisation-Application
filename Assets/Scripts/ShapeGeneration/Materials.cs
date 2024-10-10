using System.Linq;
using UnityEngine;

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

    public static GameObject GetPrefabType()
    {
        return ShapeManager.GetShapeCount() == 0
            ? Instance.PrefabShape1
            : ShapeManager.AllShapes.Last().PrefabType == Instance.PrefabShape1
                ? Instance.PrefabShape2
                : Instance.PrefabShape1;
    }
}
