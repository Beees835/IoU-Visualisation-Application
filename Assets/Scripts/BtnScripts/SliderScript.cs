using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((val) => {
            _sliderText.text = Mathf.RoundToInt(val / maxZoom * 100).ToString() + "%";
            Camera.main.orthographicSize = Mathf.Clamp(maxZoom - val, minZoom, maxZoom);
        });
    }

}
