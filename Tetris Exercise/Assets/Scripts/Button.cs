using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInChildren<Text>().color = Color.black;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponentInChildren<Text>().color = Color.white;
    }

    public void ChangeToDefaultColor()
    {
        GetComponentInChildren<Text>().color = Color.white;
    }
}