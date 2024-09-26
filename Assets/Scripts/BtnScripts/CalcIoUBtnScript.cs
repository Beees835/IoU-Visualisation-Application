using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CalcIoUBtnScript : MonoBehaviour
{

    [SerializeField] private GameObject _iouPanel;
    [SerializeField] private Button _calcIoUBtn;

    
    // Start is called before the first frame update
    void Start()
    {
        _calcIoUBtn.onClick.AddListener(OpenPanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel()
    {
        if (_iouPanel != null)
        {
            _iouPanel.SetActive(true);
        }
    }
}
