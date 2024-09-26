using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static IoUManager;

public class CalcIoUBtnScript : MonoBehaviour
{

    [SerializeField] private GameObject _iouPanel;
    [SerializeField] private Button _calcIoUBtn;
    [SerializeField] private Button _closeBtn;
    [SerializeField] private TextMeshProUGUI _iouText;

    
    // Start is called before the first frame update
    void Start()
    {
        _calcIoUBtn.onClick.AddListener(OpenPanel);
        _closeBtn.onClick.AddListener(ClosePanel);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel()
    {
        if (_iouPanel != null)
        {
            _iouText.text = CalculateIoUForShapes();
            _iouPanel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (_iouPanel != null)
        {
            _iouPanel.SetActive(false);
        }
    }

}
