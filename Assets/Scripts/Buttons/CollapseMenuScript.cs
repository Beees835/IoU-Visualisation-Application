using UnityEngine;
using UnityEngine.UI;

public class CollapseMenuScript : MonoBehaviour
{

    [SerializeField] private Button _collapseBtn;
    [SerializeField] private GameObject _expandedMenu;
    [SerializeField] private GameObject _collapsedMenu;

    // Start is called before the first frame update
    void Start()
    {
        _collapseBtn.onClick.AddListener(CollapseMenu);
    }

    void CollapseMenu()
    {
        _expandedMenu.SetActive(false);
        _collapsedMenu.SetActive(true);
    }
}
