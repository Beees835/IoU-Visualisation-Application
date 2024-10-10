using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems; 

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tipToShow;
    private float _timeToWait = 0.0000001f; 

    public void OnPointerEnter(PointerEventData eventData) 
    {   
        Debug.Log("Hovered");
        StopAllCoroutines();
        StartCoroutine(StartTimer()); 
    }

    public void OnPointerExit(PointerEventData eventData) 
    {   
        Debug.Log("Exited");
        StopAllCoroutines();
        HoverTipManager.OnMouseLoseFocus?.Invoke(); 
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_timeToWait); 
        HoverTipManager.OnMouseHover?.Invoke(tipToShow, Input.mousePosition);  // Input.mousePosition for hover position
    }
}