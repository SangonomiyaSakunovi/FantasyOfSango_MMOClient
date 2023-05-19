using System;
using UnityEngine;
using UnityEngine.EventSystems;

//Developer : SangonomiyaSakunovi
//Discription: The Click Listener.

public class ClickListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Action<PointerEventData> onClickDown;   
    public Action<PointerEventData> onClickUp;
    public Action<PointerEventData> onDrag;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickDown != null)
        {
            onClickDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onClickUp != null)
        {
            onClickUp(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag!= null)
        {
            onDrag(eventData);
        }
    }
}
