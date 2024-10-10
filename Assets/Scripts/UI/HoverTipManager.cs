using System;
using UnityEngine;
using TMPro; 

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipTapWindow; 

    public static Action<string, Vector2> OnMouseHover; 
    public static Action OnMouseLoseFocus;

    public void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    public void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    void Start()
    {
        HideTip(); 
    }

    private void ShowTip(string text, Vector2 position)
    {
        tipText.text = text;
        tipTapWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);

        
        tipTapWindow.gameObject.SetActive(true); 
        tipTapWindow.transform.position = new Vector2(position.x + 15, position.y - 10);  // Slight offset to avoid overlap
    }

    private void HideTip()
    {   
        tipText.text = ""; // Clear the text when hiding 
        tipTapWindow.gameObject.SetActive(false); 
    }
}