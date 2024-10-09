using UnityEngine;

public class Materials : MonoBehaviour
{
    public GameObject PrefabShape1;
    public GameObject PrefabShape2;
    public GameObject invalidMarkPrefab;
    public Material LineMaterial;


    // Singleton instance
    private static Materials _instance;

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
        return CanvasState.Instance.shapeCount == 0 ? Materials.Instance.PrefabShape1 : Materials.Instance.PrefabShape2;
    }
}
