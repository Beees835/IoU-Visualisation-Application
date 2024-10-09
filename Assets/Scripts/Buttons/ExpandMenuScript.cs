using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandMenuScript : MonoBehaviour
{

    [SerializeField] private Button _expandBtn;
    [SerializeField] private GameObject _expandedMenu;
    [SerializeField] private GameObject _collapsedMenu;

    // Start is called before the first frame update
    void Start()
    {
        _expandBtn.onClick.AddListener(ExpandMenu);
    }

    public void ExpandMenu()
    {
        _expandedMenu.SetActive(true);
        _collapsedMenu.SetActive(false);
    }
}
