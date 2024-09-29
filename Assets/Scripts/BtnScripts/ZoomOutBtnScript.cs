using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoomOutBtnScript : MonoBehaviour
{

    [SerializeField] private Button _zoomOutBtn;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    // Start is called before the first frame update
    void Start()
    {
        _zoomOutBtn.onClick.AddListener(ZoomOut);
    }

    void ZoomOut()
    {
        float val = Mathf.Clamp(Camera.main.orthographicSize + 1, minZoom, maxZoom);
        Camera.main.orthographicSize = val;
        _sliderText.text = Mathf.RoundToInt((maxZoom - val) / maxZoom * 100).ToString() + "%";
        _slider.value = maxZoom - val;
    }

}
