using UnityEngine;
using UnityEngine.UI;

public class CollapseMenuScript : MonoBehaviour
{
    [SerializeField] private Button _collapseBtn;
    [SerializeField] private GameObject _expandedMenu;
    [SerializeField] private GameObject _collapsedMenu;

    void Start()
    {
        _collapseBtn.onClick.AddListener(CollapseMenu);
    }

    /// <summary>
    /// Collapse the collapsable menu
    /// </summary>
    void CollapseMenu()
    {
        _expandedMenu.SetActive(false);
        _collapsedMenu.SetActive(true);
    }
}
