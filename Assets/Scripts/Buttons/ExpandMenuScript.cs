using UnityEngine;
using UnityEngine.UI;

public class ExpandMenuScript : MonoBehaviour
{
    [SerializeField] private Button _expandBtn;
    [SerializeField] private GameObject _expandedMenu;
    [SerializeField] private GameObject _collapsedMenu;

    void Start()
    {
        _expandBtn.onClick.AddListener(ExpandMenu);
    }

    /// <summary>
    /// Show the collapsable menu
    /// </summary>
    void ExpandMenu()
    {
        _expandedMenu.SetActive(true);
        _collapsedMenu.SetActive(false);
    }
}
