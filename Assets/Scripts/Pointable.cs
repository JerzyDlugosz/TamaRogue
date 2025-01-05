using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerUpHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log($"Click! Drag:{eventData.dragging}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log($"Down! Drag:{eventData.dragging}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Enter! Drag:{eventData.dragging}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Exit! Drag:{eventData.dragging}");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log($"Move! Drag:{eventData.dragging}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log($"Up! Drag:{eventData.dragging}");
    }
}
